using UnityEngine;

public class WanderingAI : MonoBehaviour {
    public float speed = 3.0f;
    public float obstacleRange = 5.0f;
    public float rotationSpeed = 90f;

    private bool isRotating = false;
    private Quaternion targetRotation;

    void Start() {
        // Initialize the target rotation to the current rotation
        targetRotation = transform.rotation;
    }

    void Update() {
        transform.Translate(Vector3.forward * speed * Time.deltaTime);

        if (isRotating) {
            // Gradually rotate towards the target rotation
            transform.rotation = Quaternion.RotateTowards(transform.rotation, targetRotation, rotationSpeed * Time.deltaTime);
            if (Quaternion.Angle(transform.rotation, targetRotation) < 1f) {
                isRotating = false;
            }
        } else {
            Ray ray = new Ray(transform.position, transform.forward);
            if (Physics.SphereCast(ray, 0.75f, out RaycastHit hit)) {
                if (hit.distance < obstacleRange) {
                    float randomAngle = Random.Range(-110f, 110f);
                    targetRotation = Quaternion.Euler(0, transform.eulerAngles.y + randomAngle, 0);
                    isRotating = true;
                }
            }
        }
    }
}
