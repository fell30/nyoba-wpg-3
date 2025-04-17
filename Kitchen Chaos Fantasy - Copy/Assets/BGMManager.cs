using FMOD.Studio;
using FMODUnity;
using UnityEngine;

public class BGMManager : MonoBehaviour {
    private EventInstance bgmInstance;

    private void Start() {
        bgmInstance = RuntimeManager.CreateInstance("event:/BGM");
    }

    public void StartBGM() {
        bgmInstance.start();  // Mulai BGM setelah tutorial selesai
    }
}
