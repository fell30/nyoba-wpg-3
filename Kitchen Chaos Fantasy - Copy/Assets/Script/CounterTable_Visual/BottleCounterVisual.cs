using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BottleCounterVisual : MonoBehaviour
{
    [SerializeField] private BottleCounter bottleCounter;
    [SerializeField] private Transform counterTopPoint;
    [SerializeField] private Transform bottleVisualPrefarb;
    private List<GameObject> bottleVisualGameObjects = new List<GameObject>();
    private void Awake()
    {
        bottleVisualGameObjects = new List<GameObject>();
    }

    private void Start()
    {
        bottleCounter.OnBottleSpawned += BottleCounter_OnBottleSpawned;
        bottleCounter.OnBottleRemoved += BottleCounter_OnBottleRemoved;
    }
    private void BottleCounter_OnBottleRemoved(object sender, System.EventArgs e)
    {
        GameObject bottleGameObject = bottleVisualGameObjects[bottleVisualGameObjects.Count - 1];
        bottleVisualGameObjects.Remove(bottleGameObject);
        Destroy(bottleGameObject);
    }
    private void BottleCounter_OnBottleSpawned(object sender, System.EventArgs e)
    {
        Transform bottleVisualTransform = Instantiate(bottleVisualPrefarb, counterTopPoint);
        float bottleOffsetY = .5f;
        bottleVisualTransform.localPosition = new Vector3(0, bottleOffsetY * bottleVisualGameObjects.Count, 0);
        bottleVisualGameObjects.Add(bottleVisualTransform.gameObject);
    }
}
