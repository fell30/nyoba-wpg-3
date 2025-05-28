using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerAnimator : MonoBehaviour
{

    private Animator animator;
    [SerializeField] private Player player;
    [SerializeField] private CauldronCounter cauldronCounter;
    const string IS_WALKING = "IsWalking";
    const string IS_COOKING = "IsCooking";

    private void Awake()
    {
        animator = GetComponent<Animator>();
    }

    private void Update()
    {
        animator.SetBool(IS_WALKING, player.IsWalking());
        animator.SetBool(IS_COOKING, player.IsCooking());
    }
}
