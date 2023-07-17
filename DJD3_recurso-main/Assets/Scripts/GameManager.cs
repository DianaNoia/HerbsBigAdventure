using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class GameManager : MonoBehaviour
{
    public static GameManager instace;

    private Vector3 respawnPosition;

    public GameObject deathEffect;

    public int currentCoins;

    public int levelEndMusic = 8;

    public string levelToLoad;

    private void Awake()
    {
        instace = this;
    }

    // Start is called before the first frame update
    void Start()
    {
        Cursor.visible = false;
        Cursor.lockState = CursorLockMode.Locked;

        respawnPosition = PlayerController.instance.transform.position;

        AddCoins(0);
    }

    private void Update()
    {
        if (Input.GetKeyDown(KeyCode.Escape))
        {
            PauseUnpause();
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

    public void AddCoins(int coinsToAdd)
    {
        currentCoins += coinsToAdd;
        UIManager.instance.coinText.text = currentCoins.ToString();
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
            Cursor.lockState = CursorLockMode.None;
        }
    }

    public IEnumerator LevelEndCo()
    {
        AudioManager.instance.PlayMusic(levelEndMusic);
        PlayerController.instance.stopMove = true;

        UIManager.instance.fadeToBlack = true;

        yield return new WaitForSeconds(2f);
        Debug.Log("level ended");

        PlayerPrefs.SetInt(SceneManager.GetActiveScene().name + "_unlocked", 1);

        if (PlayerPrefs.HasKey(SceneManager.GetActiveScene().name + "_coins"))
        {
            if (currentCoins > PlayerPrefs.GetInt(SceneManager.GetActiveScene().name + "_coins"))
            {
                PlayerPrefs.SetInt(SceneManager.GetActiveScene().name + "_coins", currentCoins);
            }
        }
        else
        {
            PlayerPrefs.SetInt(SceneManager.GetActiveScene().name + "_coins", currentCoins);
        }


        SceneManager.LoadScene(levelToLoad);   
    }
}
