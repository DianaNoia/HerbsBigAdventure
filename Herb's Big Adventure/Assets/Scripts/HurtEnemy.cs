using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class HurtEnemy : MonoBehaviour
{
    PlayerController controller;

    // Update is called once per frame
    void Update()
    {
        
    }

    private void OnTriggerEnter(Collider other)
    {
        if(other.tag == "Enemy")
        {
            controller.hurtBox.SetActive(false);
            other.GetComponent<EnemyHealthManager>().TakeDamage();
        }
    }
}
