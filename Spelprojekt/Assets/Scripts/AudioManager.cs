using System.Collections;
using System.Collections.Generic;

using UnityEngine;
using UnityEngine.Audio;

public class AudioManager : MonoBehaviour
{
    #region
    private static AudioManager ourPrivateInstance;
    public static AudioManager ourPublicInstance
    {
        get
        {
            if (ourPrivateInstance == null)
            {
                ourPrivateInstance = FindObjectOfType<AudioManager>();
                if (ourPrivateInstance == null)
                {
                    ourPrivateInstance = new GameObject("Spawned AudioManager", typeof(AudioManager)).GetComponent<AudioManager>();
                }
            }
            return ourPrivateInstance;
        }
        private set
        {
            ourPrivateInstance = value;
        }
    }
    #endregion

    #region Fields
    private AudioSource myMusicSource;
    private AudioSource myMusicSource2;
    private AudioSource mySfxSource1;
    private AudioSource mySlidingSoundSource;
    private AudioReverbFilter myMusicReverbFilter;

    private bool myFirstMusicSourceIsPlaying;

    #endregion

    [SerializeField]
    private AudioListener myAudioListener;
    [SerializeField]
    [Range(0f,1f)]
    private float myMasterVolume = 1;

    [SerializeField]
    [Range(0f, 1f)]
    private float myMinMusicVolume = 0.3f;
    [SerializeField]
    [Range(0f, 1f)]
    private float myMaxMusicVolume = 1f;
    [SerializeField]
    private float myMaxReverbDryLevel = 0f;
    [SerializeField]
    private float myMinReverbDryLevel = -1000f;

    [SerializeField]
    private AudioClip[] myMusicClips;
    private int myCurrentMusicIndex = 0;
    
    [SerializeField]
    private AudioClip mySlidingSound;
    [SerializeField]
    private AudioClip []myLethalAudioClips;
    [SerializeField]
    [Range(0f,1f)]
    private float myLethalAudioVolume;
    [SerializeField]
    private AudioClip myFallingObjectClip;
    [SerializeField]
    [Range(0f, 1f)]
    private float myFallingObjectVolume;
    [SerializeField]
    [Range(0, 1.0f)]
    private float myMaxSlidingVolume = 1f;

    [SerializeField]
    private AudioClip myClickSound;
    [SerializeField]
    [Range(0f,1f)]
    private float myClickVolume;
    [SerializeField]
    private AudioClip myCountdownSound;
    [SerializeField]
    [Range(0f, 1f)]
    private float myCountdownVolume;





    private void Awake()
    {
        //Make sure we dont destroy this instance
        DontDestroyOnLoad(this.gameObject);
        myMusicSource = this.gameObject.AddComponent<AudioSource>();
        myMusicSource2 = this.gameObject.AddComponent<AudioSource>();
        mySfxSource1 = this.gameObject.AddComponent<AudioSource>();
        mySlidingSoundSource = gameObject.AddComponent<AudioSource>();
        myAudioListener = this.gameObject.GetComponent<AudioListener>();
        //Music will keep playing even if game is paused
        myMusicSource.ignoreListenerPause = true;
        myMusicSource2.ignoreListenerPause = true;
       
       // myMusicReverbFilter = this.gameObject.AddComponent<AudioReverbFilter>();
        
        myMusicSource.loop = true;
        myMusicSource2.loop = true;

        mySfxSource1.playOnAwake = false;
        mySlidingSoundSource.playOnAwake = false;
        mySlidingSoundSource.loop = true;
        mySlidingSoundSource.clip = mySlidingSound;


    }
    private void Start()
    {
        PlayMusic(myMusicClips[1]);
    }

    private void Update()
    {
        AudioListener.volume = myMasterVolume;
    }
    public void PlayMusic(AudioClip aMusicClip)
    {
        //Determine which source is active
        AudioSource activeSource = (myFirstMusicSourceIsPlaying) ? myMusicSource : myMusicSource2;

        activeSource.clip = aMusicClip;
        activeSource.volume = myMinMusicVolume;
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
        newSource.volume = 0f;
        newSource.Play();
        
        StartCoroutine(UpdateMusicWithCrossFade(activeSource, newSource, aTransitionTime));
    }
    public void IncreaseMusicIndex()
    {
        myCurrentMusicIndex++;
        PlayMusicWithCrossFade(myMusicClips[myCurrentMusicIndex],5f);
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
            anActiveSouce.volume = (anActiveSouce.volume - (t / aTransitionTime));
            yield return null;
        }
        anActiveSouce.Stop();
        anActiveSouce.clip = aNewClip;
        anActiveSouce.Play();

        //Fade in
        for (t = 0; t < aTransitionTime; t += Time.deltaTime)
        {
            anActiveSouce.volume = (t / aTransitionTime);
            if (anActiveSouce.volume >= myMinMusicVolume)
            {
                anActiveSouce.volume = myMinMusicVolume;
            }
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

    private IEnumerator UpdateMusicWithCrossFade(AudioSource anOriginal, AudioSource aNewSource, float aTransitionTime)
    {
        
        aNewSource.volume = 0f;
        for (float t = 0.0f; t < aTransitionTime; t += Time.deltaTime)
        {
            anOriginal.volume -= Time.deltaTime;
            aNewSource.volume += 0.000000001f;
            if (aNewSource.volume > myMinMusicVolume)
            {
                aNewSource.volume = myMinMusicVolume;
            }
            yield return null;
        }
        anOriginal.Stop();
    }
    public void SetMasterVolume(float aVolume)
    {
        myMasterVolume = aVolume;
    }
  
    public void PlaySFX1(AudioClip aClip, float aVolume)
    {
        mySfxSource1.PlayOneShot(aClip, aVolume);
    }

    public void PlayClickSound()
    {
        mySfxSource1.PlayOneShot(myClickSound,myClickVolume);
    }
    public void PlayLethalHit()
    {
        mySfxSource1.PlayOneShot(myLethalAudioClips[Random.Range(0,myLethalAudioClips.Length)],myLethalAudioVolume);
    }
    public void PlayFallingObject()
    {
        //Snabb lösning för att få ljudet från spikar att vara tyst på Bo's bana
        if (myCurrentMusicIndex != 1)
        {
            mySfxSource1.PlayOneShot(myFallingObjectClip, myFallingObjectVolume);
        }
    }
    public void PlayCountDown()
    {
        mySfxSource1.PlayOneShot(myCountdownSound,myCountdownVolume);
    }
    public void PlaySlidingSound()
    {
        mySlidingSoundSource.loop = true;
        if (!mySlidingSoundSource.isPlaying)
        {
            mySlidingSoundSource.Play();
        }

        mySlidingSoundSource.volume += Time.deltaTime;


        if (mySlidingSoundSource.volume > myMaxSlidingVolume)
        {
            mySlidingSoundSource.volume = myMaxSlidingVolume;
        }
    }
    public void StopSlidingSound()
    {
        
        mySlidingSoundSource.volume -= Time.deltaTime;
        if (mySlidingSoundSource.volume <= 0)
        {
            mySlidingSoundSource.loop = false;
        }
        
    }
    public void SetMusicVolume(float aVolume)
    {
        if (myMusicSource.volume < myMaxMusicVolume || myMusicSource2.volume < myMaxMusicVolume && aVolume > 0)
        {
            myMusicSource.volume += aVolume / (myMusicSource.volume+1) * Time.deltaTime*0.7f;
            myMusicSource2.volume += aVolume / (myMusicSource2.volume+1) * Time.deltaTime * 0.7f;
        }
        
        if (myMusicSource.volume < myMinMusicVolume || myMusicSource2.volume < myMinMusicVolume)
        {
            myMusicSource.volume = myMinMusicVolume;
            myMusicSource2.volume = myMinMusicVolume;
        }
        else if (myMusicSource.volume > myMaxMusicVolume || myMusicSource2.volume > myMaxMusicVolume)
        {
            myMusicSource.volume = myMaxMusicVolume;
            myMusicSource2.volume = myMaxMusicVolume;
        }
        if (myMusicSource.volume > myMinMusicVolume || myMusicSource2.volume > myMinMusicVolume)
        {
            myMusicSource.volume -= Time.deltaTime*0.06f;
            myMusicSource2.volume -= Time.deltaTime*0.06f;
        }
    }
    public void SetMusicPitch(float aPitch)
    {
        myMusicSource.pitch = aPitch;
        myMusicSource2.pitch = aPitch;
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
    //public void SetSFXVolume(float aVolume)
    //{
    //    //mySfxSource.volume = aVolume;
    //    mySfxSource1.volume += aVolume;
    //    if (mySfxSource1.volume < myMinMusicVolume)
    //    {
    //        mySfxSource1.volume = myMinMusicVolume;
    //    }
    //}
    public void SetMusicReverb(float aNumber)
    {
        myMusicReverbFilter.dryLevel += aNumber;
        if (myMusicReverbFilter.dryLevel < myMinReverbDryLevel)
        {
            myMusicReverbFilter.dryLevel = myMinReverbDryLevel;
        }
        else if (myMusicReverbFilter.dryLevel > myMaxReverbDryLevel)
        {
            myMusicReverbFilter.dryLevel = myMaxReverbDryLevel;
        }
    }
    public void PlayLevel1Music()
    {
        myCurrentMusicIndex = 0;
        PlayMusicWithFade(myMusicClips[myCurrentMusicIndex], 2f);
    }
    public void PlayLevel2Music()
    {
        myCurrentMusicIndex = 1;
        PlayMusicWithFade(myMusicClips[myCurrentMusicIndex], 2f);
    }
    public void PlayLevel3Music()
    {
        myCurrentMusicIndex = 2;
        PlayMusicWithFade(myMusicClips[myCurrentMusicIndex], 2f);
    }
    
    void PauseAudio()
    {
        AudioListener.pause = true;
    }
    void UnpauseAudio()
    {
        AudioListener.pause = false;
    }

}
