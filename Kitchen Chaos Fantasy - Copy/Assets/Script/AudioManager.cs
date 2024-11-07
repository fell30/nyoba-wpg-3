using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class AudioManager : MonoBehaviour
{
    public static AudioManager instance;

    [SerializeField] private AudioSource audioSource;
    [SerializeField] private AudioSource footstepAudioSource; // AudioSource khusus untuk footstep

    [System.Serializable]
    public class AudioClipItem
    {
        public string audioName;
        public AudioClip clip;
    }

    [SerializeField] private AudioClipItem[] audioClipItems;

    private Dictionary<string, AudioClip> audioClips = new Dictionary<string, AudioClip>();

    private void Awake()
    {
        if (instance == null)
        {
            instance = this;
            DontDestroyOnLoad(gameObject);
        }
        else
        {
            Destroy(gameObject);
        }
    }

    private void Start()
    {
        // Menambahkan audio clips dari Inspector ke dalam dictionary
        foreach (var item in audioClipItems)
        {
            if (!audioClips.ContainsKey(item.audioName))
            {
                audioClips.Add(item.audioName, item.clip);
            }
        }
    }

    private void OnEnable()
    {
        AudioEventSystem.OnPlayAudio += PlayAudio;
        AudioEventSystem.OnStopAudio += StopAudio;
    }

    private void OnDisable()
    {
        AudioEventSystem.OnPlayAudio -= PlayAudio;
        AudioEventSystem.OnStopAudio -= StopAudio;
    }

    private void PlayAudio(string audioName)
    {
        if (audioClips.ContainsKey(audioName))
        {
            if (audioName == "Footstep")
            {
                // Menggunakan footstepAudioSource
                footstepAudioSource.clip = audioClips[audioName];
                footstepAudioSource.loop = true;
                if (!footstepAudioSource.isPlaying)
                {
                    footstepAudioSource.Play();
                }
            }
            else
            {
                audioSource.clip = audioClips[audioName];
                audioSource.loop = false;
                audioSource.Play();
            }
            // Debug.Log("Playing audio: " + audioName);
        }
        else
        {
            Debug.LogWarning("Audio clip not found: " + audioName);
        }
    }

    private void StopAudio(string audioName)
    {
        if (audioClips.ContainsKey(audioName))
        {
            if (audioName == "Footstep")
            {
                footstepAudioSource.Stop();
                footstepAudioSource.loop = false;
            }
            else
            {
                if (audioSource.isPlaying && audioSource.clip == audioClips[audioName])
                {
                    audioSource.Stop();
                }
            }
            // Debug.Log("Stopping audio: " + audioName);
        }
    }
}
