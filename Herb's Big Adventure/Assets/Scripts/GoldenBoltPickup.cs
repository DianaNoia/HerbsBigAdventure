using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GoldenBoltPickup : MonoBehaviour
{
    public int value;

    //public GameObject pickupEffect;

    public int soundToPlay;

    private void OnTriggerEnter(Collider other)
    {
        if (other.tag == "Player")
        {
            GameManager.instance.AddGoldenBolts(value);

            Destroy(gameObject);

            //Instantiate(pickupEffect, transform.position, transform.rotation);

            AudioManager.instance.PlaySFX(soundToPlay);
        }
    }
}
