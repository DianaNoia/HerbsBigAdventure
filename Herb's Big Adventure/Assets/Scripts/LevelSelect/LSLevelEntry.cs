using System.Collections;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.SceneManagement;

public class LSLevelEntry : MonoBehaviour
{
    [SerializeField]
    private LSUIManager lsUIManager;
    [SerializeField]
    private LevelCompletion levelCompletion;

    [SerializeField]
    public string levelName, 
                    levelToCheck, 
                    displayName, 
                    valueOfNormalBoltsEachLevel;

    public bool canLoadLevel,
                levelUnlocked;

    public bool levelLoading;

    [SerializeField] private GameObject mapPointActive, 
                                        mapPointInactive,
                                        loadingScreen, 
                                        loadingText,
                                        loadedText, 
                                        continueIntoLevelText, textIfLevelUnlocked, textIfLevelLocked;
                                        
    // Slider
    [SerializeField] private  Slider slider;

    // Timer to make loading wait
    private float loadingTimeForLoader = .2f;
    private float loadingTime = 2f;
    private float timer;

    //public int goldenBoltsValue = PlayerPrefs.GetInt("_goldenBolts", 0);

    // Start is called before the first frame update
    void Start()
    {
        // Set loading bar timer to 0;
        timer = 0f;

        if (PlayerPrefs.GetInt(levelToCheck + "_unlocked") == 1 || levelToCheck == "")
        {
            mapPointActive.SetActive(true);
            mapPointInactive.SetActive(false);
            levelUnlocked = true;
            //textIfLevelUnlocked.SetActive(true);
            //textIfLevelLocked.SetActive(false);
        }
        else
        {
            mapPointActive.SetActive(false);
            mapPointInactive.SetActive(true);
            levelUnlocked = false;
            //textIfLevelUnlocked.SetActive(false);
            //textIfLevelLocked.SetActive(true);

        }

        

        if (PlayerPrefs.GetString("CurrentLevel") == levelName)
        {
            PlayerController.instance.transform.position = transform.position;
            LSResetPosition.instance.respawnPosition = transform.position;
        }
    }

    // Update is called once per frame
    void Update()
    {
        // Loads level when space is pressed
        if (Input.GetButtonDown("Jump") && canLoadLevel && levelUnlocked && !levelLoading)
        {
            LoadLevel();
        }
    }

    // Runs coroutine to load level
    public void LoadLevel()
    {
        StartCoroutine(LoadSceneAsynchronously(levelName));
        levelLoading = true;
    }

    // Loads the scene asynchronously and shows a loading screen
    public IEnumerator LoadSceneAsynchronously(string levelName)
    {
        // Stops player, changes background to black
        PlayerController.instance.stopMove = true;
        UIManager.instance.fadeToBlack = true;

        // Turns on the loading screen
        loadingScreen.SetActive(true);

        // Loads actual level scene
        AsyncOperation levelScene = SceneManager.LoadSceneAsync(levelName);
        levelScene.allowSceneActivation = false;

        // Waits before fake loading bar starts
        yield return new WaitForSeconds(loadingTimeForLoader);

        while (!levelScene.isDone)
        {
            if (levelScene.progress >= 0.9f)
            {
                if (timer < loadingTime)
                {
                    timer += Time.deltaTime;
                    UpdateProgressBar();
                }
                else
                {
                    loadingText.SetActive(false);
                    loadedText.SetActive(true);
                    continueIntoLevelText.SetActive(true);

                    if (Input.GetKeyDown(KeyCode.Space))
                    {
                        // Loading is almost complete, allow the target scene to activate
                        levelScene.allowSceneActivation = true;
                    }
                }
            }

            yield return null;
        }

        PlayerPrefs.SetString("CurrentLevel", levelName);
    }

    private void UpdateProgressBar()
    {
        float progress = timer / loadingTime;
        slider.value = progress;
    }

    public void OnTriggerEnter(Collider other)
    {
        Debug.Log("OnTriggerEnter called");

        int goldenBolts = PlayerPrefs.GetInt(levelName + "_goldenBolts");

        if (other.tag == "Player")
        {
            canLoadLevel = true;

            lsUIManager.lNamePanel.SetActive(true);
            lsUIManager.lNameText.text = displayName;

            if (PlayerPrefs.HasKey(levelName + "_normalBolts"))
            {
                lsUIManager.normalBoltsText.text = PlayerPrefs.GetInt(levelName + "_normalBolts").ToString() + " / " + valueOfNormalBoltsEachLevel.ToString();
            }
            else
            {
                lsUIManager.normalBoltsText.text = "?";
            }

            if (PlayerPrefs.HasKey(levelName + "_goldenBolts"))
            {
                Debug.Log("Quantity of golden bolts: " + PlayerPrefs.GetInt(levelName + "_goldenBolts").ToString());

                // When there is 1 gb unlocked in a level
                if (goldenBolts  == 1)
                {
                    lsUIManager.goldenBolts1.SetActive(true);

                    lsUIManager.placeHolder1.SetActive(false);
                    lsUIManager.placeHolder2.SetActive(true);
                    lsUIManager.placeHolder3.SetActive(true);
                }
                // When there is 2 gb unlocked in a level
                else if (goldenBolts == 2)
                {
                    lsUIManager.goldenBolts1.SetActive(true);
                    lsUIManager.goldenBolts2.SetActive(true);

                    lsUIManager.placeHolder1.SetActive(false);
                    lsUIManager.placeHolder2.SetActive(false);
                    lsUIManager.placeHolder3.SetActive(true);
                }
                // When there is 3 gb unlocked in a level
                else if (goldenBolts == 3)
                {
                    lsUIManager.goldenBolts1.SetActive(true);
                    lsUIManager.goldenBolts2.SetActive(true);
                    lsUIManager.goldenBolts3.SetActive(true);

                    lsUIManager.placeHolder1.SetActive(false);
                    lsUIManager.placeHolder2.SetActive(false);
                    lsUIManager.placeHolder3.SetActive(false);
                }
                // When there is 0 gb unlocked in a level
                else if (goldenBolts == 0)
                {
                    lsUIManager.goldenBolts1.SetActive(false);
                    lsUIManager.goldenBolts2.SetActive(false);
                    lsUIManager.goldenBolts3.SetActive(false);

                    lsUIManager.placeHolder1.SetActive(true);
                    lsUIManager.placeHolder2.SetActive(true);
                    lsUIManager.placeHolder3.SetActive(true);
                }
            }
        }

        levelCompletion.CalculatePercentComplete();

        if (levelUnlocked)
        {
            Debug.Log("Level is unlocked: " + levelName);

            textIfLevelUnlocked.SetActive(true);
            textIfLevelLocked.SetActive(false);
        }

        if (!levelUnlocked)
        {
            Debug.Log("Level is locked: " + levelName);

            textIfLevelUnlocked.SetActive(false);
            textIfLevelLocked.SetActive(true);
        }
    }

    private void OnTriggerExit(Collider other)
    {
        Debug.Log("OnTriggerExit called");

        if (other.tag == "Player")
        {
            canLoadLevel = false;

            lsUIManager.lNamePanel.SetActive(false);

            lsUIManager.goldenBolts1.SetActive(false);
            lsUIManager.goldenBolts2.SetActive(false);
            lsUIManager.goldenBolts3.SetActive(false);

            lsUIManager.placeHolder1.SetActive(true);
            lsUIManager.placeHolder2.SetActive(true);
            lsUIManager.placeHolder3.SetActive(true);
        }
    }

    //public IEnumerator LevelLoadCo()
    //{
    //    Debug.Log("LOADING");
    //    PlayerController.instance.stopMove = true;
    //    UIManager.instance.fadeToBlack = true;

    //    yield return new WaitForSeconds(2f);

    //    SceneManager.LoadScene(levelName);
    //    PlayerPrefs.SetString("CurrentLevel", levelName);
    //}


    //public void LoadScene(int levelIndex)
    //{
    //    StartCoroutine(LoadSceneAsynchronously());
    //}

}
