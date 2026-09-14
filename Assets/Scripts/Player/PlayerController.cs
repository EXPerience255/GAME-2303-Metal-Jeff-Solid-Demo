using UnityEngine;
using UnityEngine.SceneManagement;

public class PlayerController : MonoBehaviour
{
    private PlayerInputSystem inputs;
    private CharacterController cc;
    private Scene scene;
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
        scene = SceneManager.GetActiveScene();
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

    private void Update()
    {
        if (inputs.Basic.End.IsPressed()) Application.Quit();
    }

    private void OnCollisionEnter(Collision col)
    {
        // not sure why LayerMask.GetMask("Enemy") didn't work, but hard-coding is always an option
        if (col.gameObject.layer == 6)
        {
            SceneManager.LoadScene(scene.name);
        }
    }
}
