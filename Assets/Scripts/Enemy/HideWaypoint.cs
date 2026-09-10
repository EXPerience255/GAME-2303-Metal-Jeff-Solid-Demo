using UnityEngine;

public class HideWaypoint : MonoBehaviour
{
    void Start()
    {
        gameObject.GetComponent<MeshRenderer>().enabled = false;
    }
}
