using UnityEngine;

public class MouseLookX : MonoBehaviour {
    public float Sensitivity {
        get => sensitivity;
        set => sensitivity = value;
    }
    [Range(0.1f, 10f)][SerializeField] float sensitivity = 3f;
    private const string mouseXAxis = "Mouse X";
    private float rotation = 0f;

    void Awake() {
        rotation = transform.localRotation.eulerAngles.y;
    }

    private void Update() {
        rotation += Input.GetAxis(mouseXAxis) * sensitivity;
        Quaternion xQuat = Quaternion.AngleAxis(rotation, Vector3.up);
        transform.localRotation = xQuat;
    }
}
