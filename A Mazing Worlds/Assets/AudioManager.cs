using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class AudioManager : MonoBehaviour
{
    public AudioClip backgroundMusic;

    private void Start()
    {
        // if there is already a background music playing, don't play it again
        if (GameObject.FindGameObjectsWithTag("BackgroundMusic").Length == 0)
        {
            GameObject backgroundMusic = new GameObject("BackgroundMusic");
            backgroundMusic.tag = "BackgroundMusic";
            backgroundMusic.AddComponent<AudioSource>();
            backgroundMusic.GetComponent<AudioSource>().clip = this.backgroundMusic;
            backgroundMusic.GetComponent<AudioSource>().loop = true;
            backgroundMusic.GetComponent<AudioSource>().Play();
            DontDestroyOnLoad(backgroundMusic);
        }
    }
}
