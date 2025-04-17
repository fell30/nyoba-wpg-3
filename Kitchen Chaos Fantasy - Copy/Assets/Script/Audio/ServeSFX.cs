using FMODUnity;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ServeSFX : MonoBehaviour {
    [SerializeField] private StudioEventEmitter ambilEmitter;
    public void PlayAmbilSound() {
        Debug.Log("PlayAmbilSound dipanggil");
        if (ambilEmitter != null && !ambilEmitter.IsPlaying()) {
            Debug.Log("Emitter ditemukan dan belum bermain, play!");
            ambilEmitter.Play();
        } else {
            Debug.LogWarning("Emitter null atau sedang bermain.");
        }
    }


}
