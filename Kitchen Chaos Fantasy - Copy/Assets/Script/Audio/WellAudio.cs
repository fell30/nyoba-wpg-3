using FMODUnity;
using UnityEngine;

public class WellAudio : MonoBehaviour {
    [SerializeField] private StudioEventEmitter ambilEmitter;  // SFX awal
    [SerializeField] private StudioEventEmitter taruhEmitter;  // SFX akhir

    public void PlayAmbilSound() {
        if (ambilEmitter != null && !ambilEmitter.IsPlaying()) {
            ambilEmitter.Play();
            Debug.Log("SFX Mengambil Ramuan dimainkan!");
        } else {
            Debug.LogWarning("ambilEmitter belum di-assign atau sedang dimainkan!");
        }
    }

    public void PlayTaruhSound() {
        if (taruhEmitter != null) {
            taruhEmitter.Play();
            Debug.Log("SFX Menaruh Ramuan dimainkan!");
        } else {
            Debug.LogWarning("taruhEmitter belum di-assign!");
        }
    }
}
