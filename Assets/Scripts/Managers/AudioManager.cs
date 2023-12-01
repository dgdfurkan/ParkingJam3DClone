using System;
using UnityEngine;

namespace GunduzDev
{
    [System.Serializable]
    public class Audio
    {
        //public string name;
        public AudioTypes audioType;
        public AudioClip audio;
    }

    //Add all sounds
    public enum AudioTypes
    {
        MusicCityscapes, Barrier, HitObstacle, HitVehicle, HitVehicle2, Horn ,LevelWin, OnPath
    }

    public class AudioManager : MonoBehaviour
	{
        // Audio sources
        [SerializeField] private AudioSource musicSource;
        [SerializeField] private AudioSource soundEffectsSource;
        
        [Header("Sounds")]
        [Space(20)]

        public Audio[] MusicAudios;
        public Audio[] SFXAudios;

        #region Singleton
    
        public static AudioManager Instance;
    
        private void Awake()
        {
            if (Instance != null && Instance != this)
            {
                Destroy(gameObject);
                return;
            }
            Instance = this;
        }
    
        #endregion
        
        #region Subscribe and Unsubscribe Events
        private void OnEnable() => SubscribeEvents();
        
        private void SubscribeEvents()
        {
            Signals.onMusicPlay += PlayMusic;
            Signals.onMusicStop += StopMusic;
            Signals.onSFXPlay += PlaySoundEffect;
        }

        private void UnsubscribeEvents()
        {
            Signals.onMusicPlay -= PlayMusic;
            Signals.onMusicStop -= StopMusic;
            Signals.onSFXPlay -= PlaySoundEffect;
        }

        private void OnDisable() => UnsubscribeEvents();
        
        #endregion
        
        // Initialization
        private void Start()
        {
            Initialize();
        }

        private void Initialize()
        {
            // Create audio sources
            if (musicSource != null && soundEffectsSource != null) return;
            
            musicSource = gameObject.transform.GetChild(0).GetComponent<AudioSource>();
            
            soundEffectsSource = gameObject.transform.GetChild(1).GetComponent<AudioSource>();
        }

        // Play music 
        public void PlayMusic(AudioTypes type)
        {
            Audio a = Array.Find(MusicAudios, x=>x.audioType == type);

            if (a != null)
            {
                musicSource.clip = a.audio;
                musicSource.loop = true;
                musicSource.Play();
            }
            else
            {
                Debug.LogWarning("Audio not found");
            }
        }

        // Stop music
        public void StopMusic()
        {
            musicSource.Stop();
        }

        public void PlaySoundEffect(AudioTypes type)
        {
            Audio a = Array.Find(SFXAudios, x => x.audioType == type);

            if (a != null)
            {
                soundEffectsSource.PlayOneShot(a.audio);
            }
            else
            {
                Debug.LogWarning("Audio not found");
            }            
        }
    }   
}
