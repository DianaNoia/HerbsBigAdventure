using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Audio;

public class AudioManager : MonoBehaviour
{
    private AudioManager am;
    private UIManager uim;

    public AudioSource[] music;
    public AudioSource[] sfx;

    public int levelMusicToPlay;

    public AudioMixerGroup musicMixer, sfxMixer; 

    //private int currentTrack;

    private void Awake()
    {
        am = this;
    }

    // Start is called before the first frame update
    void Start()
    {
        PlayMusic(levelMusicToPlay);
    }

    // Update is called once per frame
    void Update()
    {
        /*if (Input.GetKeyDown(KeyCode.M))
        {
            currentTrack++;
            PlayMusic(currentTrack);
        } */
    }

    public void PlayMusic(int musicToPlay)
    {
        for (int i = 0; i < music.Length; i++)
        {
            music[i].Stop();
        }

        music[musicToPlay].Play();

        // Play the music clip at the specified index
        if (musicToPlay >= 0 && musicToPlay < music.Length)
        {
            music[musicToPlay].Play();
        }
        else
        {
            Debug.LogWarning("Invalid music index: " + musicToPlay);
        }
    }

    public void PlaySFX(int sfxToPlay)
    {
        sfx[sfxToPlay].Play();
    }

    public void SetMusicLevel()
    {
        musicMixer.audioMixer.SetFloat("MusicVol", uim.musicVolSlider.value);
    }

    public void SetSFXLevel()
    {
        sfxMixer.audioMixer.SetFloat("SFXVol", uim.sfxVolSlider.value);
    }
}
