using UnityEngine;
using UnityEngine.Tilemaps;

public class Maze : MonoBehaviour {
    PathFinder pf;
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
    Tile WallTile,FloorTile;
    [SerializeField]
        Tile nWall, nwWall, nsWall,wneWall, neswWall;
    [SerializeField]
        GameObject dot;
    GameObject d;
    public static Tilemap WallMap,FloorMap;
    [SerializeField]
        bool FullScreen;
    [SerializeField]
        int complexity = 1;     // number of times each cell
                            //  is visited during generation
    int numEnemies = 4;
    int numTreasures = 6;

    void Start() {
        GameObject player = GameObject.FindWithTag("Player");
        WallMap = this.transform.Find("Walls").gameObject.GetComponent<Tilemap>();
        WallTile = Tile.CreateInstance<Tile>();
        WallTile.sprite = WallSprite;
        FloorMap = this.transform.Find("Floor").gameObject.GetComponent<Tilemap>();
        FloorTile = Tile.CreateInstance<Tile>();
        FloorTile.sprite = FloorSprite;

        if(mg == null)  mg = MazeGenerator.GetInstance();
        mg.init(W,H);
        mg.maxVisits = complexity;
        mg.BuildMaze(Random.Range(1,W-1),Random.Range(1,H-1));
        DrawMaze();

        player.transform.position = GetSpawnPosition();
        player.GetComponent<PlayerController>().position = player.transform.position;

        if(FullScreen) {
            Destroy(Camera.main.GetComponent<CameraController>());
            Camera.main.transform.position = new Vector3Int(W,H,-10);
            Camera.main.orthographicSize = (W > H ? H : W) + 2;
        }
        pf = new PathFinder();
        for(int i = 0; i < numEnemies; i++) {
            GameObject.Instantiate(ai);
        }
        for(int i = 0; i < numTreasures; i++) {
            GameObject g = GameObject.Instantiate(treasure);
            g.name = "Treasure";
        }
    }

    Vector3 GetSpawnPosition() { 
        return new Vector3(Random.Range(1,W-1)*2,Random.Range(1,H-1)*2,0) + new Vector3(0.5f,0.5f,0); 
    }

    void DrawMaze() {
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
                }
                if(mg.grid[i,j].Walls['S'] == true) {
                    WallMap.SetTile(new Vector3Int(x,y-1,0),WallTile);
                }
                else {
                    FloorMap.SetTile(new Vector3Int(x,y-1,0),FloorTile);
                    d = GameObject.Instantiate(dot);
                    d.transform.position = new Vector3Int(x,y-1,0)+new Vector3(0.5f,0.5f,0);
                }
                if(mg.grid[i,j].Walls['E'] == true) {
                    WallMap.SetTile(new Vector3Int(x+1,y,0),WallTile);
                }
                else {
                    FloorMap.SetTile(new Vector3Int(x+1,y,0),FloorTile);
                    d = GameObject.Instantiate(dot);
                    d.transform.position = new Vector3Int(x+1,y,0)+new Vector3(0.5f,0.5f,0);
                }
                if(mg.grid[i,j].Walls['W'] == true) {
                    WallMap.SetTile(new Vector3Int(x-1,y,0),WallTile);
                }
                else {
                    FloorMap.SetTile(new Vector3Int(x-1,y,0),FloorTile);
                    d = GameObject.Instantiate(dot);
                    d.transform.position = new Vector3Int(x-1,y,0)+new Vector3(0.5f,0.5f,0);
                }
                WallMap.SetTile(new Vector3Int(x+1,y+1,0),WallTile);
                WallMap.SetTile(new Vector3Int(x-1,y-1,0),WallTile);
                WallMap.SetTile(new Vector3Int(x-1,y+1,0),WallTile);
                WallMap.SetTile(new Vector3Int(x+1,y-1,0),WallTile);
                FloorMap.SetTile(new Vector3Int(x,y,0),FloorTile);
                d = GameObject.Instantiate(dot);
                d.transform.position = new Vector3Int(x,y,0)+new Vector3(0.5f,0.5f,0);
            }
        }
    }
    /*

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
Debug.Log(numWalls);
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
