using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CrateController : MonoBehaviour
{
    private PlayerController pc;

    private int maxHealth = 1;
    private int currentHealth;

    [SerializeField]
    private int breakSound;

    [SerializeField]
    private GameObject breakEffect, itemToDrop;

    // Start is called before the first frame update
    void Start()
    {
        currentHealth = maxHealth;
    }

    // Damage from fist
    public void TakeDamageNormalAttack()
    {
        Debug.Log("broke crate");

        currentHealth--;

        if (currentHealth <= 0)
        {
            AudioManager.instance.PlaySFX(breakSound);

            Destroy(gameObject);

            Instantiate(breakEffect, transform.position + new Vector3(0f, 1.2f, 0f), transform.rotation);
            Instantiate(itemToDrop, transform.position + new Vector3(0f, .5f, 0f), transform.rotation);
        }
    }
}
