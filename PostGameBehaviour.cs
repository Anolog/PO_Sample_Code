//By Miles King

using UnityEngine.UI;
using UnityEngine;

public class PostGameBehaviour : MonoBehaviour
{

    //UI space and sizing
    private const float PlayerBlockWidth = 200;
    private const float ExraSize = 100.0f;
    private const float Padding = 20.0f;

    [SerializeField]
    private bool m_CloseOnReady = true;

    [SerializeField]
    private bool m_DontDestroyOnLoad = true;

    [SerializeField]
    private PostGameUIBlock[] m_PlayerUIBlocks;

    [SerializeField]
    private Sprite m_ASprite, m_BSprite;

    [SerializeField]
    private Sprite m_EnterSprite, m_BackspaceSprite;

    [SerializeField]
    private Text m_MissionStatusText;

    [SerializeField]
    private GameObject m_Canvas;

    [SerializeField]
    private RectTransform m_MainPanel;

    private int m_ReadyCount = 0;

    #region Set/Get

    public Sprite ASprite
    {
        get
        {
            return m_ASprite;
        }

        set
        {
            m_ASprite = value;
        }
    }

    public Sprite BSprite
    {
        get
        {
            return m_BSprite;
        }

        set
        {
            m_BSprite = value;
        }
    }

    public Sprite EnterSprite
    {
        get
        {
            return m_EnterSprite;
        }

        set
        {
            m_EnterSprite = value;
        }
    }

    public Sprite BackspaceSprite
    {
        get
        {
            return m_BackspaceSprite;
        }

        set
        {
            m_BackspaceSprite = value;
        }
    }

    #endregion

    private void Start()
    {
        if (m_DontDestroyOnLoad) DontDestroyOnLoad(gameObject);
        ShowPostGame(GameManager.playerManager.PlayerList()[0].GetComponent<PlayerStats>().WinOrLoseGame);
        //For whatever reason this is needed.
        GameManager.musicManager.AudioSource.Stop();
        GameManager.musicManager.PlayMusic(GameManager.musicManager.MusicTracks[4], true, true);

    }

    public int GetReadyCount()
    {
        return m_ReadyCount;
    }

    public bool GetAllPlayersReady()
    {
        int numPlayers = GameManager.playerManager.numberOfPlayers;

        return m_ReadyCount == numPlayers;
    }

    public void SetReady()
    {
        m_ReadyCount++;

        int numPlayers = GameManager.playerManager.numberOfPlayers;

        if (m_ReadyCount == numPlayers && m_CloseOnReady) ClosePostGame();
    }

    public void SetUnready()
    {
        m_ReadyCount--;
    }

    public void ShowPostGame(bool isMissionSuccessful)
    {
        m_ReadyCount = 0;

        if (isMissionSuccessful)
        {
            //GameManager.musicManager.PlayMusic(GameManager.musicManager.MusicTracks[4], true, true);
            m_MissionStatusText.text = "Mission Complete";
        }

        else
        {
            //GameManager.musicManager.PlayMusic(GameManager.musicManager.MusicTracks[4], true, true);
            m_MissionStatusText.text = "Mission Failure";
        }

        m_Canvas.SetActive(true);

        int numPlayers = GameManager.playerManager.numberOfPlayers;

        //Set all of them to not show. If you don't do this it bugs out.
        for (int i = 0; i < 4; i++)
        {
            m_PlayerUIBlocks[i].gameObject.SetActive(false);
        }

        //float evenOffset = numPlayers % 2 == 0 ? (Padding + PlayerBlockWidth) * 0.5f : 0;


        float panelPosition = (numPlayers - 2) * -(PlayerBlockWidth - Padding * 2);
        //float panelPosition = (numPlayers - 2) * -(Padding + PlayerBlockWidth) - evenOffset;


        if (numPlayers == 1) panelPosition = 0;

        for (int i = 0; i < numPlayers; i++)
        {
            m_PlayerUIBlocks[i].gameObject.SetActive(true);
            m_PlayerUIBlocks[i].GetRectTransform().anchoredPosition = new Vector2(panelPosition, 0);
            m_PlayerUIBlocks[i].m_PlayerNumber = i;
            m_PlayerUIBlocks[i].Init(this, PostGameUIBlock.PostGameMVP.First);
            panelPosition += (Padding + PlayerBlockWidth);
        }

        //Hard code these for now
        if (numPlayers == 1)
        {
            m_PlayerUIBlocks[0].GetComponent<RectTransform>().localPosition = new Vector3(0, -50, 0);

        }

        if (numPlayers == 2)
        {
            //if two player
            m_PlayerUIBlocks[0].GetComponent<RectTransform>().localPosition = new Vector3(0 - PlayerBlockWidth / 2 - Padding, -50, 0);
            m_PlayerUIBlocks[1].GetComponent<RectTransform>().localPosition = new Vector3(0 + PlayerBlockWidth / 2 + Padding, -50, 0);
        }


        if (numPlayers == 3)
        {
            m_PlayerUIBlocks[0].GetComponent<RectTransform>().localPosition = new Vector3(0 - PlayerBlockWidth - Padding, -50, 0);
            m_PlayerUIBlocks[1].GetComponent<RectTransform>().localPosition = new Vector3(m_PlayerUIBlocks[0].GetComponent<RectTransform>().localPosition.x + PlayerBlockWidth + Padding, -50, 0);
            m_PlayerUIBlocks[2].GetComponent<RectTransform>().localPosition = new Vector3(m_PlayerUIBlocks[1].GetComponent<RectTransform>().localPosition.x + PlayerBlockWidth + Padding, -50, 0);
        }

        if (numPlayers == 4)
        {
            m_PlayerUIBlocks[0].GetComponent<RectTransform>().localPosition = new Vector3(0 - (PlayerBlockWidth * 1.5f) - Padding, -50, 0);
            m_PlayerUIBlocks[1].GetComponent<RectTransform>().localPosition = new Vector3(m_PlayerUIBlocks[0].GetComponent<RectTransform>().localPosition.x + PlayerBlockWidth + Padding, -50, 0);
            m_PlayerUIBlocks[2].GetComponent<RectTransform>().localPosition = new Vector3(m_PlayerUIBlocks[1].GetComponent<RectTransform>().localPosition.x + PlayerBlockWidth + Padding, -50, 0);
            m_PlayerUIBlocks[3].GetComponent<RectTransform>().localPosition = new Vector3(m_PlayerUIBlocks[2].GetComponent<RectTransform>().localPosition.x + PlayerBlockWidth + Padding, -50, 0);
        }
    }

    public void ClosePostGame()
    {
        if (GetAllPlayersReady())
        {
            int numPlayers = GameManager.playerManager.numberOfPlayers;

            for (int i = 0; i < numPlayers; i++)
            {
                m_PlayerUIBlocks[i].gameObject.SetActive(false);
            }

            m_Canvas.SetActive(false);


            //Destroy players
            DestroyPlayers();

            //Delete game manager to wipe info about the game and prevent memory leak
            Destroy(GameObject.FindGameObjectWithTag("GameManager"));
            //Go back to main menu
            GameManager.sceneManager.SwitchScenes("Main_Menu");
        }
    }

    public void DestroyPlayers()
    {
        int numPlayers = GameManager.playerManager.numberOfPlayers;

        for (int i = 0; i < numPlayers; i++)
        {
            Destroy(GameManager.playerManager.PlayerList()[i]);
        }
    }

}
