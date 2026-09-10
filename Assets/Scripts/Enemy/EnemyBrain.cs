using UnityEngine;
using UnityEngine.AI;
using System.Collections.Generic;

public enum EnemyStates
{
    PATROL,
    INVESTIGATE,
    PURSUE
}

public class EnemyBrain : MonoBehaviour
{
    // Private Variables
    Rigidbody rb;
    NavMeshAgent agent;
    NavMeshPath currentPath;
    Queue<Vector3> remainingPoints;
    Vector3 currentPoint;
    Vector3 heardPoint;
    float timeSinceLastDestination = 0;
    float speed;
    float heightLevel;
    int currentWaypoint = 0;
    int totalWaypoints;

    public bool canMove = true;
    public EnemyStates state = EnemyStates.PATROL;
    [SerializeField] Light sightDisplayLight;

    [Header("State Timers")]
    [SerializeField] float pursueTime;
    float pursueTimer = 0;
    [SerializeField] float investigateTime;
    float investigateTimer = 0;

    [Header("Target Positions")]
    [SerializeField] Transform[] waypoints;
    [SerializeField] Transform player;

    [Header("Enemy Stats")]
    [SerializeField] float moveSpeed;
    [SerializeField] float chaseSpeed;
    [SerializeField] float turnSpeed;

    private void Awake()
    {
        agent = GetComponent<NavMeshAgent>();
        rb = GetComponent<Rigidbody>();
        currentPath = new NavMeshPath();
        remainingPoints = new Queue<Vector3>();
        speed = moveSpeed;
        heightLevel = transform.position.y;
        totalWaypoints = waypoints.Length;
        sightDisplayLight.color = Color.green;
    }

    private void Update()
    {
        switch (state)
        {
            case EnemyStates.PATROL:
                UpdatePatrol(); break;
            case EnemyStates.INVESTIGATE:
                UpdateInvestigate(); break;
            case EnemyStates.PURSUE:
                UpdatePursue(); break;
        }

        if (!canMove) return;

        Vector3 lookTarget = (currentPoint - transform.position);
        lookTarget.y = 0;

        Vector3 yNeutralAngle = transform.forward;
        yNeutralAngle.y = 0;

        transform.forward = Vector3.Lerp(yNeutralAngle, lookTarget.normalized, Time.deltaTime * turnSpeed);

        // Bounces enemy off walls if it is too close.
        // This prevents enemy from getting stuck from being in the gap between the wall and NavMesh.
        if (Physics.Raycast(transform.position, transform.forward, 0.52f, LayerMask.GetMask("Block Raycast")))
        {
            float fixedAngle = transform.rotation.eulerAngles.y + 180;
            if (fixedAngle >= 360) fixedAngle -= 360;

            transform.rotation = Quaternion.Euler(
                transform.rotation.eulerAngles.x,
                fixedAngle,
                transform.rotation.eulerAngles.z
            );
        }
    }

    public void StartPatrol()
    {
        sightDisplayLight.color = Color.green;
        speed = moveSpeed;
        state = EnemyStates.PATROL;
    }

    // canMove is set to true here instead of StartPatrol() because putting canMove = true in StartPatrol() would, for some reason, prevent proper transition from INVESTIGATE to PATROL.
    private void UpdatePatrol()
    {
        canMove = true;
        GetPathingFromAgent(waypoints[currentWaypoint].position, 0.2f);
    }

    public void StartInvestigation(Vector3 detectedPosition)
    {
        sightDisplayLight.color = Color.yellow;
        investigateTimer = 0;
        canMove = true;
        heardPoint = detectedPosition;
        state = EnemyStates.INVESTIGATE;
    }

    void UpdateInvestigate()
    {
        if (investigateTimer > investigateTime) StartPatrol();
        if (canMove) GetPathingFromAgent(heardPoint, 1, true);
        else investigateTimer += Time.deltaTime;
    }

    public void StartPursue()
    {
        sightDisplayLight.color = Color.red;
        pursueTimer = 0;
        canMove = true;
        speed = chaseSpeed;
        state = EnemyStates.PURSUE;
    }

    void UpdatePursue()
    {
        if (pursueTimer > pursueTime) StartInvestigation(player.position);
        if (canMove) GetPathingFromAgent(player.position);
        pursueTimer += Time.deltaTime;
    }

    // Called by the state-specific update functions.
    // This is a separate function to significantly reduce the length of this code.
    private void GetPathingFromAgent(Vector3 specificTarget, float switchWaypointBuffer = 0, bool stopAtEnd = false)
    {
        if (timeSinceLastDestination < 1)
        {
            remainingPoints.Clear();
            agent.enabled = true;

            agent.CalculatePath(specificTarget, currentPath);
            foreach (Vector3 p in currentPath.corners)
            {
                remainingPoints.Enqueue(p);
            }

            remainingPoints.Dequeue();
            currentPoint = remainingPoints.Dequeue();

            agent.enabled = false;
            timeSinceLastDestination = 0;

            if (Vector3.Distance(transform.position, specificTarget) < switchWaypointBuffer)
            {
                if (stopAtEnd) canMove = false;
                else
                {
                    currentWaypoint++;
                    if (currentWaypoint >= totalWaypoints) currentWaypoint = 0;
                }
            }
        }

        if (Vector3.Distance(transform.position, currentPoint) < 0.2f)
        {
            currentPoint = remainingPoints.Dequeue();
        }

        timeSinceLastDestination += Time.deltaTime;
    }

    private void FixedUpdate()
    {
        if (!canMove) return;

        rb.linearVelocity = transform.forward * speed;
        transform.position = new Vector3(transform.position.x, heightLevel, transform.position.z);
    }

    private void OnDrawGizmos()
    {
        if (agent == null) return;

        Gizmos.color = Color.yellow;

        foreach (Vector3 p in currentPath.corners)
        {
            Gizmos.DrawSphere(p, 0.25f);
        }
    }
}