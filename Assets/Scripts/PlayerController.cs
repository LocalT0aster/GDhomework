using UniGLTF;
using UnityEngine;

public class PlayerController : MonoBehaviour {
    public const string CameraName = "Camera";
    private Transform cameraTransform;
    public float Sensitivity {
        get { return sensitivity; }
        set { sensitivity = value; }
    }
    [Range(0.1f, 10f)][SerializeField] float sensitivity = 2f;
    [Tooltip("Limits vertical camera rotation. Prevents the flipping that happens when rotation goes above 90.")]
    [Range(0f, 90f)][SerializeField] float yRotationLimit = 89f;

    public const float Gravity = -9.80665f;
    public float MovementSpeed = 0.5f;
    private CharacterController characterController;

    private float speed = 0f;
    private float horizontalInput = 0f;
    private float verticalInput = 0f;
    private Vector2 rotation = Vector2.zero;
    private const string horizontalAxis = "Horizontal";
    private const string verticalAxis = "Vertical";
    private const string mouseXAxis = "Mouse X";
    private const string mouseYAxis = "Mouse Y";

    private void Start() {
        characterController = GetComponent<CharacterController>();
        cameraTransform = getChildCamera(gameObject);
        Cursor.lockState = CursorLockMode.Locked;
        // Cursor.visible = false;
    }
    Transform getChildCamera(GameObject go) {
        return go.transform.GetChildByName(CameraName).transform;
    }

    void Update() {
        horizontalInput = Input.GetAxis(horizontalAxis);
        verticalInput = Input.GetAxis(verticalAxis);
        rotation.x += Input.GetAxis(mouseXAxis) * sensitivity;
        rotation.y += Input.GetAxis(mouseYAxis) * sensitivity;
        rotation.y = Mathf.Clamp(rotation.y, -yRotationLimit, yRotationLimit);
        Quaternion xQuat = Quaternion.AngleAxis(rotation.x, Vector3.up);
        Quaternion yQuat = Quaternion.AngleAxis(rotation.y, Vector3.left);
        transform.localRotation = xQuat;
        cameraTransform.localRotation = yQuat;

        speed = MovementSpeed * Time.deltaTime;
        characterController.Move(
            transform.TransformDirection(
                new Vector3(
                    horizontalInput * speed,
                    Gravity * Time.deltaTime,
                    verticalInput * speed)));
    }
}
