# Introduction to Game Development Homework 1

## by Danil Nesterov ([d.nesterov@innopolis.university](mailto:d.nesterov@innopolis.university))

The report source can be found at [GitHub Repo](https://github.com/LocalT0aster/GDhomework/blob/master/Reports/HW1.md).

### 1-7

![Screenshot](HW1.png)

### 7-8

```cs
using System.Collections;
using UnityEngine;

public class Player : MonoBehaviour {
    [Header("Timer")]
    [Tooltip("Damage and teleport every n seconds. Affected by Time.timeScale")]
    public float EveryNSeconds = 2f;
    private Coroutine timerCoroutine;
    private float stopwatch;

    [Header("Damage")]
    [SerializeField]
    private int health = 100;
    [Tooltip("Damage player in a range exluding the top value"), SerializeField]
    private Vector2Int damageRange = new(0, 10);
    private bool isAlive = true;

    [Header("Teleportation")]
    [SerializeField]
    private Vector3 minXYZ = new(-45f, 2f, -45f);
    [SerializeField]
    private Vector3 maxXYZ = new(45f, 4f, 45f);

    // Link Position to transform.position because
    // the GameObject already have dedicated field for this.
    [Unity.VisualScripting.DoNotSerialize]
    public Vector3 Position {
        get => transform.position;
        set => transform.position = value;
    }

    void Start() {
        timerCoroutine = StartCoroutine(RunEveryNSeconds());
    }

    IEnumerator RunEveryNSeconds() {
        while (true) {
            yield return new WaitForSeconds(EveryNSeconds);
            Position = RandomVector3(minXYZ, maxXYZ);
            updateHealth(Random.Range(damageRange.x, damageRange.y));
        }
    }

    private void updateHealth(int delta) {
        health -= delta;
        if (health < 0) {
            Debug.Log(string.Format(
            "Player Died after {0:#.000000} ingame seconds", stopwatch));
            isAlive = false;
            StopCoroutine(timerCoroutine);
        }
    }

    void Update() {
        if (isAlive) stopwatch += Time.deltaTime * Time.timeScale;
    }

    public static Vector3 RandomVector3(Vector3 min, Vector3 max) => new Vector3(
        Random.Range(min.x, max.x),
        Random.Range(min.y, max.y),
        Random.Range(min.z, max.z));
}

```
