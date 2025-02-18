using UnityEngine;

public class SceneController : MonoBehaviour {
    public static SceneController Instance { get; private set; }

    [SerializeField] private GameObject enemyPrefab;
    [SerializeField] private Transform[] enemySpawnPoints;

    void Awake() {
        if (Instance == null) {
            Instance = this;
        } else {
            Destroy(gameObject);
        }
    }

    public void SpawnEnemy() {
        if (enemyPrefab == null) {
            Debug.LogError("Enemy prefab not assigned in SceneController.");
            return;
        }

        Vector3 spawnPosition = Vector3.zero;
        // Use a spawn point if available. Otherwise choose a random position.
        if (enemySpawnPoints != null && enemySpawnPoints.Length > 0) {
            int index = Random.Range(0, enemySpawnPoints.Length);
            spawnPosition = enemySpawnPoints[index].position;
        } else {
            spawnPosition = new Vector3(Random.Range(-10f, 10f), 0, Random.Range(-10f, 10f));
        }

        Instantiate(enemyPrefab, spawnPosition, Quaternion.identity);
    }
}
