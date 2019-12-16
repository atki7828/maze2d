using UnityEngine;
using System.Collections.Generic;
using System;

public class HexMaze : MonoBehaviour {
    PathFinder pf;
    public static List<GameObject> aiList;
    static HexMazeGenerator mg;
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
    int complexity = 1;     // number of times each cell
    int numEnemies = 0;
    int numTreasures = 6;

    void Start() {
        GameObject player = GameObject.FindWithTag("Player");

        if(mg == null)  mg = HexMazeGenerator.GetInstance();
        mg.init(W,H);
        mg.maxVisits = complexity;
        mg.BuildMaze(UnityEngine.Random.Range(1,W-1),UnityEngine.Random.Range(1,H-1));
        DrawMaze();

        player.transform.position = GetSpawnPosition();

        if(FullScreen) {
            Destroy(Camera.main.GetComponent<CameraController>());
            Camera.main.transform.position = new Vector3Int(W/2,H/2,-10);
            Camera.main.orthographicSize = (W > H ? H : W) / 2 + 1;
        }
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
        return new Vector3(UnityEngine.Random.Range(1,W-1),UnityEngine.Random.Range(1,H-1),0); 
    }

    void DrawMaze() {
        GameObject w;
        float cos45 = Mathf.Sqrt(2.0f)/2.0f;
        for(int i = 0; i < W; i++) {
            for(int j = 0; j < H; j++) {
                if(mg.grid[i,j].Walls["N"]) {
                    w = GameObject.Instantiate(wall,new Vector3(i,j+0.5f,0),Quaternion.identity,this.gameObject.transform);
                    w.name = "Walls";
                    w.transform.localScale = new Vector3(0.5f,1,1);
                }
                if(mg.grid[i,j].Walls["S"]) {
                    w = GameObject.Instantiate(wall,new Vector3(i,j-0.5f,0),Quaternion.identity,this.gameObject.transform);
                    w.name = "Walls";
                    w.transform.localScale = new Vector3(0.5f,1,1);
                }
                if(mg.grid[i,j].Walls["E"]) {
                    w = GameObject.Instantiate(wall,new Vector3(i+0.5f,j,0),Quaternion.Euler(0,0,90),this.gameObject.transform);
                    w.name = "Walls";
                    w.transform.localScale = new Vector3(0.5f,1,1);
                }
                if(mg.grid[i,j].Walls["W"]) {
                    w = GameObject.Instantiate(wall,new Vector3(i-0.5f,j,0),Quaternion.Euler(0,0,90),this.gameObject.transform);
                    w.name = "Walls";
                    w.transform.localScale = new Vector3(0.5f,1,1);
                }
                if(mg.grid[i,j].Walls["NW"]) {
                    w = GameObject.Instantiate(wall,new Vector3(i-cos45/2.0f,j+cos45/2.0f,0),Quaternion.Euler(0,0,45),this.gameObject.transform);
                    w.name = "Walls";
                    w.transform.localScale = new Vector3(0.5f,1,1);
                }
                if(mg.grid[i,j].Walls["NE"]) {
                    w = GameObject.Instantiate(wall,new Vector3(i+cos45/2.0f,j+cos45/2.0f,0),Quaternion.Euler(0,0,-45),this.gameObject.transform);
                    w.name = "Walls";
                    w.transform.localScale = new Vector3(0.5f,1,1);
                }
                if(mg.grid[i,j].Walls["SW"]) {
                    w = GameObject.Instantiate(wall,new Vector3(i-cos45/2.0f,j-cos45/2.0f,0),Quaternion.Euler(0,0,-45),this.gameObject.transform);
                    w.name = "Walls";
                    w.transform.localScale = new Vector3(0.5f,1,1);
                }
                if(mg.grid[i,j].Walls["SE"]) {
                    w = GameObject.Instantiate(wall,new Vector3(i+cos45/2.0f,j-cos45/2.0f,0),Quaternion.Euler(0,0,45),this.gameObject.transform);
                    w.name = "Walls";
                    w.transform.localScale = new Vector3(0.5f,1,1);
                }
            }
        }
    }

    int WallCount(Dictionary<String,bool> w) {
        int n = 0;
        if(w["N"])  n++;
        if(w["S"])  n++;
        if(w["E"])  n++;
        if(w["W"])  n++;
        return n;
    }

    public static HexCell GetCell(int x, int y) { return mg.grid[x,y]; }
}
