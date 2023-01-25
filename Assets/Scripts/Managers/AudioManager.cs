using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Random = UnityEngine.Random;

public class AudioManager : MonoBehaviour
{
    public static AudioManager Instance;

    [SerializeField] private AudioSource mainMenuMusic, levelMusic, bossMusic;

    [SerializeField] private AudioSource[] sfx;

    private void Awake()
    {
        MakeInstance();
    }

    private void MakeInstance()
    {
        if (Instance==null)
        {
            Instance = this;
            DontDestroyOnLoad(gameObject);
        }
        else
        {
            Destroy(gameObject);
        }
    }

    public void PlayMenuMusic()
    {
        levelMusic.Stop();
        bossMusic.Stop();
        
        mainMenuMusic.Play();
    }

    public void PlayLevelMusic()
    {
        if (!levelMusic.isPlaying)
        {
            bossMusic.Stop();
            mainMenuMusic.Stop();
        
            levelMusic.Play();
        }
    }

    public void PlayBossMusic()
    {
        levelMusic.Stop();
        
        bossMusic.Play();
    }

    public void PlaySFX(int indexToPlay)
    {
        sfx[indexToPlay].Stop();
        sfx[indexToPlay].Play();
    }

    public void PlaySfxAdjusted(int indexToAdjust)
    {
        sfx[indexToAdjust].pitch = Random.Range(0.8f, 1.2f);
        PlaySFX(indexToAdjust);
    }
}
