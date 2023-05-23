using System.Collections;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.SceneManagement;

public class LSLevelEntry : MonoBehaviour
{
    public string levelName, levelToCheck, displayName;

    private bool canLoadLevel, levelUnlocked;

    public GameObject mapPointActive, mapPointInactive;

    private bool levelLoading;

    // Reference to the game loading screen and bar
    public GameObject loadingScreen;
    public Slider loadingBar;


    // Start is called before the first frame update
    void Start()
    {
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
        PlayerController.instance.stopMove = true;
        //    UIManager.instance.fadeToBlack = true;

        //    yield return new WaitForSeconds(2f);

        AsyncOperation operation = SceneManager.LoadSceneAsync(levelName);

        //    // SceneManager.LoadScene(levelName);

        loadingScreen.SetActive(true);

        while (!operation.isDone)
        {
            float progress = Mathf.Clamp01(operation.progress / .9f);

            Debug.Log("time to load level:" + operation.progress);

            //WaitForSecondsRealtime waitToLoad = new WaitForSecondsRealtime(2f); 

            loadingBar.value = progress;

            // yield return waitToLoad;
            yield return null;
        }

        PlayerPrefs.SetString("CurrentLevel", levelName);
    }

    public  void OnTriggerEnter(Collider other)
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
