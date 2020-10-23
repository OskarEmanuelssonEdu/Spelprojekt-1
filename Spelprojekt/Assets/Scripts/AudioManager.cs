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
    private AudioSource mySfxSource2;

    private AudioSource myRunningSoundSource;
    private AudioSource myGrappleHitSoundSource;
    private AudioSource mySlidingSoundSource;
    private AudioReverbFilter myMusicReverbFilter;
    private AudioReverbFilter mySFXReverbFilter;

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
    [Range(0f,1f)]
    private float myMaxRunningVolume = 1f;
    [SerializeField]
    private float myMaxReverbDryLevel = 0f;
    [SerializeField]
    private float myMinReverbDryLevel = -1000f;

    [SerializeField]
    private AudioClip myLevel1MusicClip;
    [SerializeField]
    private AudioClip myLevel2MusicClip;
    [SerializeField]
    private AudioClip myLevel3MusicClip;
    [SerializeField]
    private AudioClip mySlidingSound;
    [SerializeField]
    private AudioClip []myLethalAudioClips;
    [SerializeField]
    [Range(0f,1f)]
    private float myLethalAudioVolume;
    [SerializeField]
    [Range(0, 1.0f)]
    private float myMaxSlidingVolume = 1f;

    [SerializeField]
    AudioMixer myAudioMixer;
    //TODO Fix a audiomixer to be able to control maaster volume in game.
    [SerializeField]
    private AudioClip myClickSound;


    
   
    private void Awake()
    {
        //Make sure we dont destroy this instance
        DontDestroyOnLoad(this.gameObject);
        myMusicSource = this.gameObject.AddComponent<AudioSource>();
        myMusicSource2 = this.gameObject.AddComponent<AudioSource>();
        mySfxSource1 = this.gameObject.AddComponent<AudioSource>();
        mySlidingSoundSource = gameObject.AddComponent<AudioSource>();
        myAudioListener = this.gameObject.GetComponent<AudioListener>();
        myAudioMixer = FindObjectOfType<AudioMixer>();
        //Music will keep playing even if game is paused
        myMusicSource.ignoreListenerPause = true;
        myMusicSource2.ignoreListenerPause = true;
       
       // myMusicReverbFilter = this.gameObject.AddComponent<AudioReverbFilter>();
        
        myMusicSource.loop = true;
        myMusicSource2.loop = true;
      
        mySlidingSoundSource.loop = true;
        mySlidingSoundSource.clip = mySlidingSound;


    }
    private void Start()
    {
        PlayMusic(myLevel1MusicClip);
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
        newSource.Play();
        StartCoroutine(UpdateMusicWithCrossFade(activeSource, newSource, aTransitionTime));
    }
    public void PlayLevel1Music()
    {
        AudioSource activeSource = (myFirstMusicSourceIsPlaying) ? myMusicSource : myMusicSource2;
        AudioSource newSource = (myFirstMusicSourceIsPlaying) ? myMusicSource2 : myMusicSource;

        //Swap the source
        myFirstMusicSourceIsPlaying = !myFirstMusicSourceIsPlaying;

        // Set the fields of the audio source, then start the coroutine to crossfade
        newSource.clip = myLevel1MusicClip;
        newSource.Play();
        StartCoroutine(UpdateMusicWithCrossFade(activeSource, newSource, 1));
    }
    public void PlayLevel2Music()
    {
        AudioSource activeSource = (myFirstMusicSourceIsPlaying) ? myMusicSource : myMusicSource2;
        AudioSource newSource = (myFirstMusicSourceIsPlaying) ? myMusicSource2 : myMusicSource;

        //Swap the source
        myFirstMusicSourceIsPlaying = !myFirstMusicSourceIsPlaying;

        // Set the fields of the audio source, then start the coroutine to crossfade
        newSource.clip = myLevel2MusicClip;
        newSource.Play();
        StartCoroutine(UpdateMusicWithCrossFade(activeSource, newSource, 1));
    }
    public void PlayLevel3Music()
    {
        AudioSource activeSource = (myFirstMusicSourceIsPlaying) ? myMusicSource : myMusicSource2;
        AudioSource newSource = (myFirstMusicSourceIsPlaying) ? myMusicSource2 : myMusicSource;

        //Swap the source
        myFirstMusicSourceIsPlaying = !myFirstMusicSourceIsPlaying;

        // Set the fields of the audio source, then start the coroutine to crossfade
        newSource.clip = myLevel3MusicClip;
        newSource.Play();
        StartCoroutine(UpdateMusicWithCrossFade(activeSource, newSource, 1));
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
    public void SetMasterVolume(float aVolume)
    {
        myMasterVolume = aVolume;
    }
  
    public void PlaySFX1(AudioClip aClip, float aVolume)
    {
        mySfxSource1.PlayOneShot(aClip, aVolume);
    }
    public void PlaySFX2(AudioClip aClip, float aVolume)
    {
        mySfxSource2.PlayOneShot(aClip, aVolume);
    }
    public void PlayClickSound()
    {
        mySfxSource1.PlayOneShot(myClickSound);
    }
    public void PlayLethalHit()
    {
        mySfxSource1.PlayOneShot(myLethalAudioClips[Random.Range(0,myLethalAudioClips.Length)],myLethalAudioVolume);
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
            myMusicSource.volume += aVolume / (myMusicSource.volume+1) * Time.deltaTime;
            myMusicSource2.volume += aVolume / (myMusicSource2.volume+1) * Time.deltaTime;
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
            myMusicSource.volume -= Time.deltaTime*0.1f;
            myMusicSource2.volume -= Time.deltaTime*0.1f;
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
    void PauseAudio()
    {
        AudioListener.pause = true;
    }
    void UnpauseAudio()
    {
        AudioListener.pause = false;
    }

}
