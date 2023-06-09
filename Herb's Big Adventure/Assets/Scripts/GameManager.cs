using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class GameManager : MonoBehaviour
{
    private HealthManager hm;
    private UIManager uim;
    public PlayerController pc;
    private AudioManager am;
    private GameManager gm;

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

    private string currentScene;

    // [SerializeField]
    // private bool controlsShown = false;

    // Cheat to skip boss level and start close to boss
    [SerializeField]
    private  GameObject player;
    [SerializeField]
    private Transform playerTransform, destinationTransform ;

    private void Awake()
    {
        gm = this;
    }

    // Start is called before the first frame update
    void Start()
    {
        currentLevel = SceneManager.GetActiveScene().name;

        // Locks and hides the cursor
        Cursor.visible = false;
        Cursor.lockState = CursorLockMode.Locked;

        pc = FindObjectOfType<PlayerController>();
        hm = FindObjectOfType<HealthManager>();
        uim = FindObjectOfType<UIManager>();
        am = FindObjectOfType<AudioManager>();
        gm = FindObjectOfType<GameManager>();

        // Respawn position reset
        respawnPosition = pc.transform.position;
        
        currentScene = SceneManager.GetActiveScene().name;
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

        hm.PlayerKilled();
    }

    // Deactivates player, waits and sets it back in respawn position, activating it again
    public IEnumerator RespawnCo()
    {
        PlayerController.instance.gameObject.SetActive(false);

        //CameraController.instance.theCMBrain.enabled = false;

        // Fades screen to black after dying
        uim.fadeToBlack = true;

        // Death effect
        Instantiate(deathEffect, PlayerController.instance.transform.position + new Vector3(0f, 1f, 0f), PlayerController.instance.transform.rotation);

        yield return new WaitForSeconds(2f);

        isRespawning = true;

        // Resets health when respawn
        hm.ResetHealth();

        // Fades screen back when respawning
        uim.fadeFromBlack = true;

        PlayerController.instance.transform.position = respawnPosition;

        //CameraController.instance.theCMBrain.enabled = true;

        PlayerController.instance.gameObject.SetActive(true);
    }

    public void SetSpawnPoint(Vector3 newSpawnPoint)
    {
        respawnPosition = newSpawnPoint;
    }
    
    public void AddNormalBolts(int normalBoltsToAdd)
    {
        currentNormalBolts += normalBoltsToAdd;
        uim.boltsText.text = currentNormalBolts.ToString();
    }

    public void AddGoldenBolts(int goldenBoltsToAdd)
    {
        currentGoldenBolts += goldenBoltsToAdd;
        if (currentGoldenBolts == 1)
        {
            uim.goldenBoltImage.SetActive(true);
            uim.goldenBoltImage2.SetActive(false);
            uim.goldenBoltImage3.SetActive(false);
            uim.goldenBoltImageTransparent.SetActive(false);
            uim.goldenBoltImageTransparent2.SetActive(true);
            uim.goldenBoltImageTransparent3.SetActive(true);
        }
        if (currentGoldenBolts == 2)
        {
            uim.goldenBoltImage.SetActive(true);
            uim.goldenBoltImage2.SetActive(true);
            uim.goldenBoltImage3.SetActive(false);
            uim.goldenBoltImageTransparent.SetActive(false);
            uim.goldenBoltImageTransparent2.SetActive(false);
            uim.goldenBoltImageTransparent3.SetActive(true);
        }
        if (currentGoldenBolts == 3)
        {
            uim.goldenBoltImage.SetActive(true);
            uim.goldenBoltImage2.SetActive(true);
            uim.goldenBoltImage3.SetActive(true);
            uim.goldenBoltImageTransparent.SetActive(false);
            uim.goldenBoltImageTransparent2.SetActive(false);
            uim.goldenBoltImageTransparent3.SetActive(false);
        }
        //uim.goldenBoltsText.text = currentGoldenBolts.ToString();
    }

    public void PauseUnpause()
    {
        if (currentScene == "LevelSelect")
        {
            if (uim.pauseScreenInLevelSelect.activeInHierarchy)
            {
                uim.pauseScreenInLevelSelect.SetActive(false);
                Time.timeScale = 1f;

                Cursor.visible = false;
                Cursor.lockState = CursorLockMode.Locked;
            }
            else
            {
                uim.pauseScreenInLevelSelect.SetActive(true);
                uim.CloseOptions();
                Time.timeScale = 0f;

                Cursor.visible = true;
                Cursor.lockState = CursorLockMode.Confined;
            }
        }
        else
        {
            if (uim.pauseScreenInGame.activeInHierarchy)
            {
                uim.pauseScreenInGame.SetActive(false);
                Time.timeScale = 1f;

                Cursor.visible = false;
                Cursor.lockState = CursorLockMode.Locked;
            }
            else
            {
                uim.pauseScreenInGame.SetActive(true);
                uim.CloseOptions();
                Time.timeScale = 0f;

                Cursor.visible = true;
                Cursor.lockState = CursorLockMode.Confined;
            }
        }
    }

    public IEnumerator LevelEndCo()
    {
        am.PlayMusic(levelEndMusic);
        PlayerController.instance.stopMove = true;

        uim.fadeToBlack = true;

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
