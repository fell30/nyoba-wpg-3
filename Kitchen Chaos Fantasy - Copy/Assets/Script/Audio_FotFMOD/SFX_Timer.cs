using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using FMODUnity;


public class SFX_Timer : MonoBehaviour
{
    [SerializeField] private StudioEventEmitter TimerEmitter;



    public void PlayTimer()
    {
        if (TimerEmitter != null && !TimerEmitter.IsPlaying())
        {
            TimerEmitter.Play();
            Debug.Log("Timer sound played.");


        }
        else
        {
            Debug.LogWarning("ambilEmitter belum di-assign atau sedang dimainkan!");
        }
    }

    public void StopTimer()
    {
        if (TimerEmitter != null && TimerEmitter.IsPlaying())
        {
            TimerEmitter.Stop();
        }
        else
        {
            Debug.LogWarning("ambilEmitter belum di-assign atau tidak sedang dimainkan!");
        }
    }



}
