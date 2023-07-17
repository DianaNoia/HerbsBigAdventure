using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class HealthPickup : MonoBehaviour
{
    public int healAmount;
    public bool isFullHeal;

    public GameObject healEffect;


    public int soundToPlay;

    private void OnTriggerEnter(Collider other)
    {
        if (other.tag == "Player")
        {
            Destroy(gameObject);


            if (isFullHeal)
            {
                HealthManager.instance.ResetHealth();
            }
            else
            {
                HealthManager.instance.AddHealth(healAmount);
            }            
            
            // health effect
            Instantiate(healEffect, transform.position, transform.rotation);

            AudioManager.instance.PlaySFX(soundToPlay);
        }
    }
}
