using UnityEngine;

public class TokenDisplay : MonoBehaviour
{
    MeshRenderer rend;
    [SerializeField] Material inactiveMaterial;
    [SerializeField] Material activeMaterial;

    public Token token;

    private void Awake()
    {
        rend = GetComponent<MeshRenderer>();
        rend.material = inactiveMaterial;
    }

    private void Update()
    {
        if (token.collected)
        {
            rend.material = activeMaterial;
        }
    }
}
