using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CameraZoomOut : MonoBehaviour
{
    public IEnumerator ZoomOutCoroutine(float targetFOV, float duration)
    {
        Camera cam = GetComponent<Camera>();
        float startFOV = cam.fieldOfView;
        float elapsed = 0f;
        

        while (elapsed < duration)
        {
            elapsed += Time.deltaTime;
            cam.fieldOfView = Mathf.Lerp(startFOV, targetFOV, elapsed / duration);
            yield return null;
        }

        cam.fieldOfView = targetFOV;
    }
}
