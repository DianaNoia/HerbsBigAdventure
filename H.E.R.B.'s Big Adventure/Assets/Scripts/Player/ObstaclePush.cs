using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ObstaclePush : MonoBehaviour
{
    [SerializeField]
    private float forceMagnitude;

    // Reference to PlayerController script
    [SerializeField]
    private PlayerController playerController;

    // When the collider of player hits the collider of box
    private void OnControllerColliderHit(ControllerColliderHit hit)
    {
        Rigidbody rigidbody = hit.collider.attachedRigidbody;

        if (rigidbody != null)
        {
            playerController.anim.SetTrigger("Pushing");
            playerController.moveSpeed = 2.5f;

            Vector3 forceDirection = hit.gameObject.transform.position - transform.position;
            forceDirection.y = 0;
            forceDirection.Normalize();

            rigidbody.AddForceAtPosition(forceDirection * forceMagnitude, transform.position, ForceMode.Impulse);

            Debug.Log("Pushing");
        }
        if(rigidbody == null)
        {
            playerController.moveSpeed = 5f;
        }
    }
}
