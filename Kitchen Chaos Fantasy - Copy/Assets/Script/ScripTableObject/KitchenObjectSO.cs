using System.Collections;
using System.Collections.Generic;
using System.Runtime.CompilerServices;
using UnityEngine;

[CreateAssetMenu()]
public class KitchenObjectSO : ScriptableObject
{
    public Transform prefab;

    public Sprite recipeSprite;
    public Sprite IconObject;
    public string objectName;
    [Header(" Reward Setting")]
    public int GoldReward;
}
