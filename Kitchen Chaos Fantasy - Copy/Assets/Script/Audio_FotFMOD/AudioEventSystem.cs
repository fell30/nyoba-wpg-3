using System;
using UnityEngine;

public static class AudioEventSystem
{
    // Event untuk memutar audio
    public static event Action<string> OnPlayAudio;
    public static event Action<string> OnStopAudio;

    // Method untuk memicu event play audio
    public static void PlayAudio(string audioName)
    {
        if (OnPlayAudio != null)
        {
            OnPlayAudio(audioName);
        }
    }

    // Method untuk memicu event stop audio
    public static void StopAudio(string audioName)
    {
        if (OnStopAudio != null)
        {
            OnStopAudio(audioName);
        }
    }
}
