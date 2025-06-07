using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

[RequireComponent(typeof(Rigidbody))]
public class PlayerLevelSelection : MonoBehaviour
{
    public float moveSpeed = 5f;
    [SerializeField] private GameInput gameInput;
    [SerializeField] private Animator animator;


    private Rigidbody rb;
    private bool isWalking;
    private LevelSelection currentLevelTrigger;
    [SerializeField] private GameObject TRANSISIOUT;

    private void Awake()
    {
        rb = GetComponent<Rigidbody>();
        Cursor.lockState = CursorLockMode.Locked;

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
        SceneManager.LoadScene(currentLevelTrigger.sceneToLoad);
        //TRANSISIOUT.SetActive(false);
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
