using UnityEngine;

public class CameraController : MonoBehaviour {
    GameObject Player;
    Camera cam;
    float zOff;

    void Start() {
        Player = GameObject.FindWithTag("Player");
        cam = Camera.main;
        zOff = (Player.transform.position - cam.transform.position).z;

    }

    void Update() {
        this.transform.position = Player.transform.position - new Vector3(0,0,zOff);
    }
}
