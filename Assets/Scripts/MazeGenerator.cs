using UnityEngine;
using System;
using System.Linq;
using System.Collections;
using System.Collections.Generic;

public class MazeGenerator {
    public int W;
    public int H;
    public Cell[,] grid;
    public int maxVisits = 1;
    System.Random rand;
    static MazeGenerator mg;

    public MazeGenerator(int W, int H) {
        mg = this;
        init(W,H);
    }

    MazeGenerator() { 
        mg = this;
    }

    public void init(int W, int H) {
        rand = new System.Random();
        this.W = W;
        this.H = H;
        grid = new Cell[W,H];
        for(int i = 0; i < W; i++) {
            for(int j = 0; j < H; j++) {
                grid[i,j] = new Cell(i,j);
            }
        }
    }

    public static MazeGenerator GetInstance() {
        if(mg == null)  mg = new MazeGenerator(); 
        return mg;
    }

    public Vector3 GetSpawnPosition() {
        return new Vector3(UnityEngine.Random.Range(1,W-1)*2,UnityEngine.Random.Range(1,H-1)*2,0) + new Vector3(0.5f,0.5f,0);
    }

    public void BuildMaze(int x, int y) {
        grid[x,y].Visit();
        while(UnvisitedNeighbors(x,y) > 0) {
            foreach(int dir in Enumerable.Range(0,4).OrderBy(k => rand.Next())) {
                switch(dir%4) {
                    case 0:     // north
                        if(y < H-1 && grid[x,y+1].visited < maxVisits) {
                            grid[x,y].neighbors.Add(grid[x,y+1]);
                            grid[x,y].Walls['N'] = false;
                            grid[x,y+1].Walls['S'] = false;
                            BuildMaze(x,y+1);
                        }
                        break;
                    case 1:     // south
                        if(y > 0 && grid[x,y-1].visited < maxVisits) {
                            grid[x,y].neighbors.Add(grid[x,y-1]);
                            grid[x,y].Walls['S'] = false;
                            grid[x,y-1].Walls['N'] = false;
                            BuildMaze(x,y-1);
                        }
                        break;
                    case 2:     // west
                        if(x > 0 && grid[x-1,y].visited < maxVisits) {
                            grid[x,y].neighbors.Add(grid[x-1,y]);
                            grid[x,y].Walls['W'] = false;
                            grid[x-1,y].Walls['E'] = false;
                            BuildMaze(x-1,y);
                        }
                        break;
                    case 3:     // east
                        if(x < W-1 && grid[x+1,y].visited < maxVisits) {
                            grid[x,y].neighbors.Add(grid[x+1,y]);
                            grid[x,y].Walls['E'] = false;
                            grid[x+1,y].Walls['W'] = false;
                            BuildMaze(x+1,y);
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
                        (i == x || j == y) && 
                        (i >= 0 && i < W && j >= 0 && j < H))
                {
                    count += grid[i,j].visited < maxVisits ? 1 : 0;
                }
            }
        }
        return count;
    }
    public Cell GetCell(int x, int y) {
        return grid[x,y];
    }
}



public class Cell {
    public float f;
    public float g;
    public float h;
    public int x, y;
    public int visited;
    public Dictionary<char,bool> Walls;
    public List<Cell> neighbors;
    public Cell parent;

    public Cell(int x, int y) {
        this.x = x;
        this.y = y;
        visited = 0;
        Walls = new Dictionary<char,bool>();
        Walls['N'] = true;
        Walls['E'] = true;
        Walls['S'] = true;
        Walls['W'] = true;
        neighbors = new List<Cell>();
    }

    public void Visit() { this.visited++; }

    public void Reset() {
        f = 0;
        g = 0;
        h = 0;
        parent = null;
    }
}
