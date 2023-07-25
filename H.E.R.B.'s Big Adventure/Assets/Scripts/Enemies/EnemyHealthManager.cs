using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class EnemyHealthManager : MonoBehaviour
{
    private PlayerController pc;
    //private EnemyControllerYellow ecy;
    //private EnemyControllerRed ecr;
    //private EnemyControllerPurple ecp;
    private AudioManager am;


    [SerializeField]
    private EnemyEffects enemyEffects;

    [SerializeField]
    public int maxHealth;
    [SerializeField]
    public int currentHealth;

    public int minHealth = 0;

    [SerializeField]
    private int deathSound, soundToPlay;

    [SerializeField]
    private Animator anim;

    [SerializeField]
    public GameObject deathEffect, itemToDrop;

    // Start is called before the first frame update
    void Start()
    {
        currentHealth = maxHealth;
        
        pc = FindObjectOfType<PlayerController>();
        //ecy = FindObjectOfType<EnemyControllerYellow>();
        //ecr = FindObjectOfType<EnemyControllerRed>();
        //ecp = FindObjectOfType<EnemyControllerPurple>();
        am = FindObjectOfType<AudioManager>();
    }

private void Update()
    {
        enemyEffects.EffectSwapper();
    }

    // Damage from basic attack
    public void TakeDamageFromAttack()
    {
        currentHealth--; 
        
        am.PlaySFX(soundToPlay);

        if (currentHealth <= 0)
        {
            am.PlaySFX(deathSound);

            //Destroy(gameObject);
            
            Debug.Log("enemy died");

            Destroy(gameObject);
            Instantiate(deathEffect, transform.position + new Vector3(0f, 1.2f, 0f), transform.rotation);
            Instantiate(itemToDrop, transform.position + new Vector3(0f, .5f, 0f), transform.rotation);
        }
        else
        {
            Debug.Log("enemy took damage");
            anim.SetTrigger("Take Damage");
        }
    }

    // Checks if the player hurtbox is colliding
    private void OnTriggerEnter(Collider other)
    {
        if (other.CompareTag("HurtBox"))
        {
            TakeDamageFromAttack();
        }
    }

}
