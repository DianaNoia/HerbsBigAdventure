using System.Collections;
using UnityEngine;

public class GoldenBoltPickup : MonoBehaviour
{
    private GameManager gm;
    private AudioManager am;

    public int value;

    public GameObject pickupEffect;

    public int soundToPlay;

    private Animator anim;

    private void Start()
    {
        am = FindObjectOfType<AudioManager>();
        gm = FindObjectOfType<GameManager>();
        anim = GetComponent<Animator>();
    }

    private void OnTriggerEnter(Collider other)
    {
        if (other.tag == "Player")
        {
            gm.AddGoldenBolts(value);

            anim.SetTrigger("PickedUp");

            Instantiate(pickupEffect, transform.position, transform.rotation);

            am.PlaySFX(soundToPlay);

            StartCoroutine(WaitToDestroy());
        }
    }
    private IEnumerator WaitToDestroy()
    {
        yield return new WaitForSeconds(1f);
        Destroy(gameObject);
    }
}
