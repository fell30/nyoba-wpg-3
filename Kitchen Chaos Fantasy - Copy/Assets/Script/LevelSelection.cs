using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class LevelSelection : MonoBehaviour
{
    public GameObject Level;
    public string sceneToLoad;
    public Animator anim;

    public void Injak()
    {
        anim.SetTrigger("Injak");
    }
    public void GadiInjak()
    {
        anim.SetTrigger("GadiInajak");
    }


}
