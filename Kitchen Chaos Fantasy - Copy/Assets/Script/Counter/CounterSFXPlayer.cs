using FMODUnity;
using UnityEngine;

public class CounterSFXPlayer : MonoBehaviour {
    [SerializeField] private StudioEventEmitter ambilEmitter;
    [SerializeField] private StudioEventEmitter taruhEmitter;

    public void PlayAmbilSound() {
        if (ambilEmitter != null && !ambilEmitter.IsPlaying()) {
            ambilEmitter.Play();
        }
    }

    public void PlayTaruhSound() {
        if (taruhEmitter != null && !taruhEmitter.IsPlaying()) {
            taruhEmitter.Play();
        }
    }
}
