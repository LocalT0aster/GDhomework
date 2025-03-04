using UnityEngine;

public class LaunchPad : MonoBehaviour {
    public float launchForce = 20f;

    // Ensure the launchpad's collider is set as Trigger.
    private void OnTriggerEnter(Collider other) {
        Vector3 launchDir = transform.up;
        launchDir.Normalize();

        Rigidbody rb = other.GetComponent<Rigidbody>();
        if (rb != null) {
            rb.velocity += launchDir * launchForce;
        } else {
            FPSInput fpsInput = other.GetComponent<FPSInput>();
            if (fpsInput != null) {
                fpsInput.Launch(launchDir * launchForce);
            }
        }
    }
}
