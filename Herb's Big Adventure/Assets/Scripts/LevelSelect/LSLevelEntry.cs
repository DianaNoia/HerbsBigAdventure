using System.Collections;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.SceneManagement;

public class LSLevelEntry : MonoBehaviour
{
    public string levelName, levelToCheck, displayName;

    public bool canLoadLevel, levelUnlocked;

    [SerializeField] private GameObject mapPointActive, mapPointInactive;

    public bool levelLoading;

    // Loading screen variables
    [SerializeField] private GameObject loadingScreen, 
                                        loadingText,
                                        loadedText, 
                                        continueIntoLevelText;
    // Slider
    [SerializeField] private  Slider slider;

    // Timer to make loading wait
    private float loadingTimeForLoader = .2f;
    private float loadingTime = 2f;
    private float timer;

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
        }
        else
        {
            mapPointActive.SetActive(false);
            mapPointInactive.SetActive(true);
            levelUnlocked = false;
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
        if (Input.GetButtonDown("Jump") && canLoadLevel && levelUnlocked && 
            !levelLoading)
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
                    Debug.Log("entered timer");

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
        if (other.tag == "Player")
        {
            Debug.Log("ENTER COLLIDER");

            canLoadLevel = true;

            LSUIManager.instance.lNamePanel.SetActive(true);
            LSUIManager.instance.lNameText.text = displayName;
        }
    }

    private void OnTriggerExit(Collider other)
    {
        if (other.tag == "Player")
        {
            canLoadLevel = false;

            LSUIManager.instance.lNamePanel.SetActive(false);

            Debug.Log("EXIT COLLIDER");
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
