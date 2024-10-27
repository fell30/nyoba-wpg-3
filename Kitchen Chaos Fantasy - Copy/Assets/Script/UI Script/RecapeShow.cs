using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class RecapeShow : MonoBehaviour
{
    [SerializeField] private Animator animator;
    private string HIDE_RECAPE = "Recape_Hide";
    private bool isRecapeVisible = false;

    private void Start()
    {
        HideRecape();
    }

    private void Update()
    {
        if (Input.GetKeyDown(KeyCode.Space))
        {
            // Membalik nilai isRecapeVisible
            isRecapeVisible = !isRecapeVisible;

            // Mengubah animasi berdasarkan nilai isRecapeVisible
            if (isRecapeVisible)
            {
                ShowRecape();
            }
            else
            {
                HideRecape();
            }
        }
    }

    private void ShowRecape()
    {
        animator.SetBool(HIDE_RECAPE, false);
    }

    private void HideRecape()
    {
        animator.SetBool(HIDE_RECAPE, true);
    }
}
