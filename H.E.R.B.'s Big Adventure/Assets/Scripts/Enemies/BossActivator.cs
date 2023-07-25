using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BossActivator : MonoBehaviour
{
    [SerializeField]
    private AudioManager am;
    private BossActivator ba;

    public GameObject entrance, theBoss;

    private void Awake()
    {
        ba = this;
    }

    private void OnTriggerEnter(Collider other)
    {
        if (other.tag == "Player")
        {
            // Sets the entrance to the boss to inactive and the boss to active
            entrance.SetActive(false);
            theBoss.SetActive(true);

            // Sets the boss activator collider to inactive 
            gameObject.SetActive(false);
        }
    }
}
