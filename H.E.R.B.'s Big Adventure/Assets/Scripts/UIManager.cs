using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.SceneManagement;

public class UIManager : MonoBehaviour
{
    private UIManager uim;

    [SerializeField]
    private GameManager gameManager;

    public Image blackScreen,
                    healthImage;

    public float fadeSpeed = 2f;
    public bool fadeToBlack = false, fadeFromBlack;

    public Text healthText,
                boltsText,
                level1PercentageText,
                level2PercentageText,
                level3PercentageText,
                level4PercentageText;

    public GameObject goldenBoltImage,
                        goldenBoltImage2,
                        goldenBoltImage3,
                        goldenBoltImageTransparent,
                        goldenBoltImageTransparent2,
                        goldenBoltImageTransparent3,
                        pauseScreenInGame,
                        pauseScreenInLevelSelect,
                        optionsScreen,
                        backToLevelSelect,
                        backToMainMenu_InGame,
                        backToMainMenu_InLS,
                        levelCompletionScreen,
                        cheatInvincible,
                        cheatTeleport,
                        endScreen;
     
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
        cheatInvincible.SetActive(false);
        cheatTeleport.SetActive(false);
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

    // Resumes game
    public void Resume() 
    {
        gameManager.PauseUnpause();
    }

    // Open option panel
    public void OpenOptions()
    {
        optionsScreen.SetActive(true);
        pauseScreenInGame.SetActive(false);
        pauseScreenInLevelSelect.SetActive(false);
        levelCompletionScreen.SetActive(false);
    }

    // Close option panel
    public void CloseOptions() 
    {
        optionsScreen.SetActive(false);

        // Checks if the we are in the level select or in game pause screen
        if (pauseScreenInLevelSelect.activeInHierarchy)
        {
            pauseScreenInLevelSelect.SetActive(false);
            levelCompletionScreen.SetActive(false);
        }
        if (pauseScreenInGame.activeInHierarchy)
        {
            pauseScreenInGame.SetActive(false);
        }
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
        // deactivates the level completion screen
        levelCompletionScreen.SetActive(false);

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
        // activates the level completion screen
        levelCompletionScreen.SetActive(true);

        // deactivates the prompt
        backToMainMenu_InLS.SetActive(false);
    }

    //public void SetMusicLevel()
    //{
    //    uim.SetMusicLevel();
    //}

    //public void SetSFXLevel()
    //{
    //    uim.SetSFXLevel();
    //}
}
