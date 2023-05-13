using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PressurePlateController : MonoBehaviour
{
    public bool isPressed;

    public Transform pressurePlate, pressurePlateDown;

    private Vector3 pressurePlateUp;

    public bool isOnOff;

    // Start is called before the first frame update
    void Start()
    {
        pressurePlateUp = pressurePlate.position;
    }

    private void OnTriggerEnter(Collider other)
    {
        if (other.tag == "PushBox")
        {
            if (isOnOff)
            {
                if (isPressed)
                {
                    pressurePlate.position = pressurePlateUp;
                    isPressed = false;
                }
                else
                {
                    pressurePlate.position = pressurePlateDown.position;
                    isPressed = true;
                }
            }
            else
            {
                if (!isPressed)
                {
                    pressurePlate.position = pressurePlateDown.position;
                    isPressed = true;
                }
            }
        }
    }
}
