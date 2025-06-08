using System.Collections;
using UnityEngine;
using UnityEngine.SceneManagement;

[RequireComponent(typeof(Rigidbody))]
public class PlayerLevelSelection : MonoBehaviour
{
    public float moveSpeed = 5f;
    [SerializeField] private GameInput gameInput;

    private Rigidbody rb;
    private bool isWalking;
    private bool canMove = false;  // ⛔ Tambahan: kontrol gerakan player
    private LevelSelection currentLevelTrigger;
    [SerializeField] private GameObject TRANSISIOUT;

    private void Awake()
    {
        rb = GetComponent<Rigidbody>();
        Cursor.lockState = CursorLockMode.Locked;
    }

    private void Start()
    {
        DisableMovement(); // ⛔ Tambahan: agar player tidak langsung bisa gerak saat start
    }

    private void FixedUpdate()
    {
        if (!canMove) return; // ⛔ Tambahan: tahan gerakan
        HandleMovement();
    }

    private void Update()
    {
        if (!canMove) return; // ⛔ Tambahan: tahan input

        if (currentLevelTrigger != null && Input.GetKeyDown(KeyCode.E))
        {
            StartCoroutine(transitionOut());
        }
    }

    private IEnumerator transitionOut()
    {
        TRANSISIOUT.SetActive(true);
        yield return new WaitForSeconds(0.53f);
        SceneManager.LoadScene(currentLevelTrigger.sceneToLoad);
    }

    private void HandleMovement()
    {
        Vector2 inputVector = gameInput.GetMovmentInputNormalized();
        Vector3 moveDir = new Vector3(inputVector.x, 0f, inputVector.y);

        Vector3 moveAmount = moveDir * moveSpeed * Time.fixedDeltaTime;
        rb.MovePosition(rb.position + moveAmount);

        isWalking = moveDir != Vector3.zero;

        if (isWalking)
        {
            float rotationSpeed = 10f;
            Vector3 newDir = Vector3.Slerp(transform.forward, moveDir, Time.fixedDeltaTime * rotationSpeed);
            rb.rotation = Quaternion.LookRotation(newDir);
        }
    }

    public bool IsWalking()
    {
        return isWalking;
    }

    public void EnableMovement()  // ✅ Fungsi untuk mengaktifkan gerak
    {
        canMove = true;
    }

    public void DisableMovement() // ✅ Fungsi untuk menonaktifkan gerak
    {
        canMove = false;
    }

    private void OnTriggerEnter(Collider other)
    {
        if (other.CompareTag("LevelTrigger"))
        {
            LevelSelection levelTrigger = other.GetComponent<LevelSelection>();
            if (levelTrigger != null)
            {
                currentLevelTrigger = levelTrigger;
                if (levelTrigger.Level != null)
                    levelTrigger.Level.SetActive(true);
                levelTrigger.Injak();
            }
        }
    }

    private void OnTriggerExit(Collider other)
    {
        if (other.CompareTag("LevelTrigger"))
        {
            LevelSelection levelTrigger = other.GetComponent<LevelSelection>();
            if (levelTrigger != null)
            {
                if (levelTrigger.Level != null)
                    levelTrigger.Level.SetActive(false);
                levelTrigger.GadiInjak();

                if (currentLevelTrigger == levelTrigger)
                    currentLevelTrigger = null;
            }
        }
    }
}
