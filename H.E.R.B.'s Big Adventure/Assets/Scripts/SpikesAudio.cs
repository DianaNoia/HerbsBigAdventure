using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SpikesAudio : MonoBehaviour
{
    private AudioManager am;

    [SerializeField]
    private int spikesAudio;

    private void Start()
    {
        am = FindObjectOfType<AudioManager>();
    }

    private void Update()
    {
        PlayAudio();
    }

    private void PlayAudio()
    {
        am.PlaySFX(spikesAudio);
    }
}
