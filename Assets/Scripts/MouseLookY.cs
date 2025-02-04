using UnityEngine;

public class MouseLookY : MonoBehaviour {
    public float Sensitivity {
        get => sensitivity;
        set => sensitivity = value;
    }
    [Range(0.1f, 10f)][SerializeField] float sensitivity = 3f;
    [Tooltip("Limits vertical camera rotation. Prevents the flipping that happens when rotation goes above 90.")]
    [Range(0f, 90f)][SerializeField] float yRotationLimit = 89.9f;


    private const string mouseYAxis = "Mouse Y";
    private float rotation = 0f;

    private void Awake() {
        rotation = transform.localRotation.eulerAngles.x;
    }
    private void Update() {
        rotation += Input.GetAxis(mouseYAxis) * sensitivity;
        rotation = Mathf.Clamp(rotation, -yRotationLimit, yRotationLimit);
        Quaternion yQuat = Quaternion.AngleAxis(rotation, Vector3.left);
        transform.localRotation = yQuat;
    }
}
