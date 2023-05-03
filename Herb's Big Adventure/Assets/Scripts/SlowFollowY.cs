using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SlowFollowY : MonoBehaviour
{
    public Transform target;
    public float     speedY = 0.1f;

    // Update is called once per frame
    void Update()
    {
        Vector3 error = target.transform.position - transform.position;
        error.y *= speedY;
        transform.position = transform.position + error;
        transform.rotation = target.transform.rotation;
        transform.localScale = target.transform.localScale;
    }
}
