using UnityEngine;
using System.Collections.Generic;

public class Maze : MonoBehaviour {
    PathFinder pf;
    public static List<GameObject> aiList;
    static MazeGenerator mg;
    [SerializeField]
        int W = 12;
    [SerializeField]
        int H = 8;
    [SerializeField]
        GameObject ai;
    [SerializeField]
        GameObject treasure;
    [SerializeField]
        GameObject wall;
    [SerializeField]
        GameObject dot;
    GameObject d;
    [SerializeField]
        bool FullScreen;
    [SerializeField]
        GameObject bg;
    [SerializeField]
        int complexity = 1;     // number of times each cell
    int numEnemies = 0;
    int numTreasures = 6;
    GameObject player;

    void Awake() {
        player = GameObject.FindWithTag("Player");
        if(mg == null)  mg = MazeGenerator.GetInstance();
        mg.init(W,H);
        mg.maxVisits = complexity;
        background.size = new Vector2Int(W,H)+Vector2Int.one;
    }

    void Start() {
        mg.BuildMaze(Random.Range(1,W-1),Random.Range(1,H-1));
        DrawMaze();

        player.transform.position = GetSpawnPosition();
        player.GetComponent<PlayerController>().position = player.transform.position;

        if(FullScreen) {
            Destroy(Camera.main.GetComponent<CameraController>());
            Camera.main.transform.position = new Vector3Int(W/2,H/2,-10);
            Camera.main.orthographicSize = (W > H ? H : W) / 2f + 1;
        }
        else {
            Camera.main.orthographicSize = (W > H ? H : W) / 5f + 1;
        }
        pf = new PathFinder();
        aiList = new List<GameObject>();
        for(int i = 0; i < numEnemies; i++) {
            GameObject e = GameObject.Instantiate(ai);
            e.transform.position = mg.GetSpawnPosition();
            aiList.Add(e);
            e.name = "AI";
        }
        for(int i = 0; i < numTreasures; i++) {
            GameObject g = GameObject.Instantiate(treasure);
            g.name = "Treasure";
        }
    }

    Vector3 GetSpawnPosition() { 
        return new Vector3(Random.Range(1,W-1),Random.Range(1,H-1),0); 
    }

void DrawMaze() {
    GameObject w;
    for(int i = 0; i < W; i++) {
        for(int j = 0; j < H; j++) {
            if(mg.grid[i,j].Walls['N']) {
                w = GameObject.Instantiate(wall,new Vector3(i,j+0.5f,0),Quaternion.identity,this.transform);
                w.name = "Walls";
            }
            if(mg.grid[i,j].Walls['S']) {
                w = GameObject.Instantiate(wall,new Vector3(i,j-0.5f,0),Quaternion.identity,this.transform);
                w.name = "Walls";
            }
            if(mg.grid[i,j].Walls['E']) {
                w = GameObject.Instantiate(wall,new Vector3(i+0.5f,j,0),Quaternion.Euler(0,0,90),this.transform);
                w.name = "Walls";
            }
            if(mg.grid[i,j].Walls['W']) {
                w = GameObject.Instantiate(wall,new Vector3(i-0.5f,j,0),Quaternion.Euler(0,0,90),this.transform);
                w.name = "Walls";
            }
        }
    }
}

    int WallCount(Dictionary<char,bool> w) {
        int n = 0;
        if(w['N'])  n++;
        if(w['S'])  n++;
        if(w['E'])  n++;
        if(w['W'])  n++;
        return n;
    }

    public static Cell GetCell(int x, int y) { return mg.grid[x,y]; }
}
