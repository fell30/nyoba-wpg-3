using FMODUnity;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SFXMortar : MonoBehaviour {
    [SerializeField] private StudioEventEmitter ambilEmitter;  // SFX awal
    [SerializeField] private StudioEventEmitter potongEmitter;  // SFX akhir

    public void PlayTaruhSound() {
        if (ambilEmitter != null && !ambilEmitter.IsPlaying()) {
            ambilEmitter.Play();
            Debug.Log("SFX Mengambil Ramuan dimainkan!");
        } else {
            Debug.LogWarning("ambilEmitter belum di-assign atau sedang dimainkan!");
        }
    }

    public void PlayMortarSound() {
        if (potongEmitter != null) {
            potongEmitter.Play();
            Debug.Log("SFX Menaruh Ramuan dimainkan!");
        } else {
            Debug.LogWarning("taruhEmitter belum di-assign!");
        }
    }
}

