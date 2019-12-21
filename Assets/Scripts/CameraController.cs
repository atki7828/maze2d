using UnityEngine;

public class CameraController : MonoBehaviour {
    GameObject Player;
    Camera cam;
    float zOff;
    Vector2 MapSize;
    MazeGenerator mg;
    Vector2 CamSize;

    void Start() {
        Player = GameObject.FindWithTag("Player");
        cam = Camera.main;
        zOff = (Player.transform.position - cam.transform.position).z;
        mg = MazeGenerator.GetInstance();
        MapSize = mg.GetSize();
        CamSize = new Vector2(cam.orthographicSize*cam.aspect,cam.orthographicSize)-Vector2.one;
        Debug.Log(CamSize);

    }

    void Update() {
        this.transform.position = Player.transform.position - new Vector3(0,0,zOff);
        float y = Mathf.Clamp(this.transform.position.y,CamSize.y,MapSize.y-CamSize.y);
        float x = Mathf.Clamp(this.transform.position.x,CamSize.x,MapSize.x-CamSize.x);
        this.transform.position = new Vector3(x,y,this.transform.position.z) - Vector3.one/2f;
                
    }
}
