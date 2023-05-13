using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class EnemyHealthManager : MonoBehaviour
{
    PlayerController pc;
    EnemyController ec;

    public int maxHealth = 1;
    private int currentHealth;

    public int deathSound;

    public Animator anim;

    public GameObject deathEffect, itemToDrop;

    // Start is called before the first frame update
    void Start()
    {
        currentHealth = maxHealth;
    }

    public void TakeDamageNormalAttack()
    {
        Debug.Log("enemy took damage");

        currentHealth--;
        

        if (currentHealth <= 0)
        {
            AudioManager.instance.PlaySFX(deathSound);

            Destroy(gameObject);

            Instantiate(deathEffect, transform.position + new Vector3(0f, 1.2f, 0f), transform.rotation);
            Instantiate(itemToDrop, transform.position + new Vector3(0f, .5f, 0f), transform.rotation);
        }

        anim.SetTrigger("Take Damage");
    }
    public void TakeDamageWeaponAttack()
    {
        currentHealth -= 2;
        ec.anim.SetTrigger("Take Damage");

        if (currentHealth <= 0)
        {
            AudioManager.instance.PlaySFX(deathSound);

            ec.anim.SetTrigger("Die");

            Destroy(gameObject);

            Instantiate(deathEffect, transform.position + new Vector3(0f, 1.2f, 0f), transform.rotation);
            Instantiate(itemToDrop, transform.position + new Vector3(0f, .5f, 0f), transform.rotation);
        }
    }
    public void TakeDamageChargedAttack()
    {        
        currentHealth -= 4;
        ec.anim.SetTrigger("Take Damage");

        if (currentHealth <= 0)
        {
            AudioManager.instance.PlaySFX(deathSound);

            Destroy(gameObject);

            Instantiate(deathEffect, transform.position + new Vector3(0f, 1.2f, 0f), transform.rotation);
            Instantiate(itemToDrop, transform.position + new Vector3(0f, .5f, 0f), transform.rotation);
        }
    }
}
