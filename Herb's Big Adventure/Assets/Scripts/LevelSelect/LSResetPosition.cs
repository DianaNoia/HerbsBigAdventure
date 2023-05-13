using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class LSResetPosition : MonoBehaviour
{
    public static LSResetPosition instance;

    public Vector3 respawnPosition;

    private void Awake()
    {
        instance = this;
    }

    private void OnTriggerEnter(Collider other)
    {
        if (other.tag == "Player")
        {
            //Deactivating player controller before moving the player because character controller is interfering with moving the player
            PlayerController.instance.gameObject.SetActive(false);

            PlayerController.instance.transform.position = respawnPosition;
            PlayerController.instance.gameObject.SetActive(true);
        }
    }
}
