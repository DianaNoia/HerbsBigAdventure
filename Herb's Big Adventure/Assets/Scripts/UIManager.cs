using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.SceneManagement;

public class UIManager : MonoBehaviour
{
    private GameManager gm;
    private UIManager uim;

    public Image blackScreen,
                    healthImage;

    public float fadeSpeed = 2f;
    public bool fadeToBlack = false, fadeFromBlack;

    public Text healthText,
                boltsText,
                goldenBoltsText;

    public GameObject goldenBoltImage, 
                        goldenBoltImage2, 
                        goldenBoltImage3, 
                        goldenBoltImageTransparent, 
                        goldenBoltImageTransparent2, 
                        goldenBoltImageTransparent3;

    public GameObject pauseScreenInGame,
                        pauseScreenInLevelSelect,
                        optionsScreen,
                        controlsScreen,
                        backToLevelSelect,
                        backToMainMenu_InGame,
                        backToMainMenu_InLS;

    public Slider musicVolSlider, 
                    sfxVolSlider;

    public string levelSelect, 
                    mainMenu;

    public void Awake()
    {
        uim = this;
    }

    private void Start()
    {
        uim = FindObjectOfType<UIManager>();
        gm = FindObjectOfType<GameManager>();
    }

    // Update is called once per frame
    void Update()
    {
        if (fadeToBlack)
        {
            blackScreen.color = new Color(blackScreen.color.r, 
                blackScreen.color.g, 
                blackScreen.color.b, 
                Mathf.MoveTowards(blackScreen.color.a, 1f, fadeSpeed * Time.deltaTime));

            if (blackScreen.color.a == 1f)
            {
                fadeToBlack = false;
            }
        }

        if (fadeFromBlack)
        {
            blackScreen.color = new Color(blackScreen.color.r,
                blackScreen.color.g,
                blackScreen.color.b,
                Mathf.MoveTowards(blackScreen.color.a, 0f, fadeSpeed * Time.deltaTime));

            if (blackScreen.color.a == 0f)
            {
                fadeFromBlack = false;
            }
        }
    }

    public void Resume() 
    {
        gm.PauseUnpause();
    }

    public void OpenOptions()
    {
        optionsScreen.SetActive(true);
    }

    public void CloseOptions() 
    {
        optionsScreen.SetActive(false);
    }

    // Opens panel asking if you want to go back to level select
    public void AreYouSureLS()
    {
        // deactivates the pause screens
        pauseScreenInGame.SetActive(false);
        pauseScreenInLevelSelect.SetActive(false);

        // activates the prompt
        backToLevelSelect.SetActive(true);
    }

    // If press YES
    public void LevelSelect()
    {
        SceneManager.LoadScene(levelSelect);

        Time.timeScale = 1f;
    }

    // If press NO in game
    public void AreYouSureLS_NO()
    {
        // activates the pause screen 
        pauseScreenInGame.SetActive(true);

        // deactivates the prompt
        backToLevelSelect.SetActive(false);
    }

    // Opens panel asking if you want to go back to main menu WHILE IN GAME
    public void AreYouSureMM_InGame()
    {
        // deactivates the pause screen
        pauseScreenInGame.SetActive(false);

        // activates the prompt
        backToMainMenu_InGame.SetActive(true);
    }

    // Opens panel asking if you want to go back to main menu WHILE IN LEVEL SELECT
    public void AreYouSureMM_InLS()
    {
        // deactivates the pause screen
        pauseScreenInLevelSelect.SetActive(false);

        // activates the prompt
        backToMainMenu_InLS.SetActive(true);
    }

    // If press YES
    public void MainMenu()
    {
        SceneManager.LoadScene(mainMenu);

        Time.timeScale = 1f;
    }

    // If press NO in game
    public void AreYouSureMM_NO_InGame()
    {
        // activates the pause screen 
        pauseScreenInGame.SetActive(true);

        // deactivates the prompt
        backToMainMenu_InGame.SetActive(false);
    }

    // If press NO in Level select
    public void AreYouSureMM_NO_InLS()
    {
        // activates the pause screen 
        pauseScreenInLevelSelect.SetActive(true);

        // deactivates the prompt
        backToMainMenu_InLS.SetActive(false);
    }

    public void SetMusicLevel()
    {
        uim.SetMusicLevel();
    }

    public void SetSFXLevel()
    {
        uim.SetSFXLevel();
    }
}
