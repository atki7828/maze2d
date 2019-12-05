using UnityEngine;
using System.Collections.Generic;

public class Treasure : MonoBehaviour {

    [SerializeField]
        List<Sprite> TreasureSprites;
    MazeGenerator mg;
    GameObject Player;

    void Start() {
        Player = GameObject.FindWithTag("Player");
        mg = MazeGenerator.GetInstance();
        this.GetComponent<SpriteRenderer>().sprite = GetRandomSprite();
        this.transform.position = mg.GetSpawnPosition();
    }

    void Update() {
    }

    Sprite GetRandomSprite() { return TreasureSprites[Random.Range(0,TreasureSprites.Count)]; }

    void OnTriggerEnter2D(Collider2D other) {
        if(other.gameObject == Player) {
            Destroy(this.gameObject);
        }
    }
}
