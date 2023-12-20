using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class AudioManager : MonoBehaviour
{
    public static AudioManager instance;

    public AudioClip backgroundMusic;
    private GameObject bgMusic;
    private AudioSource bgMusicSource;

    public float musicVolume = 1f;
    public float sfxVolume = 1f;

    private void Awake()
    {
        // if there is already an instance of this class, destroy it
        if (instance != null)
        {
            Destroy(this.gameObject);
        }
        // otherwise, set the instance to this
        else
        {
            instance = this;
        }
    }
    private void Start()
    {
        // if there is already a background music playing, don't play it again
        if (GameObject.FindGameObjectsWithTag("BackgroundMusic").Length == 0)
        {
            bgMusic = new GameObject("BackgroundMusic");
            bgMusic.tag = "BackgroundMusic";
            bgMusicSource = bgMusic.AddComponent<AudioSource>();
            bgMusicSource.clip = backgroundMusic;
            bgMusicSource.loop = true;
            bgMusicSource.volume = musicVolume;
            bgMusicSource.Play();
        }
        else
        {
            bgMusic = GameObject.FindGameObjectsWithTag("BackgroundMusic")[0];
            bgMusicSource = bgMusic.GetComponent<AudioSource>();
            
            foreach (var bcMus in GameObject.FindGameObjectsWithTag("BackgroundMusic"))
            {
                if (bcMus != bgMusic)
                    Destroy(bcMus);
            }
        }
    }

    private void Update()
    {
        if (bgMusicSource.volume != musicVolume)
            bgMusicSource.volume = musicVolume;
    }

    public void PlaySFX(AudioClip sfx)
    {
        GameObject sfxObject = new GameObject("SFX");
        AudioSource sfxSource = sfxObject.AddComponent<AudioSource>();

        sfxSource.clip = sfx;
        sfxSource.volume = sfxVolume;
        sfxSource.Play();

        Destroy(sfxObject, sfx.length);
    }
}
