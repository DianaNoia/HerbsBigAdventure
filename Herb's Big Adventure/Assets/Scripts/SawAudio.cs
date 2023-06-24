using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SawAudio : MonoBehaviour
{
    private AudioManager am;

    [SerializeField]
    private int sawAudio;

    private void Start()
    {
        am = FindObjectOfType<AudioManager>();
    }

    private void OnTriggerEnter(Collider other)
    {
        PlayAudio();
    }

    private void PlayAudio()
    {
        am.PlaySFX(sawAudio);
        
    }

    
}
