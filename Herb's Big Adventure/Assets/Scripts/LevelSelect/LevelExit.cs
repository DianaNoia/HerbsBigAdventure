using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class LevelExit : MonoBehaviour
{
    private PlayerController pc;
    private GameManager gm;

    public Animator anim;

    public bool givesWeapon;


    private void Start()
    {
        pc = FindObjectOfType<PlayerController>();
        gm = FindObjectOfType<GameManager>();

    }
    private void OnTriggerEnter(Collider other)
    {
        if (other.tag == "Player")
        {
            anim.SetTrigger("Hit");
            StartCoroutine(gm.LevelEndCo());
        }

        if (givesWeapon == true)
        {
            pc.hasWeapon = true;
        }
    }
}
