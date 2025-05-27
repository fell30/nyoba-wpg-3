using System.Collections;
using System.Collections.Generic;
using System.Reflection;
using UnityEngine;

public class MortarCounterVisual : MonoBehaviour
{

    [SerializeField] private MortarCounter mortarCounter;
    [SerializeField] private ParticleSystem cutParticleSystem;
    private Animator animator;
    private const string cut = "cut";

    private void Awake()
    {
        animator = GetComponent<Animator>();
    }

    private void Start()
    {
        mortarCounter.OnCut += MortarCounter_OnCut;

    }

    private void MortarCounter_OnCut(object sender, System.EventArgs e)
    {
        animator.SetTrigger(cut);
        if (cutParticleSystem != null)
        {
            cutParticleSystem.Play();
        }
    }

}
