using System.Collections;
using UnityEngine;
using FMODUnity;

public class RespawnableMushroom : MonoBehaviour
{
    [SerializeField] private GameObject mushroomObject; // ini adalah jamur variant dari hierarchy
    [SerializeField] private float respawnDelay = 5f;

    private Vector3 originalPosition;
    private Quaternion originalRotation;

    private void Start()
    {
        if (mushroomObject != null)
        {
            originalPosition = mushroomObject.transform.localPosition;
            originalRotation = mushroomObject.transform.localRotation;
        }
    }

    public void RespawnMushroom()
    {
        StartCoroutine(RespawnRoutine());
    }

    private IEnumerator RespawnRoutine()
    {
        yield return new WaitForSeconds(respawnDelay);

        if (mushroomObject != null)
        {
            mushroomObject.SetActive(true);
            mushroomObject.transform.localPosition = originalPosition;
            mushroomObject.transform.localRotation = originalRotation;
        }
    }
}
