using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.AI;

public class ChickenAI : MonoBehaviour
{
    [SerializeField] private List<Transform> pathPoints; // Titik-titik jalur
    [SerializeField] private float waitTimeMin = 2f;
    [SerializeField] private float waitTimeMax = 5f;

    private NavMeshAgent agent;
    public Animator animator;
    private bool isWaiting = false;

    private void Start()
    {
        agent = GetComponent<NavMeshAgent>();


        GoToRandomPoint();
    }

    private void Update()
    {
        if (!agent.pathPending && agent.remainingDistance < 0.5f && !isWaiting)
        {
            StartCoroutine(WaitAndMaybeEat());
        }

        animator.SetBool("isWalking", agent.velocity.magnitude > 0.1f);
    }

    private void GoToRandomPoint()
    {
        if (pathPoints.Count == 0) return;

        int index = Random.Range(0, pathPoints.Count);
        agent.SetDestination(pathPoints[index].position);
    }

    private IEnumerator WaitAndMaybeEat()
    {
        isWaiting = true;

        // Berhenti sebentar
        float waitTime = Random.Range(waitTimeMin, waitTimeMax);
        agent.isStopped = true;

        // 50% kemungkinan makan
        if (Random.value > 0.5f)
        {
            animator.SetTrigger("Eat");

        }

        yield return new WaitForSeconds(waitTime);

        agent.isStopped = false;
        GoToRandomPoint();
        isWaiting = false;
    }
}
