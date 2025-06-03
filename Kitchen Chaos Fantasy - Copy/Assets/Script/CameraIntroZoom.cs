using UnityEngine;

public class CameraIntroZoom : MonoBehaviour
{
    private Camera cam;

    [Header("Zoom Settings")]
    public float startFOV = 40f;       // Awalnya kamera dekat
    public float targetFOV = 60f;      // Lalu menjauh sedikit
    public float zoomDuration = 2f;    // Dalam waktu 2 detik

    private float timer = 0f;

    void Start()
    {
        cam = GetComponent<Camera>();
        cam.fieldOfView = startFOV;
    }

    void Update()
    {
        if (cam.fieldOfView < targetFOV)
        {
            timer += Time.deltaTime;
            cam.fieldOfView = Mathf.Lerp(startFOV, targetFOV, timer / zoomDuration);
        }
    }
}
