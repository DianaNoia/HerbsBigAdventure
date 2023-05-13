using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SwitchingObjectsWithPressurePlates : MonoBehaviour
{
    public GameObject theObject;

    public PressurePlateController thePressurePlate;

    public bool revealWhenPressed;

    // Update is called once per frame
    void Update()
    {
        if (thePressurePlate.isPressed)
        {
            theObject.SetActive(revealWhenPressed);
        }
        else
        {
            theObject.SetActive(!revealWhenPressed);
        }
    }
}
