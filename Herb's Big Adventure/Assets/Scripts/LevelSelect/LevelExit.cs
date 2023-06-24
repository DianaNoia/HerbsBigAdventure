using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class LevelExit : MonoBehaviour
{
    private PlayerController pc;
    private GameManager gm;
    private UIManager uim;

    public Animator anim;

    public bool givesWeapon;

    public bool hasGivenWeapon;

    private void Start()
    {
        pc = FindObjectOfType<PlayerController>();
        gm = FindObjectOfType<GameManager>();
        uim = FindObjectOfType<UIManager>();

        hasGivenWeapon = false;
    }

    private void OnTriggerEnter(Collider other)
    {
        if (other.tag == "Player")
        {
            StartCoroutine(EndDelay());

            StartCoroutine(gm.LevelEndCo());
        }
    }
    
    private IEnumerator EndDelay()
    {
        uim.endScreen.SetActive(true);
        yield return new WaitForSeconds(6f);
    }
}
