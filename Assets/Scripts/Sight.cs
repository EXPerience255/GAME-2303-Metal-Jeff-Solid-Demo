using UnityEngine;

public class Sight : MonoBehaviour
{
    float dot;
    bool seen;

    [SerializeField] Transform target;
    [SerializeField] LayerMask targetLayer;
    [SerializeField] float range;

    private void Update()
    {
        Vector3 forwardDir = transform.forward;
        Vector3 toTarget = (target.position - transform.position).normalized;

        dot = Vector3.Dot(forwardDir, toTarget);

        RaycastHit hit;

        if (Physics.Raycast(transform.position, toTarget, out hit, range, targetLayer))
        {
            if (hit.transform.gameObject.GetComponent<PlayerController>()) seen = true;
            else seen = false;
        }
        else
        {
            seen = false;
        }
    }

    private void OnDrawGizmos()
    {
        Gizmos.color = Color.black;
        Gizmos.DrawLine(transform.position, target.position);

        Gizmos.color = Color.red;
        if (dot > 0.8f && seen) Gizmos.color = Color.green;
        Vector3 endPoint = transform.position + transform.forward * 10;
        Gizmos.DrawLine(transform.position, endPoint);
    }
}
