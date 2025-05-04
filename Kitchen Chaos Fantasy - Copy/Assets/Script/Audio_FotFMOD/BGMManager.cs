using FMOD.Studio;
using FMODUnity;
using UnityEngine;

public class BGMManager : MonoBehaviour
{
    private EventInstance bgmInstance;

    private void Start()
    {
        bgmInstance = RuntimeManager.CreateInstance("event:/BGM");
    }

    public void StartBGM()
    {
        bgmInstance.start();
    }
    public void StopBGM()
    {
        bgmInstance.stop(FMOD.Studio.STOP_MODE.ALLOWFADEOUT);
        bgmInstance.release();
    }
}