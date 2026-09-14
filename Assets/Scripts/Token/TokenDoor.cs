using UnityEngine;

public class TokenDoor : MonoBehaviour
{
    [SerializeField] Token[] tokens;
    [SerializeField] Transform door;
    [SerializeField] float moveSpeed;
    [SerializeField] bool isBigDoor;

    bool active;
    Vector3 startingPos;

    private void Awake()
    {
        startingPos = door.transform.position;
    }

    private void Update()
    {
        if (active) door.transform.position = Vector3.Lerp(door.transform.position, startingPos - new Vector3(0, door.transform.localScale.y - 0.1f, 0), Time.deltaTime * moveSpeed);
    }

    private void OnTriggerEnter(Collider other)
    {
        if (other.gameObject.GetComponent<PlayerController>())
        {
            foreach (Token t in tokens)
            {
                if (!t.collected) return;
            }

            active = true;
            if (!isBigDoor) door.gameObject.GetComponent<BoxCollider>().enabled = false;
        }
    }
}
