using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class MainMenu : MonoBehaviour
{
    private LSLevelEntry lSLevelEntry;

    [SerializeField]
    private string firstLevel, 
                    levelSelect;

    [SerializeField]
    private GameObject continueButton, 
                        backToQuit;

    [SerializeField]
    private string[] levelNames;

    // Start is called before the first frame update
    void Start()
    {
        lSLevelEntry = FindObjectOfType<LSLevelEntry>();

        if (PlayerPrefs.HasKey("Continue"))
        {
            continueButton.SetActive(true);
        }
        else
        {
            ResetProgress();
        }
    }
    private void Update()
    {
        Cursor.visible = true;
        Cursor.lockState = CursorLockMode.Confined;
    }

    public void NewGame()
    {
        SceneManager.LoadScene(levelSelect);

        PlayerPrefs.SetInt("Continue", 0);
        PlayerPrefs.SetString("CurrentLevel", firstLevel);

        ResetProgress();
    }

    public void Continue()
    {
        SceneManager.LoadScene(levelSelect);
    }

    // Opens panel asking if you want to quit
    public void AreYouSureQuit()
    {
        // activates the prompt
        backToQuit.SetActive(true);
    }

    // If press YES
    public void QuitGame()
    {
        Application.Quit();
        Debug.Log("quit");
    }
    public void AreYouSureQuit_No()
    {
        // deactivates the prompt
        backToQuit.SetActive(false);
    }

    public void ResetProgress()
    {
        for (int i = 0; i < levelNames.Length; i++)
        {
            PlayerPrefs.SetInt(levelNames[i] + "_unlocked", 0);
            PlayerPrefs.SetInt(levelNames[i] + "_goldenBolts", 0);
            PlayerPrefs.SetInt(levelNames[i] + "_normalBolts", 0);
        }
    }
}
