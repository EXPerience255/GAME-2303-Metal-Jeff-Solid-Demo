using UnityEngine;

public class PlayerController : MonoBehaviour
{
    private PlayerInputSystem inputs;
    private CharacterController cc;
    private Vector2 moveInput;
    private float speed;

    public bool moving;
    public bool sneaking;
    public float moveSpeed;
    public float sneakSpeed;

    private void Awake()
    {
        inputs = new PlayerInputSystem();
        cc = GetComponent<CharacterController>();
        speed = moveSpeed;
    }

    private void OnEnable()
    {
        inputs.Enable();
    }

    private void OnDisable()
    {
        inputs.Disable();
    }

    private void FixedUpdate()
    {
        moveInput = inputs.Basic.Movement.ReadValue<Vector2>();
        if (moveInput != new Vector2(0, 0)) moving = true; else moving = false;
        Vector3 moveDir = new Vector3(
            moveInput.x * Mathf.Cos(Mathf.PI / 4) - moveInput.y * Mathf.Cos(Mathf.PI / 4),
            0,
            moveInput.x * Mathf.Cos(Mathf.PI / 4) + moveInput.y * Mathf.Cos(Mathf.PI / 4)
        ).normalized;
        cc.Move(moveDir * speed * Time.deltaTime);

        sneaking = inputs.Basic.Sneak.IsPressed();
        if (sneaking) speed = sneakSpeed; else speed = moveSpeed;
    }
}
