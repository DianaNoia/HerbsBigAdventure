using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BoxFloorCheck : MonoBehaviour
{
    [SerializeField]
    private GameObject box, resetPosition;

    //private Vector3 boxPosition;

    //private void Start()
    //{
    //    // box position reset
    //    boxPosition = box.transform.position;   
    //}

    private void OnTriggerEnter(Collider other)
    {
        if (other.tag == "KillZone")
        {
            ResetBox();
        }
    }

    public void ResetBox()
    {
        StartCoroutine(ResetBoxCo());
    }

    public IEnumerator ResetBoxCo()
    {
        yield return new WaitForSeconds(2f);

        box.transform.position = resetPosition.transform.position;
    }
}
