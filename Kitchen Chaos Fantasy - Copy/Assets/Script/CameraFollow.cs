using UnityEngine;
using System.Collections;

public class CameraFollow : MonoBehaviour
{
    private Vector3 _offset;
    [SerializeField] private Transform target;

    [Header("Smooth Settings")]
    [SerializeField] private float smoothTime;
    private Vector3 _CurrentVelocity = Vector3.zero;

    private void Awake()
    {
        _offset = transform.position - target.position;
    }

    private void LateUpdate()
    {
        if (target == null) return;

        Vector3 targetPosition = target.position + _offset;
        transform.position = Vector3.SmoothDamp(transform.position, targetPosition, ref _CurrentVelocity, smoothTime);
    }

    //Fungsi untuk efek Zoom Out 
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
