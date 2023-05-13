using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class HealthManager : MonoBehaviour
{
    public static HealthManager instance;

    [SerializeField]
    private int currentHealth, maxHealth;

    [SerializeField]
    private bool invicibilityCheat;

    public float invincibleLength = 1f;
    private float invincibleCounter;

    [SerializeField]
    private Sprite[] healthBarImgs;

    [SerializeField]
    private int soundToPlay;

    private void Awake()
    {
        instance = this;
    }

    // Start is called before the first frame update
    void Start()
    {
        ResetHealth();
        invicibilityCheat = false;
    }

    // Update is called once per frame
    void Update()
    {
        if(invincibleCounter > 0)
        {
            invincibleCounter -= Time.deltaTime;

            for (int i = 0; i < PlayerController.instance.playerPieces.Length; i++)
            {
                if(Mathf.Floor(invincibleCounter * 5f) % 2 == 0)
                {
                    PlayerController.instance.playerPieces[i].SetActive(true);
                }
                else
                {
                    PlayerController.instance.playerPieces[i].SetActive(false);
                }

                if(invincibleCounter <= 0)
                {
                    PlayerController.instance.playerPieces[i].SetActive(true);
                }
            }
        }

        // Make Player invicible
        if (Input.GetKey(KeyCode.Alpha6))
        {
            Debug.Log("Invicible");
            invicibilityCheat = true;
        }
    }

    public void Hurt()
    {
        if (invincibleCounter <= 0 && invicibilityCheat == false)
        {
            currentHealth--;

            if (currentHealth <= 0)
            {
                currentHealth = 0;
                GameManager.instance.Respawn();
            }
            else
            {
                PlayerController.instance.KnockBack();
                invincibleCounter = invincibleLength;
            }

            UpdateUI();

            AudioManager.instance.PlaySFX(soundToPlay);
        }
        else if (invicibilityCheat == true)
        {
            currentHealth = maxHealth;
        }
    }

    public void ResetHealth()
    {
        currentHealth = maxHealth;
        UIManager.instance.healthImage.enabled = true;

        UpdateUI();
    }

    public void AddHealth(int amountToHeal)
    {
        currentHealth += amountToHeal;
        if (currentHealth > maxHealth)
        {
            currentHealth = maxHealth;
        }

        UpdateUI();
    }

    public void UpdateUI()
    {
        UIManager.instance.healthText.text = currentHealth.ToString();

        switch (currentHealth)
        {
            case 4:
                UIManager.instance.healthImage.sprite = healthBarImgs[4];
                break;
            case 3:
                UIManager.instance.healthImage.sprite = healthBarImgs[3];
                break;
            case 2:
                UIManager.instance.healthImage.sprite = healthBarImgs[2];
                break;
            case 1:
                UIManager.instance.healthImage.sprite = healthBarImgs[1];
                break;
            case 0:
                UIManager.instance.healthImage.sprite = healthBarImgs[0];
                break;
        }
    }

    public void PlayerKilled()
    {
        currentHealth = 0;
        UpdateUI();

        AudioManager.instance.PlaySFX(soundToPlay);
    }
}
