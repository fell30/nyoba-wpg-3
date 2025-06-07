using System.Collections;
using System.Collections.Generic;
using FMODUnity;
using UnityEngine;

public class bgm_Level_Selection : MonoBehaviour
{
    [SerializeField] private StudioEventEmitter bgmEmitter;
    public void PlayBGM()
    {
        if (bgmEmitter != null && !bgmEmitter.IsPlaying())
        {
            bgmEmitter.Play();

        }
        else
        {
            Debug.LogWarning("bgmEmitter belum di-assign atau sedang dimainkan!");
        }
    }
    public void StopBGM()
    {
        if (bgmEmitter != null && bgmEmitter.IsPlaying())
        {
            bgmEmitter.Stop();
        }
        else
        {
            Debug.LogWarning("bgmEmitter belum di-assign atau tidak sedang dimainkan!");
        }
    }

}
