using UnityEngine;

public class dot : MonoBehaviour {
    GameObject Player;

    void Start() {
        Player = GameObject.FindWithTag("Player");
    }

    void OnTriggerEnter2D(Collider2D other) {
        if(other.gameObject == Player) Destroy(this.gameObject);
    }
}
