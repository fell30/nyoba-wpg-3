using UnityEngine;
using System.Collections;

public class CameraFollow : MonoBehaviour
{
    private Vector3 _offset;
    [SerializeField] private Transform target;

    [Header("Smooth Settings")]
    [SerializeField] private float smoothTime;
    private Vector3 _CurrentVelocity = Vector3.zero;
    [SerializeField] private float minX, maxX, minZ, maxZ;


    private void Awake()
    {
        _offset = transform.position - target.position;
    }

    private void LateUpdate()
    {
        if (target == null) return;

        Vector3 targetPosition = target.position + _offset;

        // Clamp posisi kamera agar tidak melewati batas
        float clampedX = Mathf.Clamp(targetPosition.x, minX, maxX);
        float clampedZ = Mathf.Clamp(targetPosition.z, minZ, maxZ);

        Vector3 clampedPosition = new Vector3(clampedX, targetPosition.y, clampedZ);

        transform.position = Vector3.SmoothDamp(transform.position, clampedPosition, ref _CurrentVelocity, smoothTime);
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

    private void OnDrawGizmosSelected()
    {
        Gizmos.color = Color.red;
        Gizmos.DrawLine(new Vector3(minX, transform.position.y, minZ), new Vector3(minX, transform.position.y, maxZ));
        Gizmos.DrawLine(new Vector3(maxX, transform.position.y, minZ), new Vector3(maxX, transform.position.y, maxZ));
        Gizmos.DrawLine(new Vector3(minX, transform.position.y, minZ), new Vector3(maxX, transform.position.y, minZ));
        Gizmos.DrawLine(new Vector3(minX, transform.position.y, maxZ), new Vector3(maxX, transform.position.y, maxZ));
    }

}
