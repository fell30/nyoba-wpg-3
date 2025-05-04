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

        }
        else
        {
            Debug.LogWarning("ambilEmitter belum di-assign atau sedang dimainkan!");
        }
    }



}
