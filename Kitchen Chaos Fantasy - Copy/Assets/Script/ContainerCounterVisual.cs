using System.Collections;
using System.Collections.Generic;
using System.Reflection;
using UnityEngine;

public class ContainerCounterVisual : MonoBehaviour
{

    [SerializeField] private ContainerCounter containerCounter;
    private Animator animator;
    private const string OPEN_CLOSE = "OpenClose";

    private void Awake()
    {
        animator = GetComponent<Animator>();
    }

    private void Start()
    {
        containerCounter.OnGrabbedKitchenObject += container;
    }

    private void container(object sender, System.EventArgs e)
    {
        animator.SetTrigger(OPEN_CLOSE);
    }

}
