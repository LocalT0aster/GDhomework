using UnityEngine;

// The script is designed for cube inside the empty parent.
public class DVDLogo : MonoBehaviour {
    public Vector3 velocity = new Vector3(1f, 1f, 1f);
    public Vector3 Boundary = new Vector3(5f, 5f, 5f);
    public float MaxSpeed = 5f;
    [Tooltip("If default, calulates offset based on the primitive cube local scale.")]
    public Vector3 ShapeOffset = Vector3.zero;
    private new Renderer renderer;

    void Awake() {
        renderer = GetComponent<Renderer>();
        // Calculate offset based on the localScale.
        // This assumes the mesh is a unit cube (1 unit per side). If your mesh has a different size,
        // you might need to adjust the ShapeOffset accordingly.
        if (ShapeOffset == Vector3.zero)
            ShapeOffset = new(transform.localScale.x * 0.5f,
                              transform.localScale.y * 0.5f,
                              transform.localScale.z * 0.5f);
        RandomColor();
    }

    void Update() {
        Vector3 newPos = transform.localPosition + velocity * Time.deltaTime;

        // Check and handle bounce for the x-axis.
        if (newPos.x > (Boundary.x - ShapeOffset.x)) {
            newPos.x = Boundary.x - ShapeOffset.x;
            velocity.x = -1f * Mathf.Sign(velocity.x) * Random.Range(1f, MaxSpeed);
            RandomColor();
        } else if (newPos.x < -(Boundary.x - ShapeOffset.x)) {
            newPos.x = -Boundary.x + ShapeOffset.x;
            velocity.x = -1f * Mathf.Sign(velocity.x) * Random.Range(1f, MaxSpeed);
            RandomColor();
        }

        // Check and handle bounce for the y-axis.
        if (newPos.y > (Boundary.y - ShapeOffset.y)) {
            newPos.y = Boundary.y - ShapeOffset.y;
            velocity.y = -1f * Mathf.Sign(velocity.y) * Random.Range(1f, MaxSpeed);
            RandomColor();
        } else if (newPos.y < -(Boundary.y - ShapeOffset.y)) {
            newPos.y = -Boundary.y + ShapeOffset.y;
            velocity.y = -1f * Mathf.Sign(velocity.y) * Random.Range(1f, MaxSpeed);
            RandomColor();
        }

        // Check and handle bounce for the z-axis.
        if (newPos.z > (Boundary.z - ShapeOffset.z)) {
            newPos.z = Boundary.z - ShapeOffset.z;
            velocity.z = -1f * Mathf.Sign(velocity.z) * Random.Range(1f, MaxSpeed);
            RandomColor();
        } else if (newPos.z < -(Boundary.z - ShapeOffset.z)) {
            newPos.z = -Boundary.z + ShapeOffset.z;
            velocity.z = -1f * Mathf.Sign(velocity.z) * Random.Range(1f, MaxSpeed);
            RandomColor();
        }

        transform.localPosition = newPos;
    }

    void RandomColor() {
        // Random hue between 0 and 1, with full saturation and brightness.
        Color newColor = Color.HSVToRGB(Random.value, 1f, 1f);
        if (renderer != null) {
            renderer.material.color = newColor;
        }
    }
}
