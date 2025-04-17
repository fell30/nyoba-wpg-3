using FMODUnity;
using UnityEngine;

public class SFXCounter : MonoBehaviour {
    public static void PlaySFX(EventReference soundEvent, Vector3 position) {
        GameObject sfxObject = new GameObject("OneShotSFX");
        sfxObject.transform.position = position;

        var emitter = sfxObject.AddComponent<StudioEventEmitter>();
        emitter.EventReference = soundEvent;
        emitter.Play();

        Object.Destroy(sfxObject, 2f); // Hancurkan setelah 2 detik
    }
}
