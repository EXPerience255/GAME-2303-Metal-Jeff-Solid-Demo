using UnityEngine;

public class Token : MonoBehaviour
{
    Animator anim;
    protected MeshRenderer rend;
    [SerializeField] Material inactiveMaterial;
    [SerializeField] Material activeMaterial;

    public bool collected;

    private void Awake()
    {
        anim = GetComponent<Animator>();
        rend = GetComponent<MeshRenderer>();
        rend.material = inactiveMaterial;
    }

    private void OnTriggerEnter(Collider other)
    {
        if (other.gameObject.GetComponent<PlayerController>())
        {
            collected = true;
            anim.SetBool("collected", collected);
            rend.material = activeMaterial;
            GetComponent<SphereCollider>().enabled = false;
        }
    }
}
