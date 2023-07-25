using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class HealthPickup : MonoBehaviour
{
    private HealthManager hm;
    private AudioManager am;

    public int healAmount;
    public bool isFullHeal;

    public GameObject healEffect;

    public int soundToPlay;
    private void Start()
    {
        hm = FindObjectOfType<HealthManager>();
        am = FindObjectOfType<AudioManager>(); ;

    }

    private void OnTriggerEnter(Collider other)
    {
        if (other.tag == "Player")
        {
            Destroy(gameObject);


            if (isFullHeal)
            {
                hm.ResetHealth();
            }
            else
            {
                hm.AddHealth(healAmount);
            }            
            
            // health effect
            Instantiate(healEffect, transform.position, transform.rotation);

            am.PlaySFX(soundToPlay);
        }
    }
}
