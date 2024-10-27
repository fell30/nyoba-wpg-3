using UnityEngine;
using UnityEngine.AI;

public class NpcPATROL : MonoBehaviour
{
    public enum State
    {
        Patrol,
        Wait
    }

    public State currentState;
    public Transform[] patrolPoints;
    public float patrolSpeed = 2.0f;
    public float waitTime = 2.0f;

    private NavMeshAgent agent;
    private Animator animator;
    private int currentPatrolIndex;
    private float waitTimer;

    void Start()
    {
        agent = GetComponent<NavMeshAgent>();
        animator = GetComponent<Animator>();
        currentPatrolIndex = 0;
        waitTimer = waitTime;

        // Set the initial destination
        if (patrolPoints.Length > 0)
        {
            agent.SetDestination(patrolPoints[currentPatrolIndex].position);
            //Debug.Log("Setting destination to patrol point: " + patrolPoints[currentPatrolIndex].position);
        }

        SetState(State.Patrol);
    }

    void Update()
    {
        switch (currentState)
        {
            case State.Patrol:
                Patrol();
                break;
            case State.Wait:
                Wait();
                break;
        }
    }

    void Patrol()
    {
        agent.speed = patrolSpeed;

        // Set patrol animation if it's not already playing
        SetAnimation("Patrol", "StartPatrol");

        // Cek rintangan di depan menggunakan Raycast
        RaycastHit hit;
        Vector3 forward = transform.TransformDirection(Vector3.forward);

        // Raycast dari posisi NPC dengan panjang 2 meter
        if (Physics.Raycast(transform.position, forward, out hit, 2f))
        {
            if (hit.collider.CompareTag("Obstacle"))
            {
                // Debug.Log("Obstacle Detected: " + hit.collider.name);
                // Arahkan NPC untuk menghindari obstacle, bisa ubah destinasi
                AvoidObstacle();
            }
        }

        if (agent.remainingDistance <= agent.stoppingDistance)
        {
            SetState(State.Wait);
        }
    }

    void AvoidObstacle()
    {
        // Logika sederhana untuk menghindari obstacle,
        // misalnya kamu bisa langsung ubah patrol point
        currentPatrolIndex = (currentPatrolIndex + 1) % patrolPoints.Length;
        agent.SetDestination(patrolPoints[currentPatrolIndex].position);
        // Debug.Log("Avoiding obstacle, moving to next patrol point.");
    }

    void Wait()
    {
        agent.speed = 0;

        // Set wait animation if it's not already playing
        SetAnimation("Wait", "StartWait");

        if (waitTimer <= 0)
        {
            currentPatrolIndex = (currentPatrolIndex + 1) % patrolPoints.Length;
            agent.SetDestination(patrolPoints[currentPatrolIndex].position);
            SetState(State.Patrol);
        }
        else
        {
            waitTimer -= Time.deltaTime;
        }
    }

    void SetState(State newState)
    {
        currentState = newState;
        if (currentState == State.Wait)
        {
            waitTimer = waitTime;
        }
    }

    // This method handles setting the animation state
    void SetAnimation(string animationName, string triggerName)
    {
        if (!animator.GetCurrentAnimatorStateInfo(0).IsName(animationName))
        {
            animator.SetTrigger(triggerName);
        }
    }
}
