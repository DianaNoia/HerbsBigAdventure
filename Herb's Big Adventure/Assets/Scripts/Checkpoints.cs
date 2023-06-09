using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Checkpoints : MonoBehaviour
{
    private GameManager gm;
    private AudioManager am;

    public GameObject cpOn, cpOff;

    public int soundToPlay;

    public bool hasPlayed = false;

    private void OnTriggerEnter(Collider other)
    {
        if (other.tag =="Player")
        {
            gm.SetSpawnPoint(transform.position);

            Checkpoints[] allCP = FindObjectsOfType<Checkpoints>();

            for (int i = 0; i < allCP.Length; i++)
            {
                allCP[i].cpOff.SetActive(true);
                allCP[i].cpOn.SetActive(false);
            }

            cpOff.SetActive(false);
            cpOn.SetActive(true);

            if (!hasPlayed)
            {
                am.PlaySFX(soundToPlay);
                hasPlayed = true;
            }
        }
    }
}
