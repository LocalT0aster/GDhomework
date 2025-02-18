using UnityEngine;

public class ProjectileShooter : MonoBehaviour {
    [SerializeField] private GameObject projectilePrefab;
    [SerializeField] private float projectileSpeed = 20f;
    [Tooltip("Optional")]
    [SerializeField] private Transform projectileSpawnPoint;

    private Camera cam;

    void Start() {
        cam = Camera.main;
        if (cam == null) {
            throw new MissingReferenceException("Main camera not found. Please tag your camera as 'MainCamera'");
        }

        // Check the existence of Rigidbody
        GameObject projectile = Instantiate(projectilePrefab, new(0f,-10000f,0f), Quaternion.identity);
        Rigidbody rb = projectile.GetComponent<Rigidbody>();
        if (rb == null) {
            throw new MissingComponentException("Projectile prefab does not have a Rigidbody component.");
        }
        Destroy(projectile);
    }

    void Update() {
        if (Input.GetMouseButtonDown(1)) {
            ShootProjectile();
        }
    }

    private void ShootProjectile() {
        if (projectilePrefab == null) {
            Debug.LogError("Projectile prefab is not assigned.");
            return;
        }

        Vector3 spawnPos = (projectileSpawnPoint != null)
            ? projectileSpawnPoint.position
            : cam.transform.position + cam.transform.forward;

        Instantiate(projectilePrefab, spawnPos, Quaternion.identity)
            .GetComponent<Rigidbody>()
            .velocity = cam.transform.forward * projectileSpeed;
    }
}
