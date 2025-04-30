using FMODUnity;
using UnityEngine;

public class WellAudio : MonoBehaviour
{
    [SerializeField] private StudioEventEmitter ambilEmitter;  // SFX awal
    [SerializeField] private StudioEventEmitter taruhEmitter;  // SFX akhir
    [SerializeField] private StudioEventEmitter masakEmitter;  // SFX masak

    public void PlayAmbilSound()
    {
        if (ambilEmitter != null && !ambilEmitter.IsPlaying())
        {
            ambilEmitter.Play();
            Debug.Log("SFX Mengambil Ramuan dimainkan!");
        }
        else
        {
            Debug.LogWarning("ambilEmitter belum di-assign atau sedang dimainkan!");
        }
    }

    public void PlayTaruhSound()
    {
        if (taruhEmitter != null)
        {
            taruhEmitter.Play();
            Debug.Log("SFX Menaruh Ramuan dimainkan!");
        }
        else
        {
            Debug.LogWarning("taruhEmitter belum di-assign!");
        }
    }
    public void MasakPotion()
    {
        if (masakEmitter != null)
            masakEmitter.Play();
        else
            Debug.LogWarning("masakEmitter belum di-assign!");
    }
    public void StopMasakPotion()
    {
        if (masakEmitter != null && masakEmitter.IsPlaying())
        {
            masakEmitter.Stop();
            Debug.Log("SFX Masak dihentikan!");
        }
        else
        {
            Debug.LogWarning("masakEmitter belum di-assign atau tidak sedang dimainkan!");
        }
    }
    public void StopAmbilSound()
    {
        if (ambilEmitter != null && ambilEmitter.IsPlaying())
        {
            ambilEmitter.Stop();
            Debug.Log("SFX Mengambil Ramuan dihentikan!");
        }
        else
        {
            Debug.LogWarning("ambilEmitter belum di-assign atau tidak sedang dimainkan!");
        }
    }
    public void StopTaruhSound()
    {
        if (taruhEmitter != null && taruhEmitter.IsPlaying())
        {
            taruhEmitter.Stop();
            Debug.Log("SFX Menaruh Ramuan dihentikan!");
        }
        else
        {
            Debug.LogWarning("taruhEmitter belum di-assign atau tidak sedang dimainkan!");
        }
    }
    public void StopAllSounds()
    {
        StopAmbilSound();
        StopTaruhSound();
        StopMasakPotion();
    }
}