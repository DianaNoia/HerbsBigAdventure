using System.Collections;
using System.Collections.Generic;
using UnityEngine;

// Activates the yellow enemy tutorial
public class ActivateTut : MonoBehaviour
{
    [SerializeField]
    private GameObject tutorial;

    [SerializeField]
    private BoxCollider collider;

    private void OnTriggerEnter(Collider other)
    {
        if (other.CompareTag("Player"))
        {
            tutorial.SetActive(true);
            
            StartCoroutine(TutorialTimer());
        }
    }

    public IEnumerator TutorialTimer()
    {
        yield return new WaitForSeconds(6f);
        tutorial.SetActive(false);

        collider.enabled = false;
    }
}
