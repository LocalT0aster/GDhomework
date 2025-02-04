using UnityEngine;

[RequireComponent(typeof(CharacterController))]
public class FPSInput : MonoBehaviour {
    public float Gravity = -9.80665f;
    [Header("Walking")]
    public float WalkingSpeed = 1f;
    public float RunningSpeed = 2f;
    public KeyCode RunKeyCode = KeyCode.LeftShift;
    // Runtime variables
    private float currentSpeed = 1f;
    private float deltaSpeed = 0f;
    private bool isSprinting = false;
    private CharacterController characterController;
    // Axis
    private const string horizontalAxis = "Horizontal";
    private const string verticalAxis = "Vertical";
    private float horizontalInput = 0f;
    private float verticalInput = 0f;

    [Header("Jumping")]
    public float JumpHeight = 4f;
    public float CoyoteJumpTime = 0.2f;
    public KeyCode JumpKeyCode = KeyCode.Space;
    private float groundedTimer = 0f;
    private float verticalVelocity = 0f;

    void Awake() {
        characterController = GetComponent<CharacterController>();
    }

    void Update() {
        // XZ movement
        horizontalInput = Input.GetAxis(horizontalAxis);
        verticalInput = Input.GetAxis(verticalAxis);

        if (Input.GetKey(RunKeyCode)) {
            if (!isSprinting) {
                currentSpeed = RunningSpeed;
                isSprinting = true;
            }
        } else if (isSprinting) {
            currentSpeed = WalkingSpeed;
            isSprinting = false;
        }
        deltaSpeed = currentSpeed * Time.deltaTime;

        // Y movement
        if (characterController.isGrounded) {
            groundedTimer = CoyoteJumpTime;
            if (verticalVelocity < 0f) verticalVelocity = 0f; // slam into ground
        }
        if (groundedTimer > 0f) {
            groundedTimer -= Time.deltaTime;
        }

        verticalVelocity += Gravity * Time.deltaTime;

        if (Input.GetKey(JumpKeyCode) && groundedTimer > 0f) {
            groundedTimer = 0f;
            verticalVelocity += Mathf.Sqrt(JumpHeight * -2f * Gravity);
        }
        // Final move
        characterController.Move(
            transform.TransformDirection(
                new Vector3(
                    horizontalInput * deltaSpeed,
                    verticalVelocity * Time.deltaTime,
                    verticalInput * deltaSpeed)));
    }
}
