using System.Collections;
using UnityEngine;

public class RespawnTheFallen : MonoBehaviour {
    public float CheckEveryNSeconds = 1f;
    public float YLevel = -10f;
    public Vector3 RespawnLocation = new(0f, 1f, 0f);
    private Coroutine timerCoroutine;
    void Awake() {
        timerCoroutine = StartCoroutine(RunEveryNSeconds());
    }

    IEnumerator RunEveryNSeconds() {
        while (true) {
            yield return new WaitForSeconds(CheckEveryNSeconds);
            if (transform.position.y < YLevel) {
                Debug.Log(string.Format("\"{0}\" fell off the map, teleporting to {1}.", gameObject.name, RespawnLocation.ToString()));
                transform.position = RespawnLocation;
            }
        }
    }
}
