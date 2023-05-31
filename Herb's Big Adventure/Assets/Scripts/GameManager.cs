using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class GameManager : MonoBehaviour
{
    public static GameManager instance;
    PlayerController pc;

    private Vector3 respawnPosition;

    [SerializeField]
    private GameObject deathEffect;

    [SerializeField]
    public int      currentNormalBolts,
                    currentGoldenBolts;

    [SerializeField]
    private int levelEndMusic = 8;

    [SerializeField]
    public string levelToLoad;

    [SerializeField]
    public string currentLevel;

    [SerializeField]
    public bool isRespawning;

    // [SerializeField]
    // private bool controlsShown = false;

    // Cheat to skip boss level and start close to boss
    [SerializeField]
    private  GameObject player;
    [SerializeField]
    private Transform playerTransform, destinationTransform ;

    private void Awake()
    {
        instance = this;
    }

    // Start is called before the first frame update
    void Start()
    {
        currentLevel = SceneManager.GetActiveScene().name;

        // Locks and hides the cursor
        Cursor.visible = false;
        Cursor.lockState = CursorLockMode.Locked;

        // Respawn position reset
        respawnPosition = PlayerController.instance.transform.position;

        //OpenControls();
    }

    private void Update()
    {
        // Pausing the game
        if (Input.GetKeyDown(KeyCode.Escape))
        {
            Debug.Log("Pausing!!");
            PauseUnpause();
        }

        // Add keybinds for numbers for cheats to skip levels
        // Go to level select
        if (Input.GetKeyDown(KeyCode.Alpha1))
        {
            SceneManager.LoadScene("LevelSelect");
        }
        // Go to Level 1
        if (Input.GetKeyDown(KeyCode.Alpha2))
        {
            SceneManager.LoadScene("Level1");
        }
        // Go to Level 2
        if (Input.GetKeyDown(KeyCode.Alpha3))
        {
            SceneManager.LoadScene("Level3");
        }
        // Go to boss level
        if (Input.GetKeyDown(KeyCode.Alpha4))
        {
            SceneManager.LoadScene("Boss");
        }
        // Place player closer to goal
        if (Input.GetKeyDown(KeyCode.Alpha5))
        {
            Debug.Log("Change players position");
            player.SetActive(false);
            playerTransform.position = destinationTransform.position;
            player.SetActive(true);
        }
    }

    public void Respawn()
    {
        StartCoroutine(RespawnCo());

        HealthManager.instance.PlayerKilled();
    }

    // Deactivates player, waits and sets it back in respawn position, activating it again
    public IEnumerator RespawnCo()
    {
        PlayerController.instance.gameObject.SetActive(false);

        CameraController.instance.theCMBrain.enabled = false;

        // Fades screen to black after dying
        UIManager.instance.fadeToBlack = true;

        // Death effect
        Instantiate(deathEffect, PlayerController.instance.transform.position + new Vector3(0f, 1f, 0f), PlayerController.instance.transform.rotation);

        yield return new WaitForSeconds(2f);

        isRespawning = true;

        // Resets health when respawn
        HealthManager.instance.ResetHealth();

        // Fades screen back when respawning
        UIManager.instance.fadeFromBlack = true;

        PlayerController.instance.transform.position = respawnPosition;

        CameraController.instance.theCMBrain.enabled = true;

        PlayerController.instance.gameObject.SetActive(true);
    }

    public void SetSpawnPoint(Vector3 newSpawnPoint)
    {
        respawnPosition = newSpawnPoint;
    }
    
    public void AddNormalBolts(int normalBoltsToAdd)
    {
        currentNormalBolts += normalBoltsToAdd;
        UIManager.instance.boltsText.text = currentNormalBolts.ToString();
    }

    public void AddGoldenBolts(int goldenBoltsToAdd)
    {
        currentGoldenBolts += goldenBoltsToAdd;
        UIManager.instance.goldenBoltsText.text = currentGoldenBolts.ToString();
    }

    public void PauseUnpause()
    {
        if (UIManager.instance.pauseScreen.activeInHierarchy)
        {
            UIManager.instance.pauseScreen.SetActive(false);
            Time.timeScale = 1f;

            Cursor.visible = false;
            Cursor.lockState = CursorLockMode.Locked;
        }
        else
        {
            UIManager.instance.pauseScreen.SetActive(true);
            UIManager.instance.CloseOptions();
            Time.timeScale = 0f;

            Cursor.visible = true;
            Cursor.lockState = CursorLockMode.Confined;
        }
    }

    //public void OpenControls()
    //{
    //    if (currentLevel == "Level1")
    //    {
    //        if (UIManager.instance.controlsScreen.activeInHierarchy && controlsShown)
    //        {
    //            UIManager.instance.controlsScreen.SetActive(false);
    //            Time.timeScale = 1f;

    //            Cursor.visible = false;
    //            Cursor.lockState = CursorLockMode.Locked;
    //        }
    //        else if (!UIManager.instance.controlsScreen.activeInHierarchy && !controlsShown)
    //        {
    //            UIManager.instance.controlsScreen.SetActive(true);
    //            Time.timeScale = 0f;

    //            controlsShown = true;

    //            Cursor.visible = true;
    //            Cursor.lockState = CursorLockMode.None;
    //        }            
    //    }
    //}

    public IEnumerator LevelEndCo()
    {
        AudioManager.instance.PlayMusic(levelEndMusic);
        PlayerController.instance.stopMove = true;

        UIManager.instance.fadeToBlack = true;

        yield return new WaitForSeconds(2f);
        Debug.Log("level ended");

        PlayerPrefs.SetInt(SceneManager.GetActiveScene().name + "_unlocked", 1);

        if (PlayerPrefs.HasKey(SceneManager.GetActiveScene().name + "_normalBolts"))
        {
            if (currentNormalBolts > PlayerPrefs.GetInt(SceneManager.GetActiveScene().name + "_normalBolts"))
            {
                PlayerPrefs.SetInt(SceneManager.GetActiveScene().name + "_normalBolts", currentNormalBolts);
            }
        }
        else
        {
            PlayerPrefs.SetInt(SceneManager.GetActiveScene().name + "_normalBolts", currentNormalBolts);
        }

        if (PlayerPrefs.HasKey(SceneManager.GetActiveScene().name + "_goldenBolts"))
        {
            if (currentGoldenBolts > PlayerPrefs.GetInt(SceneManager.GetActiveScene().name + "_goldenBolts"))
            {
                PlayerPrefs.SetInt(SceneManager.GetActiveScene().name + "_goldenBolts", currentGoldenBolts);
            }
        }
        else
        {
            PlayerPrefs.SetInt(SceneManager.GetActiveScene().name + "_goldenBolts", currentGoldenBolts);
        }

        SceneManager.LoadScene(levelToLoad);   
    }
}
