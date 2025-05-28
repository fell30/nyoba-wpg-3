using UnityEngine;

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
        Vector3 targetPosition = target.position + _offset;
        transform.position = Vector3.SmoothDamp(transform.position, targetPosition, ref _CurrentVelocity, smoothTime);
    }
}
