using System.Collections;
using System.Collections.Generic;
using UnityEngine.UI;
using UnityEngine;
using System;

public class PauseMenuUI : MonoBehaviour
{
    public Button ResumeButton;
    public Button SettingsButton;
    public Button ExitButton;
    public Button BackButton;
    public Button QuitButton;
    public Button ConfirmQuitButton;
    public Button DeclineQuitButton;
    public Slider BrightnessSlider;

    public SliderControl ControllerSliderControl;

    public Text DisplayText;

    //private WaveSpawner m_WaveSpawnerRef;
    private UINavigation m_UINavRef;

    // Use this for initialization
    void Start()
    {
        m_UINavRef = GetComponentInParent<UINavigation>();
        m_UINavRef.MenuButton = new Button[] { ResumeButton, SettingsButton, ExitButton, QuitButton };
        //Array.Reverse(m_UINavRef.MenuButton);

        ControllerSliderControl.enabled = false;

        ResumeButton.onClick.AddListener(ResumeButtonEvent);
        SettingsButton.onClick.AddListener(SettingsButtonEvent);
        ExitButton.onClick.AddListener(ExitButtonEvent);
        BackButton.onClick.AddListener(BackButtonEvent);
        QuitButton.onClick.AddListener(QuitButtonEvent);
        ConfirmQuitButton.onClick.AddListener(ConfirmQuitButtonEvent);
        DeclineQuitButton.onClick.AddListener(DeclineQuitButtonEvent);

        //BackButton.gameObject.SetActive(false);
    }

    // Update is called once per frame
    void Update()
    {

    }

    void ResumeButtonEvent()
    {
        //m_WaveSpawnerRef = GetComponentInParent<Game>().Spawner;
        Cursor.visible = false;
        Cursor.lockState = CursorLockMode.Locked;

        Time.timeScale = 1;
        gameObject.SetActive(false);

        GameObject.Find("Game").GetComponent<PlayerInput>().paused = false;
    }

    void SettingsButtonEvent()
    {
        m_UINavRef.MenuButton = new Button[] { SettingsButton };
        //Array.Reverse(m_UINavRef.MenuButton);

        DisplayText.text = "Settings";

        ControllerSliderControl.enabled = true;

        BrightnessSlider.gameObject.SetActive(true);
        BackButton.gameObject.SetActive(true);

        ResumeButton.gameObject.SetActive(false);
        SettingsButton.gameObject.SetActive(false);
        ExitButton.gameObject.SetActive(false);
        QuitButton.gameObject.SetActive(false);



    }

    void ExitButtonEvent()
    {
        for (int i = 0; i < GameManager.playerManager.PlayerList().Count; i++)
        {
            GameManager.playerManager.PlayerList()[i].SetActive(false);
        }
        GameManager.musicManager.PlayMusic(GameManager.musicManager.MusicTracks[4], true, true);
        GameManager.sceneManager.SwitchScenes("PostGame_Scene");
    }

    void BackButtonEvent()
    {
        m_UINavRef.MenuButton = new Button[] { ResumeButton, SettingsButton, ExitButton, QuitButton };
        Array.Reverse(m_UINavRef.MenuButton);

        ControllerSliderControl.enabled = false;

        DisplayText.text = "Game Paused";
        ResumeButton.gameObject.SetActive(true);
        SettingsButton.gameObject.SetActive(true);
        ExitButton.gameObject.SetActive(true);
        QuitButton.gameObject.SetActive(true);

        BrightnessSlider.gameObject.SetActive(false);
        BackButton.gameObject.SetActive(false);
    }

    void QuitButtonEvent()
    {
        m_UINavRef.MenuButton = new Button[] { ConfirmQuitButton, DeclineQuitButton };
        Array.Reverse(m_UINavRef.MenuButton);

        DisplayText.text = "Are you sure?";

        ConfirmQuitButton.gameObject.SetActive(true);
        DeclineQuitButton.gameObject.SetActive(true);

        ResumeButton.gameObject.SetActive(false);
        SettingsButton.gameObject.SetActive(false);
        ExitButton.gameObject.SetActive(false);
        QuitButton.gameObject.SetActive(false);

    }

    void ConfirmQuitButtonEvent()
    {
        #if UNITY_EDITOR
        UnityEditor.EditorApplication.isPlaying = false;
        #endif

        Application.Quit();
    }

    void DeclineQuitButtonEvent()
    {
        m_UINavRef.MenuButton = new Button[] { ResumeButton, SettingsButton, ExitButton, QuitButton };
        Array.Reverse(m_UINavRef.MenuButton);

        DisplayText.text = "Game Paused";

        ConfirmQuitButton.gameObject.SetActive(false);
        DeclineQuitButton.gameObject.SetActive(false);

        ResumeButton.gameObject.SetActive(true);
        SettingsButton.gameObject.SetActive(true);
        ExitButton.gameObject.SetActive(true);
        QuitButton.gameObject.SetActive(true);
    }
}
