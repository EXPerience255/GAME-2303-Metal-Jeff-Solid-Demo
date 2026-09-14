using UnityEngine;

public class EnemySenses : MonoBehaviour
{
    EnemyBrain brain;
    float dot;
    bool seen;

    [SerializeField] Transform target;
    [SerializeField] LayerMask targetLayer;
    [SerializeField] float range;
    [SerializeField] float dotRange;

    private void Awake()
    {
        brain = GetComponent<EnemyBrain>();
    }

    private void Update()
    {
        Vector3 forwardDir = transform.forward;
        Vector3 toTarget = (target.position - transform.position).normalized;

        dot = Vector3.Dot(forwardDir, toTarget);

        RaycastHit hit;

        if (Physics.Raycast(transform.position, toTarget, out hit, range, targetLayer))
        {
            PlayerController jeff = hit.transform.gameObject.GetComponent<PlayerController>();
            if (jeff)
            {
                seen = true;
                if (dot > dotRange && !jeff.debug) brain.StartPursue();
            }
            else seen = false;
        }
        else
        {
            seen = false;
        }
    }

    public void OnHeard(GameObject player, float soundRange)
    {
        if (brain.state == EnemyStates.PURSUE) return;

        Vector3 forwardDir = transform.forward;
        Vector3 toTarget = (target.position - transform.position).normalized;

        RaycastHit hit;

        if (Physics.Raycast(transform.position, toTarget, out hit, soundRange, targetLayer))
        {
            if (hit.transform.gameObject.GetComponent<PlayerController>())
            {
                brain.StartInvestigation(player.transform.position);
            }
        }
    }

    private void OnDrawGizmos()
    {
        Gizmos.color = Color.black;
        Gizmos.DrawLine(transform.position, target.position);

        Gizmos.color = Color.red;
        if (dot > dotRange && seen) Gizmos.color = Color.green;
        Vector3 endPoint = transform.position + transform.forward * 10;
        Gizmos.DrawLine(transform.position, endPoint);
    }
}
