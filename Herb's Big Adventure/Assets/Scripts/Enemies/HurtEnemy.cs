using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class HurtEnemy : MonoBehaviour
{
    PlayerController controller;

    private void OnTriggerEnter(Collider other)
    {
        Debug.LogWarning("confirm damage");
        Debug.LogWarning("NORMAL DAMAGE");

        other.GetComponent<EnemyHealthManager>().TakeDamageNormalAttack();
        
        //if (controller.weaponHurtBox.activeInHierarchy)
        //{
        //    Debug.LogWarning("WEAPON DAMAGE");
        //    other.GetComponent<EnemyHealthManager>().TakeDamageWeaponAttack();
        //}
        //if (controller.chargedHurtBox.activeInHierarchy)
        //{
        //    Debug.LogWarning("CHARGE DAMAGE");
        //    other.GetComponent<EnemyHealthManager>().TakeDamageChargedAttack();
        //}
    }
}
