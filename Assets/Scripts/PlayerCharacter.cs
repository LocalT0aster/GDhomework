using UnityEngine;

[RequireComponent(typeof(ReactiveTarget))]
public class PlayerCharacter : MonoBehaviour {
    [SerializeField] private int maxHealth = 5;
    private int currentHealth;

    void Start() {
        currentHealth = maxHealth;
    }

    public void Hurt(int damage) {
        if (currentHealth > 0) {
            currentHealth -= damage;
            Debug.Log(string.Format("\"{0}\"'s Health: {1}", gameObject.name, currentHealth));
            if (currentHealth <= 0) {
                Debug.Log(string.Format("\"{0}\" is dead!", gameObject.name));
                GetComponent<ReactiveTarget>().ReactToHit();
            }
        }
    }
}
