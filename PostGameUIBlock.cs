//By Miles King

using UnityEngine.UI;
using UnityEngine;
using UnityEngine.EventSystems;

//The individual player post game stats review
public class PostGameUIBlock : MonoBehaviour {

    [SerializeField]
    public int m_PlayerNumber;

    public enum PostGameMVP { First, Second, Third, Fourth }
    private Color[] m_MVPColors = new Color[4] {
        new Color(1, 0.84f, 0),             //Gold
        new Color(0.75f, 0.75f, 0.75f),     //Silver
        new Color(0.31f, 0.19f, 0.078f),    //Bronze
        new Color(0, 0, 0, 0)               //Transparent
    };

    //Manage the block via states
    private enum BlockState { Total, Review, Waiting }
    private BlockState m_BlockState;

    [SerializeField]
    private Text m_PlayerText;
    [SerializeField]
    private Image m_ContinueImage, m_ReturnImage;
    [SerializeField]
    private RectTransform m_Content;
    [SerializeField]
    private Text m_StatsText;
    [SerializeField]
    private Text m_LargeText;
    [SerializeField]
    private Image m_MVPImage;
    [SerializeField]
    private RectTransform m_Panel;
    [SerializeField]
    private Scrollbar m_ScrollBar;

    //Where we get our stats from, also our player
    private PlayerStats m_Player;
    private GamePad m_Controller;

    private PostGameBehaviour m_PostGameBehaviour;

    //Simple way to focus only on controller input or keyboard
    private delegate void ControllerSpecificUpdate();
    private ControllerSpecificUpdate m_ControllerUpdate;

    private BlockState m_PreviousState;

    public RectTransform GetRectTransform()
    {
        return m_Panel;
    }

    /// <summary>
    /// Turn on a ui block for the player to see, should only be called by post game behaviour
    /// </summary>
    public void Init(PostGameBehaviour postGameBehaviour, PostGameMVP mvpStatus)
    {
        if (m_ReturnImage == null)
        {
            m_ReturnImage = null;
        }

        m_PlayerText.text = "Player " + (m_PlayerNumber + 1);

        m_MVPImage.color = m_MVPColors[(int)mvpStatus];

        m_PostGameBehaviour = postGameBehaviour;

        GameObject player = GameManager.playerManager.PlayerList()[m_PlayerNumber];
        m_Player = player.GetComponent<PlayerStats>();

        

        Controllers controllerType = player.GetComponent<PlayerSettings>().controller;

        m_Controller = GameManager.controllerInput.GetController(controllerType);
        SwitchState((int)BlockState.Total);

        if(controllerType == Controllers.KEYBOARD_MOUSE)
        {
            m_ContinueImage.sprite = m_PostGameBehaviour.EnterSprite;
            m_ReturnImage.sprite = m_PostGameBehaviour.BackspaceSprite;

            m_ControllerUpdate = KeyboardUpdate;
        }
        else
        {
            m_ContinueImage.sprite = m_PostGameBehaviour.ASprite;
            m_ReturnImage.sprite = m_PostGameBehaviour.BSprite;

            m_ControllerUpdate = ControllerUpdate;
        }
    }

    private void Update ()
    {
        if (m_Player != null)
        {
            m_ControllerUpdate();
        }
	}

    private void Start()
    {
        EventTrigger forwardTrigger = m_ContinueImage.gameObject.AddComponent<EventTrigger>();
        EventTrigger returnTrigger = m_ReturnImage.gameObject.AddComponent<EventTrigger>();

        EventTrigger.Entry clickEntryForward = new EventTrigger.Entry();
        clickEntryForward.eventID = EventTriggerType.PointerDown;
        clickEntryForward.callback.AddListener((data) => { ForwardState(); });

        EventTrigger.Entry clickEntryBackwards = new EventTrigger.Entry();
        clickEntryBackwards.eventID = EventTriggerType.PointerDown;
        clickEntryBackwards.callback.AddListener((data) => { BackState(); });

        forwardTrigger.triggers.Add(clickEntryForward);
        returnTrigger.triggers.Add(clickEntryBackwards);
    }

    private void KeyboardUpdate()
    {
        if (Input.GetKeyDown(KeyCode.Return)) ForwardState();
        else if (Input.GetKeyDown(KeyCode.Backspace)) BackState();
    }

    private void ControllerUpdate()
    {
        if (m_Controller.GetButton(GAMEPAD_BUTTONS.A_BUTTON, BUTTON_STATES.PRESSED)) ForwardState();
        else if (m_Controller.GetButton(GAMEPAD_BUTTONS.B_BUTTON, BUTTON_STATES.PRESSED)) BackState();
    }

    /// <summary>
    /// One shot state switch
    /// </summary>
    private void SwitchState(int toState)
    {
        if (toState > 2 || toState < 0) return;

        m_PreviousState = m_BlockState;
        m_BlockState = (BlockState)toState;
        switch(m_BlockState)
        {
            case BlockState.Review:
                SetReview();
                break;
            case BlockState.Total:
                SetTotal();
                break;
            case BlockState.Waiting:
                SetWaiting();
                break;
        }
    }

    /// <summary>
    /// Move a state forward
    /// </summary>
    private void ForwardState()
    {
        SwitchState((int)m_BlockState + 1);
    }

    /// <summary>
    /// Move a state backwards
    /// </summary>
    private void BackState()
    {
        SwitchState((int)m_BlockState - 1);
    }

    private void SetTotal()
    {
        m_LargeText.gameObject.SetActive(true);
        m_LargeText.text = "Total Score: " + m_Player.ScorePointsEarned;

        m_StatsText.gameObject.SetActive(false);

        m_ContinueImage.gameObject.SetActive(true);
        m_ReturnImage.gameObject.SetActive(false);
    }

    private void SetReview()
    {
        m_StatsText.text = string.Empty;

        m_LargeText.gameObject.SetActive(false);

        m_StatsText.gameObject.SetActive(true);

        #region Stats Print

        m_StatsText.text += "Downed: " + m_Player.GetDowns() + DevUI.BackSlashN;
        m_StatsText.text += "Revived: " + m_Player.GetRevives() + DevUI.BackSlashN;
        m_StatsText.text += "Damage: " + m_Player.DamageDealt + DevUI.BackSlashN;
        m_StatsText.text += "Score: " + m_Player.ScorePointsEarned + DevUI.BackSlashN;

        #endregion

        float height = LayoutUtility.GetPreferredHeight(m_StatsText.rectTransform) + DevUI.MinContentSize;
        m_Content.sizeDelta = new Vector2(m_Content.sizeDelta.x, height);

        m_ContinueImage.gameObject.SetActive(true);
        m_ReturnImage.gameObject.SetActive(true);

        if (m_PreviousState == BlockState.Waiting) m_PostGameBehaviour.SetUnready();
    }

    private void SetWaiting()
    {
        m_LargeText.gameObject.SetActive(true);
        m_LargeText.text = "Waiting for other players";

        m_StatsText.gameObject.SetActive(false);

        m_ContinueImage.gameObject.SetActive(false);
        m_ReturnImage.gameObject.SetActive(true);

        m_PostGameBehaviour.SetReady();
    }
}
