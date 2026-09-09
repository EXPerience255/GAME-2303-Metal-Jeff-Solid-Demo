using UnityEngine;
using UnityEngine.AI;
using System.Collections.Generic;

public class Navigator : MonoBehaviour
{
    Rigidbody rb;
    NavMeshAgent agent;
    NavMeshPath currentPath;
    Queue<Vector3> remainingPoints;
    Vector3 currentPoint;
    float timeSinceLastDestination = 0f;

    [SerializeField] Transform waypoint;
    [SerializeField] float moveSpeed;

    private void Start()
    {
        agent = GetComponent<NavMeshAgent>();
        rb = GetComponent<Rigidbody>();
        currentPath = new NavMeshPath();
        remainingPoints = new Queue<Vector3>();
    }

    private void Update()
    {
        if (timeSinceLastDestination < 1)
        {
            remainingPoints.Clear();
            agent.enabled = true;

            agent.CalculatePath(waypoint.position, currentPath);
            foreach (Vector3 p in currentPath.corners)
            {
                remainingPoints.Enqueue(p);
            }

            remainingPoints.Dequeue();
            currentPoint = remainingPoints.Dequeue();

            agent.enabled = false;
            timeSinceLastDestination = 0;
        }
        
        timeSinceLastDestination += Time.deltaTime;

        Vector3 lookTarget = (currentPoint - transform.position);
        lookTarget.y = 0;
        transform.forward = lookTarget.normalized;
    }

    private void FixedUpdate()
    {
        rb.linearVelocity = transform.forward * moveSpeed;
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