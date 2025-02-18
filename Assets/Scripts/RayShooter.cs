using System.Collections;
using UnityEngine;

public class RayShooter : MonoBehaviour {
    private Camera cam;
    [SerializeField] private float sphereIndicatorDuration = 1f;

    void Start() {
        cam = Camera.main;
        if (cam == null) {
            Debug.LogError("Main camera not found. Please tag your camera as 'MainCamera'.");
        }
    }

    void Update() {
        if (Input.GetMouseButtonDown(0)) {
            ShootRay();
        }
    }

    private void ShootRay() {
        // Create a ray from the center of the screen
        Vector3 screenCenter = new Vector3(cam.pixelWidth / 2, cam.pixelHeight / 2, 0);
        Ray ray = cam.ScreenPointToRay(screenCenter);

        if (Physics.Raycast(ray, out RaycastHit hit)) {
            ReactiveTarget target = hit.transform.GetComponent<ReactiveTarget>();
            if (target != null) {
                target.ReactToHit();
            } else {
                StartCoroutine(ShowSphereIndicator(hit.point));
            }
        }
    }

    private IEnumerator ShowSphereIndicator(Vector3 position) {
        GameObject sphere = GameObject.CreatePrimitive(PrimitiveType.Sphere);
        sphere.transform.position = position;
        yield return new WaitForSeconds(sphereIndicatorDuration);
        Destroy(sphere);
    }

    void OnGUI() {
        int size = 12;
        float posX = (cam.pixelWidth - size) / 2;
        float posY = (cam.pixelHeight - size) / 2;
        GUI.Label(new Rect(posX, posY, size, size), "+");
    }
}
