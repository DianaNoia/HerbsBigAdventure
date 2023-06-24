using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CameraSwitcher : MonoBehaviour
{
    [SerializeField]
    private GameObject camPlayer, camObjective;

    [SerializeField]
    private SphereCollider collider;

    private void OnTriggerEnter(Collider other)
    {
        if (other.CompareTag("Player"))
        {
            camPlayer.SetActive(false);
            camObjective.SetActive(true);
            
            StartCoroutine(SwapCameraTimer());
        }
    }

    public IEnumerator SwapCameraTimer()
    {
        yield return new WaitForSeconds(3f);

        camPlayer.SetActive(true);
        camObjective.SetActive(false);

        collider.enabled = false;
    }
}
