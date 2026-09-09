using UnityEngine;

public class SoundField : MonoBehaviour
{
    private float detectTimer = 0;
    [SerializeField] private EnemySenses steve;

    private void Update()
    {
        detectTimer += Time.deltaTime;
    }

    private void OnTriggerEnter(Collider other)
    {
        PlayerController player = other.gameObject.GetComponent<PlayerController>();

        if (player != null)
        {
            if (player.moving && !player.sneaking) steve.OnHeard(other.gameObject, GetComponent<SphereCollider>().radius);
        }
    }

    private void OnTriggerStay(Collider other)
    {
        if (detectTimer < 0.5) return;

        PlayerController player = other.gameObject.GetComponent<PlayerController>();

        if (player != null)
        {
            if (player.moving && !player.sneaking) steve.OnHeard(other.gameObject, GetComponent<SphereCollider>().radius);
            detectTimer = 0;
        }
    }
}
