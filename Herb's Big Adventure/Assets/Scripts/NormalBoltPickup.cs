using UnityEngine;

public class NormalBoltPickup : MonoBehaviour
{
    private GameManager gm;
    private AudioManager am;

    public int value;

    //public GameObject pickupEffect;

    public int soundToPlay;

    private void Start()
    {
        am = FindObjectOfType<AudioManager>();
        gm = FindObjectOfType<GameManager>();
    }

    private void OnTriggerEnter(Collider other)
    {
        if (other.tag == "Player")
        {
            gm.AddNormalBolts(value);

            Destroy(gameObject);

            //Instantiate(pickupEffect, transform.position, transform.rotation);

            am.PlaySFX(soundToPlay);
        }
    }
}
