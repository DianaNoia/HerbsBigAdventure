using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class EnemyHealthManager : MonoBehaviour
{
    private PlayerController pc;
    private EnemyController ec;
    private AudioManager am;

    [SerializeField]
    private EnemyEffects enemyEffects;

    [SerializeField]
    public int maxHealth;
    [SerializeField]
    public int currentHealth;

    public int minHealth = 0;

    [SerializeField]
    private int deathSound;

    [SerializeField]
    private Animator anim;

    [SerializeField]
    private GameObject deathEffect, itemToDrop;

    // Start is called before the first frame update
    void Start()
    {
        currentHealth = maxHealth;
        
        pc = FindObjectOfType<PlayerController>();
        ec = FindObjectOfType<EnemyController>();
        am = FindObjectOfType<AudioManager>();
    }

private void Update()
    {
        enemyEffects.EffectSwapper();
    }

    // Damage from basic attack
    public void TakeDamageFromAttack()
    {
        Debug.Log("enemy took damage");

        currentHealth--;
        
        if (currentHealth <= 0)
        {
            am.PlaySFX(deathSound);

            Destroy(gameObject);

            Instantiate(deathEffect, transform.position + new Vector3(0f, 1.2f, 0f), transform.rotation);
            Instantiate(itemToDrop, transform.position + new Vector3(0f, .5f, 0f), transform.rotation);
        }

        anim.SetTrigger("Take Damage");
    }

    //// Damage from weapon swing
    //public void TakeDamageWeaponAttack()
    //{
    //    currentHealth -= 2;
    //    ec.anim.SetTrigger("Take Damage");

    //    if (currentHealth <= 0)
    //    {
    //        am.PlaySFX(deathSound);

    //        //ec.anim.SetTrigger("Die");

    //        Destroy(gameObject);

    //        Instantiate(deathEffect, transform.position + new Vector3(0f, 1.2f, 0f), transform.rotation);
    //        Instantiate(itemToDrop, transform.position + new Vector3(0f, .5f, 0f), transform.rotation);
    //    }
    //}

    //// Damage from charged attack
    //public void TakeDamageChargedAttack()
    //{        
    //    currentHealth -= 4;
    //    ec.anim.SetTrigger("Take Damage");

    //    if (currentHealth <= 0)
    //    {
    //        am.PlaySFX(deathSound);

    //        Destroy(gameObject);

    //        Instantiate(deathEffect, transform.position + new Vector3(0f, 1.2f, 0f), transform.rotation);
    //        Instantiate(itemToDrop, transform.position + new Vector3(0f, .5f, 0f), transform.rotation);
    //    }
    //}


    // Checks if the player hurtbox is colliding
    private void OnTriggerEnter(Collider other)
    {
        if (other.CompareTag("HurtBox"))
        {
            TakeDamageFromAttack();
        }
    }

}
