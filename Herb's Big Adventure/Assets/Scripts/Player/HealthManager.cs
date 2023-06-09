using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class HealthManager : MonoBehaviour
{
    private HealthManager hm;
    private GameManager gm;
    private UIManager uim;
    private AudioManager am;
    private PlayerController pc;

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
        hm = this;
    }

    // Start is called before the first frame update
    void Start()
    {
        gm = FindObjectOfType<GameManager>();
        am = FindObjectOfType<AudioManager>(); 
        pc = FindObjectOfType<PlayerController>();
        hm = FindObjectOfType<HealthManager>();
        uim = FindObjectOfType<UIManager>();

        ResetHealth();
        invicibilityCheat = false;
    }

    // Update is called once per frame
    void Update()
    {
        if(invincibleCounter > 0)
        {
            invincibleCounter -= Time.deltaTime;

            for (int i = 0; i < pc.playerPieces.Length; i++)
            {
                if(Mathf.Floor(invincibleCounter * 5f) % 2 == 0)
                {
                    pc.playerPieces[i].SetActive(true);
                }
                else
                {
                    pc.playerPieces[i].SetActive(false);
                }

                if(invincibleCounter <= 0)
                {
                    pc.playerPieces[i].SetActive(true);
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
                gm.Respawn();
            }
            else
            {
                pc.KnockBack();
                invincibleCounter = invincibleLength;
            }

            UpdateUI();

            am.PlaySFX(soundToPlay);
        }
        else if (invicibilityCheat == true)
        {
            currentHealth = maxHealth;
        }
    }

    public void ResetHealth()
    {
        currentHealth = maxHealth;
        uim.healthImage.enabled = true;

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
        uim.healthText.text = currentHealth.ToString();

        switch (currentHealth)
        {
            case 4:
                uim.healthImage.sprite = healthBarImgs[4];
                break;
            case 3:
                uim.healthImage.sprite = healthBarImgs[3];
                break;
            case 2:
                uim.healthImage.sprite = healthBarImgs[2];
                break;
            case 1:
                uim.healthImage.sprite = healthBarImgs[1];
                break;
            case 0:
                uim.healthImage.sprite = healthBarImgs[0];
                break;
        }
    }

    public void PlayerKilled()
    {
        currentHealth = 0;
        UpdateUI();

        am.PlaySFX(soundToPlay);
    }
}
