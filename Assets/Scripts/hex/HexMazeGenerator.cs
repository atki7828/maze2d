using UnityEngine;
using System;
using System.Linq;
using System.Collections;
using System.Collections.Generic;

public class HexMazeGenerator {
    int W;
    int H;
    public Vector2Int GetSize() { return new Vector2Int(this.W,this.H); }
    public HexCell[,] grid;
    public int maxVisits = 1;
    static HexMazeGenerator mg;

    System.Random rand = new System.Random();

    public HexMazeGenerator(int W, int H) {
        mg = this;
        init(W,H);
    }

    HexMazeGenerator() { 
        mg = this;
    }

    public void init(int W, int H) {
        this.W = W;
        this.H = H;
        grid = new HexCell[W,H];
        float off = 0.0f;
        for(int i = 0; i < W; i++) {
            for(int j = 0; j < H; j++) {
                off = (float)j/2.0f;
                grid[i,j] = new HexCell(i-off,j);
                grid[i,j].visited = UnityEngine.Random.Range(0,maxVisits);
            }
        }
    }

    public static HexMazeGenerator GetInstance() {
        if(mg == null)  mg = new HexMazeGenerator(); 
        return mg;
    }

    public Vector3 GetSpawnPosition() {
        return new Vector3(UnityEngine.Random.Range(1,W-1),UnityEngine.Random.Range(1,H-1),0); 
    }

    public void TakeStep(Vector2Int start, Vector2Int end) {
        grid[start.x,start.y].neighbors.Add(grid[end.x,end.y]);
        grid[end.x,end.y].neighbors.Add(grid[start.x,start.y]);
        if(start.x == end.x) {  // north or south
            if(start.y < end.y) {   // north
                grid[start.x,start.y].Walls["N"] = false;
                grid[end.x,end.y].Walls["S"] = false;
            }
            else { // south 
                grid[start.x,start.y].Walls["S"] = false;
                grid[end.x,end.y].Walls["N"] = false;
            }
        }
        else if(start.y == end.y) { // east or west
            if(start.x < end.x) {   // east
                grid[start.x,start.y].Walls["E"] = false;
                grid[end.x,end.y].Walls["W"] = false;
            }
            else {  // west
                grid[start.x,start.y].Walls["W"] = false;
                grid[end.x,end.y].Walls["E"] = false;
            }
        }
        else {  //diagonal
            if(start.y < end.y) {   // north
                if(start.x < end.x) {   // east
                    grid[start.x,start.y].Walls["NE"] = false;
                    grid[end.x,end.y].Walls["SW"] = false;
                }
                else {  // west
                    grid[start.x,start.y].Walls["NW"] = false;
                    grid[end.x,end.y].Walls["SE"] = false;
                }

            }
            else {  // south
                if(start.x < end.x) {   // east
                    grid[start.x,start.y].Walls["SE"] = false;
                    grid[end.x,end.y].Walls["NW"] = false;
                }
                else {  // west
                    grid[start.x,start.y].Walls["SW"] = false;
                    grid[end.x,end.y].Walls["NE"] = false;
                }
            }

        }
    }

    public void BuildMaze(int x, int y) {
        grid[x,y].Visit();
        while(UnvisitedNeighbors(x,y) > 0) {
            foreach(int dir in Enumerable.Range(0,8).OrderBy(a => rand.Next())) {
                switch(dir) {
                    case 0:     //north
                        if(y < H-1 && grid[x,y+1].visited < maxVisits) {
                            TakeStep(new Vector2Int(x,y),new Vector2Int(x,y+1));
                            BuildMaze(x,y+1);
                        }
                        break;
                    case 1:     //south
                        if(y > 0 && grid[x,y-1].visited < maxVisits) {
                            TakeStep(new Vector2Int(x,y),new Vector2Int(x,y-1));
                            BuildMaze(x,y-1);
                        }
                        break;
                    case 2:     //east
                        if(x < W-1 && grid[x+1,y].visited < maxVisits) {
                            TakeStep(new Vector2Int(x,y),new Vector2Int(x+1,y));
                            BuildMaze(x+1,y);
                        }
                        break;
                    case 3:     //west
                        if(x > 0 && grid[x-1,y].visited < maxVisits) {
                            TakeStep(new Vector2Int(x,y),new Vector2Int(x-1,y));
                            BuildMaze(x-1,y);
                        }
                        break;
                    case 4:     //northwest
                        if(y < H-1 && x > 0 && grid[x-1,y+1].visited < maxVisits) {
                            TakeStep(new Vector2Int(x,y),new Vector2Int(x-1,y+1));
                            BuildMaze(x-1,y+1);
                        }
                        break;
                    case 5:     //northeast
                        if(y < H-1 && x < W-1 && grid[x+1,y+1].visited < maxVisits) {
                            TakeStep(new Vector2Int(x,y),new Vector2Int(x+1,y+1));
                            BuildMaze(x+1,y+1);
                        }
                        break;
                    case 6:     //southwest
                        if(y > 0 && x > 0 && grid[x-1,y-1].visited < maxVisits) {
                            TakeStep(new Vector2Int(x,y),new Vector2Int(x-1,y-1));
                            BuildMaze(x-1,y-1);
                        }
                        break;
                    case 7:     //southeast
                        if(y > 0 && x < W-1 && grid[x+1,y-1].visited < maxVisits) {
                            TakeStep(new Vector2Int(x,y),new Vector2Int(x+1,y-1));
                            BuildMaze(x+1,y-1);
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
                if((i !=x || j != y) && 
                        (i >= 0 && i < W && j >= 0 && j < H))
                {
                    count += grid[i,j].visited < maxVisits ? 1 : 0;
                }
            }
        }
        return count;
    }
    public HexCell GetCell(int x, int y) {
        return grid[x,y];
    }
}



public class HexCell {
    public float x, y;
    public int visited;
    public Dictionary<String,bool> Walls;
    public List<HexCell> neighbors;

    public HexCell(float x, float y) {
        this.x = x;
        this.y = y;
        visited = 0;
        Walls = new Dictionary<String,bool>();
        Walls["N"] = true;
        Walls["S"] = true;
        Walls["E"] = true;
        Walls["W"] = true;
        Walls["NE"] = true;
        Walls["NW"] = true;
        Walls["SE"] = true;
        Walls["SW"] = true;
        neighbors = new List<HexCell>();
    }

    public void Visit() { this.visited++; }

}
