using System.Collections;
using UnityEngine;

public class ReactiveTarget : MonoBehaviour {
    private bool isDying = false;
    [SerializeField] private float deathDelay = 1.5f;
    [SerializeField] private float rotationAngleOnHit = -75f;

    public void ReactToHit() {
        if (!isDying) {
            StartCoroutine(Die());
        }
    }

    private IEnumerator Die() {
        isDying = true;

        if (GetComponent<WanderingAI>()) {
            GetComponent<WanderingAI>().enabled = false;
        }
        transform.Rotate(rotationAngleOnHit, 0, 0);
        yield return new WaitForSeconds(deathDelay);

        if (SceneController.Instance != null) {
            SceneController.Instance.SpawnEnemy();
        }
        Destroy(gameObject);
    }
}
