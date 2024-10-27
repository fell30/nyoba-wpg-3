using System.Collections;
using System.Collections.Generic;
//using System.Diagnostics;
using UnityEngine;

public class KitchenObject : MonoBehaviour
{
    [SerializeField] private KitchenObjectSO kitchenObjectSO;
    private IkitchenObjectParent kitchenObjectParent;



    public KitchenObjectSO GetKitchenObjectSO()
    {
        return kitchenObjectSO;
    }
    public void SetKitchenObjectParent(IkitchenObjectParent kitchenObjectParent)
    {
        if (this.kitchenObjectParent != null)
        {
            this.kitchenObjectParent.ClearKitchenObject();
        }
        this.kitchenObjectParent = kitchenObjectParent;
        if (kitchenObjectParent.HasKitchenObject())
        {
            // Debug.LogError("IKichenObjectParent already has a kitchen object");
        }
        kitchenObjectParent.SetKitchenObject(this);
        transform.parent = kitchenObjectParent.GetKitchenObjectFollowTransform();
        transform.localPosition = Vector3.zero;

        if (kitchenObjectParent is Player)
        {
            AudioEventSystem.PlayAudio("PickupSound");
        }
        else
        {
            AudioEventSystem.PlayAudio("DroupSound");
        }
    }


    public IkitchenObjectParent GetIkitchenObjectParent()
    {
        return kitchenObjectParent;
    }

    public void DestroySelf()
    {

        kitchenObjectParent.ClearKitchenObject();
        Destroy(gameObject);
    }
}
