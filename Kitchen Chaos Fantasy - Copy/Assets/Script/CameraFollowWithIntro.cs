using System.Collections;
using UnityEngine;

public class CameraFollowWithIntro : MonoBehaviour
{
    [Header("Target Settings")]
    public Transform target;
    [SerializeField] private PlayerLevelSelection playerScript;

    [Header("Positions")]
    public Transform menuTransform;
    public Vector3 gameplayOffset = new Vector3(0, 15f, -5f); // Offset bisa di-tweak sesuai posisi

    [Header("Camera Rotation")]
    [SerializeField] private Vector3 gameplayEulerAngles = new Vector3(75f, 0f, 0f); // 75 derajat dari atas
    private Quaternion _followRotation;

    [Header("Smooth Follow")]
    [SerializeField] private float smoothTime = 0.3f;
    private Vector3 _currentVelocity = Vector3.zero;
    private bool _isFollowing = false;
    private bool _isTransitioning = false;

    [Header("FOV Settings")]
    [SerializeField] private float startFOV = 40f;
    [SerializeField] private float targetFOV = 60f;
    [SerializeField] private float zoomDuration = 2f;

    private Camera _cam;

    private void Awake()
    {
        _cam = GetComponent<Camera>();
        if (_cam == null)
        {
            Debug.LogError("[CameraFollow] No Camera component found!");
            enabled = false;
            return;
        }

        _followRotation = Quaternion.Euler(gameplayEulerAngles);

        if (menuTransform != null)
        {
            transform.position = menuTransform.position;
            transform.rotation = menuTransform.rotation;
        }
        else
        {
            Debug.LogWarning("[CameraFollow] MenuTransform is not assigned!");
        }

        _cam.fieldOfView = startFOV;
    }

    /// <summary>
    /// Dipanggil saat tombol Play ditekan.
    /// </summary>
    public void StartIntroTransition()
    {
        if (target == null)
        {
            Debug.LogWarning("[CameraFollow] Target is not assigned!");
            return;
        }

        if (!_isTransitioning)
        {
            StartCoroutine(PlayIntro());
        }
    }

    private IEnumerator PlayIntro()
    {
        _isTransitioning = true;

        Vector3 startPos = transform.position;
        Quaternion startRot = transform.rotation;
        float elapsed = 0f;

        Vector3 endPos = target.position + gameplayOffset;
        Quaternion endRot = _followRotation;

        Debug.Log("[CameraFollow] Starting intro transition...");

        while (elapsed < zoomDuration)
        {
            elapsed += Time.deltaTime;
            float t = Mathf.Clamp01(elapsed / zoomDuration);

            transform.position = Vector3.Lerp(startPos, endPos, t);
            transform.rotation = Quaternion.Slerp(startRot, endRot, t);
            _cam.fieldOfView = Mathf.Lerp(startFOV, targetFOV, t);

            yield return null;
        }

        // Final state
        transform.position = endPos;
        transform.rotation = endRot;
        _cam.fieldOfView = targetFOV;

        _isFollowing = true;
        _isTransitioning = false;

        Debug.Log("[CameraFollow] Intro transition complete. Camera is now following.");

        // Aktifkan player movement setelah transisi
        playerScript?.EnableMovement();
    }

    private void LateUpdate()
    {
        if (_isFollowing && !_isTransitioning && target != null)
        {
            Vector3 targetPos = target.position + gameplayOffset;
            transform.position = Vector3.SmoothDamp(transform.position, targetPos, ref _currentVelocity, smoothTime);
            transform.rotation = _followRotation;
        }
    }
}
