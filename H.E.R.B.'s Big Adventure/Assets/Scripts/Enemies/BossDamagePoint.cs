using System.Collections;
using System.Collections.Generic;
using UnityEngine;

// Class that controls boss damage point
public class BossDamagePoint : MonoBehaviour
{
    private BossController bc;

    private void Start()
    {
        bc = FindObjectOfType<BossController>();
    }
    private void OnTriggerEnter(Collider other)
    {
        if (other.tag == "HurtBox")
        {
            bc.DamageBoss();
        }
    }
}
