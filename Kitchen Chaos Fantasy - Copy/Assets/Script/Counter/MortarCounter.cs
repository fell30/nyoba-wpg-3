using System;
using System.Collections;
using System.Collections.Generic;
using JetBrains.Annotations;
using UnityEngine;
using UnityEngine.UI;

public class MortarCounter : BaseCounter, IHasProgress {
    public event EventHandler<IHasProgress.OnProgressChangedEventArgs> OnProgressChanged;

    public event EventHandler OnCut;
    [SerializeField] private MortarRecipeSO[] cuttingObjectSOArray;
    [SerializeField] private SFXMortar sfxMortar;

    private int MortarProgress;


    public override void Interact(Player player) {
        if (!HasKitchenObject()) {
            if (player.HasKitchenObject()) {
                if (HasRecipeWithInput(player.GetKitchenObject().GetKitchenObjectSO())) {

                    player.GetKitchenObject().SetKitchenObjectParent(this);

                    MortarProgress = 0;
                    sfxMortar.PlayTaruhSound(); // SFX saat meletakkan bahan

                    MortarRecipeSO mortarRecipeSO = GetMortarRecipeSOWithInput(GetKitchenObject().GetKitchenObjectSO());
                    OnProgressChanged?.Invoke(this, new IHasProgress.OnProgressChangedEventArgs { progressNormalized = (float)MortarProgress / mortarRecipeSO.mortarProgressMax });
                }

            } else {

            }
        } else {
            if (player.HasKitchenObject()) {

            } else {
                GetKitchenObject().SetKitchenObjectParent(player);
            }
        }


    }
    public override void InteractAlternate(Player player) {
        if (HasKitchenObject() && HasRecipeWithInput(GetKitchenObject().GetKitchenObjectSO())) {
            MortarProgress++;
            sfxMortar.PlayMortarSound(); // SFX saat proses tumbuk/uleg

            MortarRecipeSO mortarRecipeSO = GetMortarRecipeSOWithInput(GetKitchenObject().GetKitchenObjectSO());

            OnCut?.Invoke(this, EventArgs.Empty);

            OnProgressChanged?.Invoke(this, new IHasProgress.OnProgressChangedEventArgs { progressNormalized = (float)MortarProgress / mortarRecipeSO.mortarProgressMax });

            if (MortarProgress >= mortarRecipeSO.mortarProgressMax) {
                KitchenObjectSO outputKitchenObjectSO = GetOutputForInput(GetKitchenObject().GetKitchenObjectSO());
                GetKitchenObject().DestroySelf();
                KitchenObject.SpawnKitchenObject(outputKitchenObjectSO, this);

            }

        }
    }

    private bool HasRecipeWithInput(KitchenObjectSO inputKitchenObjectSO) {
        MortarRecipeSO mortarRecipeSO = GetMortarRecipeSOWithInput(inputKitchenObjectSO);
        return mortarRecipeSO != null;

    }
    private KitchenObjectSO GetOutputForInput(KitchenObjectSO inputKitchenObjectSO) {
        MortarRecipeSO mortarRecipeSO = GetMortarRecipeSOWithInput(inputKitchenObjectSO);
        if (mortarRecipeSO != null) {
            return mortarRecipeSO.output;
        } else {
            return null;
        }

    }

    private MortarRecipeSO GetMortarRecipeSOWithInput(KitchenObjectSO inputKitchenObjectSO) {
        foreach (MortarRecipeSO mortarRecipeSO in cuttingObjectSOArray) {
            if (mortarRecipeSO.input == inputKitchenObjectSO) {
                return mortarRecipeSO;
            }
        }
        return null;
    }
}
