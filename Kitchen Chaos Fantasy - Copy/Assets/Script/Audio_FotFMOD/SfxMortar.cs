using FMODUnity;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SfxMortar : MonoBehaviour
{
    [SerializeField] private StudioEventEmitter menaruhEmitter;  // SFX awal
    [SerializeField] private StudioEventEmitter tumbukEmitter;  // SFX akhir

    public void PlayTaruhSound()
    {
        if (menaruhEmitter != null && !menaruhEmitter.IsPlaying())
        {
            menaruhEmitter.Play();
            // Debug.Log("SFX Mengambil Ramuan dimainkan!");
        }
        else
        {
            Debug.LogWarning("ambilEmitter belum di-assign atau sedang dimainkan!");
        }
    }

    public void PlayMortarSound()
    {
        if (tumbukEmitter != null)
        {
            tumbukEmitter.Play();
            //            Debug.Log("SFX Menaruh Ramuan dimainkan!");
        }
        else
        {
            Debug.LogWarning("taruhEmitter belum di-assign!");
        }
    }
}