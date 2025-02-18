using System.Collections;
using UnityEngine;

public class Fireball : MonoBehaviour {
    public float Speed = 10.0f;
    public int Damage = 1;
    public float DestroyTimeout = 10f;

    void Awake() {
        StartCoroutine(TimeoutCoroutine());
    }

    IEnumerator TimeoutCoroutine() {
        yield return new WaitForSeconds(DestroyTimeout);
        Destroy(gameObject);
    }

    //void Update() {
    //    transform.Translate(Vector3.forward * Speed * Time.deltaTime);
    //}

    private void OnCollisionEnter(Collision collision) {
        PlayerCharacter player = collision.gameObject.GetComponent<PlayerCharacter>();
        if (player != null) {
            player.Hurt(Damage);
        }
        Destroy(gameObject);
    }

    void OnTriggerEnter(Collider other) {
        PlayerCharacter player = other.GetComponent<PlayerCharacter>();
        if (player != null) {
            player.Hurt(Damage);
        }
        Destroy(gameObject);
    }
}
