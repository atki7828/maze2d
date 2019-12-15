using UnityEngine;
using System.Collections.Generic;
using UnityEngine.Tilemaps;

public class Maze : MonoBehaviour {
    PathFinder pf;
    public static List<GameObject> aiList;
    static MazeGenerator mg;
    [SerializeField]
        int W = 12;
    [SerializeField]
        int H = 8;
    [SerializeField]
        Sprite WallSprite;
    [SerializeField]
        Sprite FloorSprite;
    [SerializeField]
        GameObject ai;
    [SerializeField]
        GameObject treasure;
    [SerializeField]
        Tile FloorTile,WallTile;
    [SerializeField]
        Tile nWall, nwWall, nsWall, nwsWall, nwseWall;
    [SerializeField]
        GameObject dot;
    GameObject d;
    public static Tilemap WallMap,FloorMap;
    [SerializeField]
        bool FullScreen;
    [SerializeField]
        int complexity = 1;     // number of times each cell
                            //  is visited during generation
    int numEnemies = 2;
    int numTreasures = 6;

    void Start() {
        GameObject player = GameObject.FindWithTag("Player");
        WallMap = this.transform.Find("Walls").gameObject.GetComponent<Tilemap>();
        FloorMap = this.transform.Find("Floor").gameObject.GetComponent<Tilemap>();
        //FloorTile = Tile.CreateInstance<Tile>();
        //FloorTile.sprite = FloorSprite;

        if(mg == null)  mg = MazeGenerator.GetInstance();
        mg.init(W,H);
        mg.maxVisits = complexity;
        mg.BuildMaze(Random.Range(1,W-1),Random.Range(1,H-1));
        DrawMaze();

        player.transform.position = GetSpawnPosition();
        player.GetComponent<PlayerController>().position = player.transform.position;

        if(FullScreen) {
            Destroy(Camera.main.GetComponent<CameraController>());
            Camera.main.transform.position = new Vector3Int(W/2,H/2,-10);
            Camera.main.orthographicSize = (W > H ? H : W) / 2 + 1;
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
        return new Vector3(Random.Range(1,W-1),Random.Range(1,H-1),0) + new Vector3(0.5f,0.5f,0); 
    }


    void DrawMaze() {
        for(int i = 0; i < W; i++) {
            for(int j = 0; j < H; j++) {
                Dictionary<char,bool> w = mg.grid[i,j].Walls;
                int numWalls = WallCount(w);
                if(numWalls == 0) {
                }
                if(numWalls == 1) {
                    WallMap.SetTile(new Vector3Int(i,j,0),nWall);
                    if(w['W']) {
                        WallMap.SetTransformMatrix(new Vector3Int(i,j,0),Matrix4x4.TRS(Vector3.zero,Quaternion.Euler(0f,0f,90f),Vector3.one));
                    }
                    else if(w['S']) {
                        WallMap.SetTransformMatrix(new Vector3Int(i,j,0),Matrix4x4.TRS(Vector3.zero,Quaternion.Euler(0f,0f,180f),Vector3.one));
                    }
                    else if(w['E']) {
                        WallMap.SetTransformMatrix(new Vector3Int(i,j,0),Matrix4x4.TRS(Vector3.zero,Quaternion.Euler(0f,0f,-90f),Vector3.one));
                    }
                }
                if(numWalls == 2) {
                    if((w['N'] && w['S']) 
                            || (w['E'] && w['W'])) {
                        WallMap.SetTile(new Vector3Int(i,j,0),nsWall);
                        if(w['E']) {
                            WallMap.SetTransformMatrix(new Vector3Int(i,j,0),Matrix4x4.TRS(Vector3.zero,Quaternion.Euler(0f,0f,90f),Vector3.one));
                        }
                    }
                    else if((w['N'] && w['W']) 
                            || (w['N'] && w['E']) 
                            || (w['S'] && w['W']) 
                            || (w['S'] && w['E'])) {
                        WallMap.SetTile(new Vector3Int(i,j,0),nwWall);
                        if(w['N']) {
                            if(w['E']) {
                                WallMap.SetTransformMatrix(new Vector3Int(i,j,0),Matrix4x4.TRS(Vector3.zero,Quaternion.Euler(0f,0f,-90f),Vector3.one));
                            }
                        }
                        else if(w['S']) {
                            if(w['E']) {
                                WallMap.SetTransformMatrix(new Vector3Int(i,j,0),Matrix4x4.TRS(Vector3.zero,Quaternion.Euler(0f,0f,180f),Vector3.one));
                            }
                            else {
                                WallMap.SetTransformMatrix(new Vector3Int(i,j,0),Matrix4x4.TRS(Vector3.zero,Quaternion.Euler(0f,0f,90f),Vector3.one));
                            }
                        }
                    }
                }
                if(numWalls == 3) {
                    WallMap.SetTile(new Vector3Int(i,j,0),nwsWall);
                    if(!w['N']) {
                        WallMap.SetTransformMatrix(new Vector3Int(i,j,0),Matrix4x4.TRS(Vector3.zero,Quaternion.Euler(0f,0f,90f),Vector3.one));
                    }
                    else if(!w['W']) {
                        WallMap.SetTransformMatrix(new Vector3Int(i,j,0),Matrix4x4.TRS(Vector3.zero,Quaternion.Euler(0f,0f,180f),Vector3.one));
                    }
                    else if(!w['S']) {
                        WallMap.SetTransformMatrix(new Vector3Int(i,j,0),Matrix4x4.TRS(Vector3.zero,Quaternion.Euler(0f,0f,-90f),Vector3.one));
                    }
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

    /*
    void DrawMaze() {
        GameObject dots = new GameObject("dots");
        for(int i = 0; i < W; i++) { 
            for(int j = 0; j < H; j++) {
                int x = i*2;
                int y = j*2;
                if(mg.grid[i,j].Walls['N'] == true) {
                    WallMap.SetTile(new Vector3Int(x,y+1,0),WallTile);
                }
                else {
                    FloorMap.SetTile(new Vector3Int(x,y+1,0),FloorTile);
                    d = GameObject.Instantiate(dot);
                    d.transform.position = new Vector3Int(x,y+1,0)+new Vector3(0.5f,0.5f,0);
                    d.transform.parent = dots.transform;
                }
                if(mg.grid[i,j].Walls['S'] == true) {
                    WallMap.SetTile(new Vector3Int(x,y-1,0),WallTile);
                }
                else {
                    FloorMap.SetTile(new Vector3Int(x,y-1,0),FloorTile);
                    d = GameObject.Instantiate(dot);
                    d.transform.position = new Vector3Int(x,y-1,0)+new Vector3(0.5f,0.5f,0);
                    d.transform.parent = dots.transform;
                }
                if(mg.grid[i,j].Walls['E'] == true) {
                    WallMap.SetTile(new Vector3Int(x+1,y,0),WallTile);
                }
                else {
                    FloorMap.SetTile(new Vector3Int(x+1,y,0),FloorTile);
                    d = GameObject.Instantiate(dot);
                    d.transform.position = new Vector3Int(x+1,y,0)+new Vector3(0.5f,0.5f,0);
                    d.transform.parent = dots.transform;
                }
                if(mg.grid[i,j].Walls['W'] == true) {
                    WallMap.SetTile(new Vector3Int(x-1,y,0),WallTile);
                }
                else {
                    FloorMap.SetTile(new Vector3Int(x-1,y,0),FloorTile);
                    d = GameObject.Instantiate(dot);
                    d.transform.position = new Vector3Int(x-1,y,0)+new Vector3(0.5f,0.5f,0);
                    d.transform.parent = dots.transform;
                }
                WallMap.SetTile(new Vector3Int(x+1,y+1,0),WallTile);
                WallMap.SetTile(new Vector3Int(x-1,y-1,0),WallTile);
                WallMap.SetTile(new Vector3Int(x-1,y+1,0),WallTile);
                WallMap.SetTile(new Vector3Int(x+1,y-1,0),WallTile);
                FloorMap.SetTile(new Vector3Int(x,y,0),FloorTile);
                d = GameObject.Instantiate(dot);
                d.transform.position = new Vector3Int(x,y,0)+new Vector3(0.5f,0.5f,0);
                d.transform.parent = dots.transform;
            }
        }
    }

    void DrawMaze() {
        for(int i = 0; i < W; i++) {
            for(int j = 0; j < H; j++) {
                int numWalls = 0;
                Tile t;
                if(mg.grid[i,j].Walls['N'] == true) {
                    numWalls++;
                }
                if(mg.grid[i,j].Walls['E'] == true) {
                    numWalls++;
                }
                if(mg.grid[i,j].Walls['S'] == true) {
                    numWalls++;
                }
                if(mg.grid[i,j].Walls['W'] == true) {
                    numWalls++;
                }
                if(numWalls == 1) {
                    WallMap.SetTile(new Vector3Int(i,j,0),nWall);
                }
                else if(numWalls == 2) {
                }
                else if(numWalls == 3) {
                    WallMap.SetTile(new Vector3Int(i,j,0),wneWall);
                }
                else if(numWalls == 4) {
                    WallMap.SetTile(new Vector3Int(i,j,0),neswWall);
                }
            }
        }
        WallMap.SetTile(new Vector3Int(0,0,0),neswWall);
    }
    */

    public static Cell GetCell(int x, int y) { return mg.grid[x,y]; }
}
