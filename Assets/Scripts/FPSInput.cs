using UnityEngine;

[RequireComponent(typeof(CharacterController))]
public class FPSInput : MonoBehaviour {
    public float Gravity = -9.80665f;
    [Header("Jumping")]
    public float JumpHeight = 4f;
    public float CoyoteJumpTime = 0.2f;
    public KeyCode JumpKeyCode = KeyCode.Space;
    private float groundedTimer = 0f;
    private float verticalVelocity = 0f;

    [Header("Movement Speeds")]
    public float WalkingSpeed = 5f;
    public float RunningSpeed = 10f;
    public float CrouchingSpeed = 2.5f;

    [Header("Inertia")]
    public float Acceleration = 10f;  // how fast to accelerate when there is input
    public float Deceleration = 10f;  // how fast to slow down when input is released

    [Header("Crouching")]
    public float StandingHeight = 2f;
    public float CrouchingHeight = 1f;
    public KeyCode CrouchKeyCode = KeyCode.LeftControl;
    public Transform PlayerModel;

    [Header("Running")]
    public KeyCode RunKeyCode = KeyCode.LeftShift;

    private float currentSpeed = 5f;
    private Vector3 velocity = Vector3.zero;
    private CharacterController characterController;

    private const string horizontalAxis = "Horizontal";
    private const string verticalAxis = "Vertical";

    void Awake() {
        characterController = GetComponent<CharacterController>();
    }

    void Update() {
        float horizontalInput = Input.GetAxis(horizontalAxis);
        float verticalInput = Input.GetAxis(verticalAxis);
        Vector3 inputDir = new Vector3(horizontalInput, 0, verticalInput);
        if (inputDir.magnitude > 1f) inputDir.Normalize();

        bool isCrouching = Input.GetKey(CrouchKeyCode);
        if (isCrouching) {
            currentSpeed = CrouchingSpeed;
            // Adjust the CharacterController height when crouching
            if (characterController.height != CrouchingHeight) {
                characterController.height = CrouchingHeight;
                if (PlayerModel)
                    PlayerModel.localScale = Vector3.Scale(PlayerModel.localScale, new(1f, CrouchingHeight / StandingHeight, 1f));
            }
        } else {

            if (characterController.height != StandingHeight) {
                characterController.height = StandingHeight;
                if (PlayerModel)
                    PlayerModel.localScale = Vector3.Scale(PlayerModel.localScale, new(1f, StandingHeight / CrouchingHeight, 1f));
            }

            if (Input.GetKey(RunKeyCode)) {
                currentSpeed = RunningSpeed;
            } else {
                currentSpeed = WalkingSpeed;
            }
        }

        Vector3 targetVelocity = inputDir * currentSpeed;
        Vector3 currentHorizontalVelocity = new Vector3(velocity.x, 0, velocity.z);

        float rate = (inputDir.sqrMagnitude > 0.01f) ? Acceleration : Deceleration;
        currentHorizontalVelocity = Vector3.MoveTowards(currentHorizontalVelocity, targetVelocity, rate * Time.deltaTime);
        velocity.x = currentHorizontalVelocity.x;
        velocity.z = currentHorizontalVelocity.z;

        if (characterController.isGrounded) {
            groundedTimer = CoyoteJumpTime;
            if (verticalVelocity < 0f)
                verticalVelocity = 0f;
        }
        if (groundedTimer > 0f) {
            groundedTimer -= Time.deltaTime;
        }
        verticalVelocity += Gravity * Time.deltaTime;

        if (Input.GetKey(JumpKeyCode) && groundedTimer > 0f) {
            groundedTimer = 0f;
            verticalVelocity = Mathf.Sqrt(JumpHeight * -2f * Gravity);
        }

        Vector3 finalMove = new Vector3(velocity.x, verticalVelocity, velocity.z);
        characterController.Move(transform.TransformDirection(finalMove) * Time.deltaTime);
    }
    public void Launch(Vector3 launchVelocity) {
        velocity.x = launchVelocity.x;
        velocity.z = launchVelocity.z;
        verticalVelocity = launchVelocity.y;
    }
}
