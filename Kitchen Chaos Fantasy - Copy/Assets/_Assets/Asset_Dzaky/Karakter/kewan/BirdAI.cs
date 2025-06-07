using System.Collections;
using UnityEngine;

public class BirdAI : MonoBehaviour
{
    public float moveSpeed = 3f;
    public float waitTimeMin = 2f;
    public float waitTimeMax = 4f;

    public float flyRadius = 10f; // Radius area terbang
    public float minFlyHeight = 5f;
    public float maxFlyHeight = 10f;

    public Transform centerPoint; // Titik pusat area terbang (bisa Empty GameObject di tengah pulau)

    private Vector3 targetPosition;
    private Animator animator;
    private bool isFlying = true;

    private void Start()
    {
        animator = GetComponent<Animator>();
        animator.SetBool("isFlying", true);

        PickNewTarget();
    }

    private void Update()
    {
        FlyToTarget();
    }

    void FlyToTarget()
    {
        transform.position = Vector3.MoveTowards(transform.position, targetPosition, moveSpeed * Time.deltaTime);
        transform.rotation = Quaternion.Slerp(transform.rotation, Quaternion.LookRotation(targetPosition - transform.position), 2f * Time.deltaTime);

        if (Vector3.Distance(transform.position, targetPosition) < 0.5f)
        {
            StartCoroutine(WaitAndPickNewTarget());
        }
    }

    IEnumerator WaitAndPickNewTarget()
    {
        isFlying = false;
        float waitTime = Random.Range(waitTimeMin, waitTimeMax);
        yield return new WaitForSeconds(waitTime);
        PickNewTarget();
        isFlying = true;
    }

    void PickNewTarget()
    {
        Vector3 randomOffset = new Vector3(
            Random.Range(-flyRadius, flyRadius),
            Random.Range(minFlyHeight, maxFlyHeight),
            Random.Range(-flyRadius, flyRadius)
        );

        targetPosition = centerPoint.position + randomOffset;
    }
}
