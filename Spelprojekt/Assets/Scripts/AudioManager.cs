using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class AudioManager : MonoBehaviour
{
    #region
    private static AudioManager myInstance;
    public static AudioManager Instance
    {
        get
        {
            if (myInstance == null)
            {
                myInstance = FindObjectOfType<AudioManager>();
                if (myInstance == null)
                {
                    myInstance = new GameObject("Spawned AudioManager", typeof(AudioManager)).GetComponent<AudioManager>();
                }
            }
            return myInstance;
        }
        private set
        {
            myInstance = value;
        }
    }
    #endregion

    #region Fields
    private AudioSource myMusicSource;
    private AudioSource myMusicSource2;
    private AudioSource mySfxSource;

    private bool myFirstMusicSourceIsPlaying;

    #endregion
    private void Awake()
    {
        //Make sure we dont destroy this instance
        DontDestroyOnLoad(this.gameObject);
        myMusicSource = this.gameObject.AddComponent<AudioSource>();
        myMusicSource2 = this.gameObject.AddComponent<AudioSource>();
        mySfxSource = this.gameObject.AddComponent<AudioSource>();

        //Loop the music tracks
        myMusicSource.loop = true;
        myMusicSource2.loop = true;
    }
    public void PlayMusic(AudioClip aMusicClip)
    {
        //Determine which source is active
        AudioSource activeSource = (myFirstMusicSourceIsPlaying) ? myMusicSource : myMusicSource2;

        activeSource.clip = aMusicClip;
        activeSource.volume = 1;
        activeSource.Play();
    }
    public void PlayMusicWithFade(AudioClip aNewClip, float aTransitionTime = 1.0f)
    {
        AudioSource activeSource = (myFirstMusicSourceIsPlaying) ? myMusicSource : myMusicSource2;
        StartCoroutine(UpdateMusicWithFade(activeSource, aNewClip, aTransitionTime));
    }
    //Crossfades between two music clips
    public void PlayMusicWithCrossFade(AudioClip aMusicClip, float aTransitionTime = 1.0f)
    {
        //Determine which source is active
        AudioSource activeSource = (myFirstMusicSourceIsPlaying) ? myMusicSource : myMusicSource2;
        AudioSource newSource = (myFirstMusicSourceIsPlaying) ? myMusicSource2 : myMusicSource;

        //Swap the source
        myFirstMusicSourceIsPlaying = !myFirstMusicSourceIsPlaying;

        // Set the fields of the audio source, then start the coroutine to crossfade
        newSource.clip = aMusicClip;
        newSource.Play();
        StartCoroutine(UpdateMusicWithCrossFade(activeSource, newSource, aTransitionTime));
    }
    private IEnumerator UpdateMusicWithFade(AudioSource anActiveSouce, AudioClip aNewClip, float aTransitionTime)
    {
        // Make sure the source is active and playing
        if (!anActiveSouce.isPlaying)
        {
            anActiveSouce.Play();
        }
        float t = 0.0f;
        //Fade out
        for (t = 0; t < aTransitionTime; t+=Time.deltaTime)
        {
            anActiveSouce.volume = (1 - (t / aTransitionTime));
            yield return null;
        }
        anActiveSouce.Stop();
        anActiveSouce.clip = aNewClip;
        anActiveSouce.Play();

        //Fade in
        for (t = 0; t < aTransitionTime; t += Time.deltaTime)
        {
            anActiveSouce.volume = (t / aTransitionTime);
            yield return null;
        }
    }
    private IEnumerator FadeOutMusic(AudioSource anActiveSouce, float aTransitionTime, float aVolume)
    {
        //Make sure the source is active and playing
        if (!anActiveSouce.isPlaying)
        {
            anActiveSouce.Play();
        }
        float t = 0.0f;
        //Fade out
        if (anActiveSouce.volume != aVolume)
        {
            for (t = 0; t < aTransitionTime; t += Time.deltaTime)
            {
                anActiveSouce.volume = aVolume * (1 - (t / aTransitionTime));
                yield return null;
            }
        }
            
        //anActiveSouce.Stop();
        anActiveSouce.volume = aVolume;
    }
    private IEnumerator FadeInMusic(AudioSource anActiveSouce, float aTransitionTime, float aVolume)
    {
        //Make sure the source is active and playing
        if (!anActiveSouce.isPlaying)
        {
            anActiveSouce.Play();
        }
        float t = 0.0f;
        //Fade in if the current volume is not the same as the volume we want to fade to
        if (anActiveSouce.volume != aVolume)
        {
            for (t = 0; t < aTransitionTime; t += Time.deltaTime)
            {
                anActiveSouce.volume = aVolume * (t / aTransitionTime);
                yield return null;
            }
        }
        
        //anActiveSouce.Stop();
        anActiveSouce.volume = aVolume;
    }

    private IEnumerator UpdateMusicWithCrossFade(AudioSource anOriginal, AudioSource newSource, float aTransitionTime)
    {
        float t = 0.0f;

        for (t = 0.0f; t < aTransitionTime; t += Time.deltaTime)
        {
            anOriginal.volume = (1 - (t / aTransitionTime));
            newSource.volume = (t / aTransitionTime);
            yield return null;
        }
        anOriginal.Stop();
    }
    public void PlaySFX(AudioClip aClip)
    {
        mySfxSource.PlayOneShot(aClip);
    }
    public void PlaySFX(AudioClip aClip, float aVolume)
    {
        mySfxSource.PlayOneShot(aClip, aVolume);
    }

    public void SetMusicVolume(float aVolume)
    {
        myMusicSource.volume = aVolume;
        myMusicSource2.volume = aVolume;
    }
    public void FadeOutMusicVolume(float aTransitionTime, float aVolume)
    {
        AudioSource activeSource = (myFirstMusicSourceIsPlaying) ? myMusicSource : myMusicSource2;
        StartCoroutine(FadeOutMusic(activeSource, aTransitionTime, aVolume));
    }
    public void FadeInMusicVolume(float aTransitionTime, float aVolume)
    {
        AudioSource activeSource = (myFirstMusicSourceIsPlaying) ? myMusicSource : myMusicSource2;
        StartCoroutine(FadeInMusic(activeSource, aTransitionTime, aVolume));
    }
    public void SetSFXVolume(float aVolume)
    {
        mySfxSource.volume = aVolume;
    }
    
}
