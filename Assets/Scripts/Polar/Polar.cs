using UnityEngine;
using System.Collections.Generic;
using System.Linq;

public class Polar : MonoBehaviour {
    [SerializeField]
    int W,H;
    [SerializeField]
    int maxRad = 15;
    public static float dRad, dTheta;
    [SerializeField]
    GameObject wall;
    [SerializeField]
    GameObject pix;
    public PolarCell[,] grid;
    public List<PolarCell> cells;

    System.Random rand;

    [SerializeField]
    int maxVisits = 1;

    void Start() {
        rand = new System.Random();
        //Camera.main.orthographicSize = maxRad+2;
        dRad = (float)maxRad/H;
        dTheta = 2*Mathf.PI/W;
        cells = new List<PolarCell>();

        grid = new PolarCell[W,H];
        for(int i = 0; i < W; i++) {
            for(int j = 0; j < H; j++) {
                grid[i,j] = new PolarCell(i,j);
                cells.Add(grid[i,j]);
            }
        }
        BuildMaze(grid[UnityEngine.Random.Range(0,W-1),UnityEngine.Random.Range(0,H-1)]);
        for(int i = 0; i < W; i++)  grid[i,0].Walls['S'] = false;
        foreach(PolarCell c in grid) {
            DrawCell(c);
        }
    }

    void BuildMaze(PolarCell c) {
        c.Visit();
        int x = c.x;
        int y = c.y;
        int x2;
        while(UnvisitedNeighbors(x,y) > 0) {
            foreach(int dir in Enumerable.Range(0,4).OrderBy(a => rand.Next())) {
                switch(dir) {
                    case 0: // north
                        if(y < H-1 && grid[x,y+1].visited < maxVisits) {
                            grid[x,y].neighbors.Add(grid[x,y+1]);
                            grid[x,y+1].neighbors.Add(grid[x,y]);
                            grid[x,y].Walls['N'] = false;
                            grid[x,y+1].Walls['S'] = false;
                            BuildMaze(grid[x,y+1]);
                        }
                        break;
                    case 1: // south
                        if(y > 0 && grid[x,y-1].visited < maxVisits) {
                            grid[x,y].neighbors.Add(grid[x,y-1]);
                            grid[x,y-1].neighbors.Add(grid[x,y]);
                            grid[x,y].Walls['S'] = false;
                            grid[x,y-1].Walls['N'] = false;
                            BuildMaze(grid[x,y-1]);
                        }
                        break;
                    case 2: // east
                        if(x < W-1) x2 = x+1;
                        else    x2 = 0;
                        if(grid[x2,y].visited < maxVisits) {
                            grid[x,y].neighbors.Add(grid[x2,y]);
                            grid[x2,y].neighbors.Add(grid[x,y]);
                            grid[x,y].Walls['W'] = false;
                            grid[x2,y].Walls['E'] = false;
                            BuildMaze(grid[x2,y]);
                        }
                        break;
                    case 3: // west
                        if(x > 0)   x2 = x-1;
                        else    x2 = W-1;
                        if(grid[x2,y].visited < maxVisits) {
                            grid[x,y].neighbors.Add(grid[x2,y]);
                            grid[x2,y].neighbors.Add(grid[x,y]);
                            grid[x,y].Walls['E'] = false;
                            grid[x2,y].Walls['W'] = false;
                            BuildMaze(grid[x2,y]);
                        }
                        break;
                    default:
                        break;
                }
            }
        }

    }

    int UnvisitedNeighbors(int x, int y) {
        int count = 0;
        for(int i = x-1; i <= x+1; i++) {
            for(int j = y-1; j <= y+1; j++) {
                if((i != x || j != y) &&
                        (i == x || j == y) &&
                        (i >= 0 && i < W && j >= 0 && j < H)) {
                    count += grid[i,j].visited < maxVisits ? 1 : 0;
                }
            }
        }
        return count;
    }

    void DrawCell(PolarCell c) {
        float r = c.r;
        float theta = c.theta;
        GameObject cell = new GameObject(c.x.ToString() + "," + c.y.ToString() + "(n:" + c.Walls['N'].ToString() + ",s:" + c.Walls['S'].ToString() + ",e:" + c.Walls['E'].ToString() + ",w:" + c.Walls['W'].ToString() + ")");

        GameObject n,s,e,w;
        float eWallTheta = theta - dTheta/2;
        float wWallTheta = theta + dTheta/2;
        float sWallRad = r-dRad/2;
        float nWallRad = r+dRad/2;

        Vector2 s1 = new Vector2(sWallRad*Mathf.Cos(eWallTheta),sWallRad*Mathf.Sin(eWallTheta));
        Vector2 s2 = new Vector2(sWallRad*Mathf.Cos(wWallTheta),sWallRad*Mathf.Sin(wWallTheta));
        Vector2 n1 = new Vector2(nWallRad*Mathf.Cos(eWallTheta),nWallRad*Mathf.Sin(eWallTheta));
        Vector2 n2 = new Vector2(nWallRad*Mathf.Cos(wWallTheta),nWallRad*Mathf.Sin(wWallTheta));
        float sWallLen = dist(s1,s2);
        float nWallLen = dist(n1,n2);

        if(c.Walls['E']) {
            e = GameObject.Instantiate(wall,new Vector3(r*Mathf.Cos(eWallTheta),r*Mathf.Sin(eWallTheta),0),Quaternion.Euler(0,0,(eWallTheta)*Mathf.Rad2Deg),cell.transform);
            e.transform.localScale = new Vector3(dRad,1,1);
            e.name = "e";
        }

        if(c.Walls['W']) {
            w = GameObject.Instantiate(wall,new Vector3(r*Mathf.Cos(wWallTheta),r*Mathf.Sin(wWallTheta),0),Quaternion.Euler(0,0,(wWallTheta)*Mathf.Rad2Deg),cell.transform);
            w.transform.localScale = new Vector3(dRad,1,1);
            w.name = "w";
        }


        if(c.Walls['S']) {
            s = new GameObject("s");
            s.transform.parent = cell.transform;
            for(float i = eWallTheta; i <= wWallTheta; i += 0.01f) {
                GameObject.Instantiate(pix,new Vector3(sWallRad*Mathf.Cos(i),sWallRad*Mathf.Sin(i),0),Quaternion.identity,s.transform);
            }
        }

        if(c.Walls['N']) {
            n = new GameObject("n");
            n.transform.parent = cell.transform;
            for(float i = eWallTheta; i <= wWallTheta; i += 0.01f) {
                GameObject.Instantiate(pix,new Vector3(nWallRad*Mathf.Cos(i),nWallRad*Mathf.Sin(i),0),Quaternion.identity,n.transform);
            }
        }

    }

    float dist(Vector2 a, Vector2 b) {
        float dx = Mathf.Abs(a.x-b.x);
        float dy = Mathf.Abs(a.y-b.y);
        float d = Mathf.Sqrt(Mathf.Pow(dx,2)+Mathf.Pow(dy,2));
        return d;
    }
}

public class PolarCell { 
    public float r, theta;
    public int x,y;
    public int visited;
    public Dictionary<char,bool> Walls;
    public List<PolarCell> neighbors;
    public PolarCell(int x, int y) {
        this.x = x;
        this.y = y;
        this.r = (y+1)*Polar.dRad;
        this.theta = x*Polar.dTheta;
        this.visited = 0;
        Walls = new Dictionary<char,bool>();
        Walls['N'] = true;
        Walls['E'] = true;
        Walls['S'] = true;
        Walls['W'] = true;
        this.neighbors = new List<PolarCell>();
    }
    public void Visit() {this.visited++; }

}
