using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class LevelExit : MonoBehaviour
{
    PlayerController pc;
    public Animator anim;

    public bool givesWeapon;

    private void OnTriggerEnter(Collider other)
    {
        if (other.tag == "Player")
        {
            anim.SetTrigger("Hit");
            StartCoroutine(GameManager.instance.LevelEndCo());
        }

        if (givesWeapon == true)
        {
            pc.hasWeapon = true;
        }
    }
}
