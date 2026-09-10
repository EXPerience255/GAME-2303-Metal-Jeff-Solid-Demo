using UnityEngine;

public class EnemyAnimator : MonoBehaviour
{
    [SerializeField] EnemyBrain brain;
    Animator anim;

    private void Awake()
    {
        anim = GetComponent<Animator>();
    }

    private void Update()
    {
        if (brain.canMove) anim.SetBool("canMove", true);
        else anim.SetBool("canMove", false);

        if (brain.state == EnemyStates.PURSUE) anim.SetBool("isPursue", true);
        else if (brain.state == EnemyStates.PATROL) anim.SetBool("isPursue", false);
    }
}
