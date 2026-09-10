using UnityEngine;

public class PlayerAnimator : MonoBehaviour
{
    [SerializeField] PlayerController player;
    Animator anim;

    private void Awake()
    {
        anim = GetComponent<Animator>();
    }

    private void Update()
    {
        if (anim != null)
        {
            if (player.moving) anim.SetBool("isMoving", true);
            else anim.SetBool("isMoving", false);

            if (player.sneaking) anim.SetBool("isSneaking", true);
            else anim.SetBool("isSneaking", false);
        }
    }
}
