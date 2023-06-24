using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class HurtPlayerExplosion : MonoBehaviour
{
    private HealthManager hm;

    private void Start()
    {
        hm = FindObjectOfType<HealthManager>();
    }

    private void OnTriggerEnter(Collider other)
    {
        if (other.tag == "Player")
        {
            hm.HurtExplosion();
        }
    }
}
