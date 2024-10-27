using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class MortarCounter : BaseCounter
{
    [SerializeField] private List<KitchenObjectSO> validCuttingObjects; // Daftar bahan yang bisa dipotong
    [SerializeField] private List<KitchenObjectSO> cuttingResults; // Daftar hasil potongan sesuai dengan bahan
    [SerializeField] private Slider cuttingProgressSlider; // Referensi ke Slider di UI

    private float cutDuration = 3f; // Durasi pemotongan dalam detik
    private float cuttingProgress = 0f; // Progres pemotongan
    private bool isCutting = false; // Apakah sedang dalam proses pemotongan
    private KitchenObject currentKitchenObject; // Objek yang sedang dipotong
    private Player interactingPlayer; // Pemain yang berinteraksi
    private Vector3 playerStartPosition; // Posisi pemain saat pemotongan dimulai

    private void Start()
    {
        if (cuttingProgressSlider != null)
        {
            cuttingProgressSlider.gameObject.SetActive(false); // Sembunyikan slider di awal
            cuttingProgressSlider.value = 0;
            cuttingProgressSlider.maxValue = 1; // Set nilai maksimum slider ke 1
        }
    }

    public override void Interact(Player player)
    {
        if (!HasKitchenObject())
        {
            if (player.HasKitchenObject())
            {
                player.GetKitchenObject().SetKitchenObjectParent(this);
            }
        }
        else
        {
            if (!player.HasKitchenObject())
            {
                GetKitchenObject().SetKitchenObjectParent(player);
            }
        }
    }

    public override void InteractAlternate(Player player)
    {
        if (HasKitchenObject())
        {
            if (!isCutting)
            {
                currentKitchenObject = GetKitchenObject();

                // Cek apakah objek yang ada adalah bahan yang bisa dipotong
                if (IsValidCuttingObject(currentKitchenObject))
                {
                    isCutting = true;
                    cuttingProgress = 0f;
                    cuttingProgressSlider.gameObject.SetActive(true);
                    cuttingProgressSlider.value = 0f;
                    interactingPlayer = player; // Simpan referensi pemain
                    playerStartPosition = player.Getposition(); // Simpan posisi pemain saat mulai memotong]
                    AudioEventSystem.PlayAudio("Mortar");
                }
                else
                {
                    Debug.Log("Bahan ini tidak bisa dipotong!");
                }
            }
        }
    }

    private void Update()
    {
        if (isCutting)
        {
            if (interactingPlayer != null && interactingPlayer.IsInteractAlternatePressed())
            {
                // Cek apakah pemain telah bergerak
                if (interactingPlayer.HasMovedSince(playerStartPosition))
                {
                    // Pemain bergerak, batalkan pemotongan
                    CancelCutting();
                    AudioEventSystem.StopAudio("Mortar");
                    return;
                }

                // Pemain masih menahan tombol dan tidak bergerak, lanjutkan pemotongan
                cuttingProgress += Time.deltaTime / cutDuration;
                cuttingProgressSlider.value = cuttingProgress;

                if (cuttingProgress >= 1f)
                {
                    // Pemotongan selesai
                    FinishCutting();
                }
            }
            else
            {
                // Pemain melepaskan tombol, batalkan pemotongan
                CancelCutting();
                AudioEventSystem.StopAudio("Mortar");
            }
        }
    }

    private void FinishCutting()
    {
        // Pemotongan selesai, hancurkan objek lama dan instantiate objek baru
        KitchenObjectSO resultObjectSO = GetCuttingResult(currentKitchenObject.GetKitchenObjectSO());
        if (resultObjectSO != null)
        {
            GetKitchenObject().DestroySelf();
            Transform resultObjectTransform = Instantiate(resultObjectSO.prefab);
            resultObjectTransform.GetComponent<KitchenObject>().SetKitchenObjectParent(this);
        }
        else
        {
            Debug.LogError("Tidak ditemukan hasil pemotongan untuk bahan ini!");
        }

        cuttingProgressSlider.gameObject.SetActive(false); // Sembunyikan slider
        isCutting = false; // Reset status pemotongan
        cuttingProgress = 0f;
        currentKitchenObject = null;
        interactingPlayer = null;
    }

    private void CancelCutting()
    {
        // Reset proses pemotongan
        isCutting = false;
        cuttingProgress = 0f;
        cuttingProgressSlider.value = 0f;
        cuttingProgressSlider.gameObject.SetActive(false);
        currentKitchenObject = null;
        interactingPlayer = null;
    }

    private bool IsValidCuttingObject(KitchenObject kitchenObject)
    {
        // Cek apakah objek saat ini adalah bahan yang bisa dipotong
        return validCuttingObjects.Contains(kitchenObject.GetKitchenObjectSO());
    }

    private KitchenObjectSO GetCuttingResult(KitchenObjectSO inputObjectSO)
    {
        // Cari hasil potongan sesuai dengan bahan yang dimasukkan
        int index = validCuttingObjects.IndexOf(inputObjectSO);
        if (index >= 0 && index < cuttingResults.Count)
        {
            return cuttingResults[index];
        }
        return null;
    }
}
