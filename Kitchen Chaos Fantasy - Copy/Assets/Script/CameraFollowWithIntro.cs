using System.Collections;
using UnityEngine;

public class CameraFollowWithIntro : MonoBehaviour
{
    [Header("Target Settings")]
    [SerializeField] private Transform target;

    [Header("Follow Settings")]
    [SerializeField] private float smoothTime = 0.3f;
    private Vector3 _offset;
    private Vector3 _currentVelocity = Vector3.zero;
    private bool _isFollowing = false;

    [Header("Intro Animation")]
    [SerializeField] private float introMoveDuration = 2f;
    [SerializeField] private float startYOffset = -10f;
    [SerializeField] private float startFOV = 40f;
    [SerializeField] private float targetFOV = 60f;
    [SerializeField] private float zoomDuration = 2f;

    private Camera _cam;

    private void Awake()
    {
        _cam = GetComponent<Camera>();
    }

    private void Start()
    {
        if (target == null) return;

        // Simpan posisi offset dari kamera ke target, berdasarkan posisi kamera awal di Scene (posisi follow normal)
        _offset = transform.position - target.position;

        // Hitung posisi target akhir (posisi follow normal)
        Vector3 finalPosition = target.position + _offset;

        // Hitung posisi awal dari bawah (startYOffset ke bawah)
        Vector3 startPosition = finalPosition + new Vector3(0f, startYOffset, 0f);
        transform.position = startPosition;

        // Atur FOV awal kamera
        _cam.fieldOfView = startFOV;

        // Mulai animasi transisi kamera
        StartCoroutine(PlayIntro(finalPosition));
    }

    private void LateUpdate()
    {
        if (_isFollowing && target != null)
        {
            Vector3 targetPosition = target.position + _offset;
            transform.position = Vector3.SmoothDamp(transform.position, targetPosition, ref _currentVelocity, smoothTime);
        }
    }

    private IEnumerator PlayIntro(Vector3 finalPosition)
    {
        float elapsed = 0f;
        Vector3 startPosition = transform.position;
        float initialFOV = _cam.fieldOfView;

        while (elapsed < introMoveDuration)
        {
            elapsed += Time.deltaTime;

            float t = Mathf.Clamp01(elapsed / introMoveDuration);
            transform.position = Vector3.Lerp(startPosition, finalPosition, t);

            float zoomT = Mathf.Clamp01(elapsed / zoomDuration);
            _cam.fieldOfView = Mathf.Lerp(startFOV, targetFOV, zoomT);

            yield return null;
        }

        // Pastikan posisi dan FOV tepat di akhir
        transform.position = finalPosition;
        _cam.fieldOfView = targetFOV;

        // Mulai mengikuti player setelah intro selesai
        _isFollowing = true;
    }
}
