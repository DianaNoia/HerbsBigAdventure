using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ObstaclePush : MonoBehaviour
{
    [SerializeField]
    private float forceMagnitude;

    // Reference to PlayerController script
    private PlayerController playerController;

    // When the collider of player hits the collider of box
    private void OnControllerColliderHit(ControllerColliderHit hit)
    {
        Rigidbody rigidbody = hit.collider.attachedRigidbody;

        if (rigidbody != null)
        {
            Vector3 forceDirection = hit.gameObject.transform.position - transform.position;
            forceDirection.y = 0;
            forceDirection.Normalize();

            rigidbody.AddForceAtPosition(forceDirection * forceMagnitude, transform.position, ForceMode.Impulse);

            playerController.moveSpeed = 2.5f;
            playerController.anim.SetTrigger("Pushing");
            Debug.Log("Pushing");
        }
        //else
        //{
        //    playerController.moveSpeed = 5f;
        //    Debug.Log("Stopped Pushing");
        //}
    }
}
