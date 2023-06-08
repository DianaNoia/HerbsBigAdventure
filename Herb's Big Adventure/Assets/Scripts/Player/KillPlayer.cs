using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class KillPlayer : MonoBehaviour
{
    [SerializeField]
    private GameManager gm;

    private void OnTriggerEnter(Collider other)
    {
        if (other.tag == "Player")
        {
            gm.Respawn();
        }
    }
}
