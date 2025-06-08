using System.Collections;
using UnityEngine;
using UnityEngine.SceneManagement;

[RequireComponent(typeof(Rigidbody))]
public class PlayerLevelSelection : MonoBehaviour
{
    public float moveSpeed = 5f;
    [SerializeField] private GameInput gameInput;
    [SerializeField] private Animator animator;
    [SerializeField] private bgm_Level_Selection bgmLevelSelection;
    [SerializeField] private GameObject TRANSISIOUT;

    private Rigidbody rb;
    private bool isWalking;
    private LevelSelection currentLevelTrigger;

    private void Awake()
    {
        rb = GetComponent<Rigidbody>();
        Cursor.lockState = CursorLockMode.Locked;
        rb = GetComponent<Rigidbody>();

    }

    public void playBGMLevelSelection()
    {
        bgmLevelSelection.PlayBGM();
    }

    private void FixedUpdate()
    {
        HandleMovement();
    }

    private void Update()
    {
        animator.SetBool("IsWalking", isWalking);

        if (currentLevelTrigger != null && Input.GetKeyDown(KeyCode.E))
        {
            StartCoroutine(transitionOut());
        }
    }

    private IEnumerator transitionOut()
    {
        TRANSISIOUT.SetActive(true);
        yield return new WaitForSeconds(0.53f);

        if (bgmLevelSelection != null)
        {
            bgmLevelSelection.StopBGM();
        }

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

    private void OnTriggerEnter(Collider other)
    {
        if (other.CompareTag("LevelTrigger"))
        {
            LevelSelection levelTrigger = other.GetComponent<LevelSelection>();
            if (levelTrigger != null)
            {
                currentLevelTrigger = levelTrigger;

                // Aktifkan UI teks level
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
                // Nonaktifkan UI teks level
                if (levelTrigger.Level != null)
                    levelTrigger.Level.SetActive(false);

                levelTrigger.GadiInjak();

                if (currentLevelTrigger == levelTrigger)
                    currentLevelTrigger = null;
            }
        }
    }
}
