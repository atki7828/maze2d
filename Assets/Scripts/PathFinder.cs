using UnityEngine;
using System.IO;
using UnityEngine.Tilemaps;
using System;
using System.Collections;
using System.Collections.Generic;

public class PathFinder {

    Cell[,] grid;
    Tilemap maze;
    Node[,] nodes;
    static MazeGenerator mg;
    Node current;
    HashSet<Node> OpenSet, ClosedSet;
    public GameObject self;

    public PathFinder() {
        maze = Maze.FloorMap;
        mg = MazeGenerator.GetInstance();
        grid = mg.grid;
        //init();
        OpenSet = new HashSet<Node>();
        ClosedSet = new HashSet<Node>();
    }

    void init() {
        int w = mg.W*2;
        int h = mg.H*2;
        nodes = new Node[w,h];
        for(int i = 0; i < w; i++) {
            for(int j = 0; j < h; j++) {
                if(maze.GetTile(new Vector3Int(i,j,0)) != null) {
                    nodes[i,j] = new Node(i,j);
                }
                else {
                    nodes[i,j] = null;
                }
            }
        }
        foreach(Node n in nodes) {
            if(n != null) {
                n.neighbors = new List<Node>();
                for(int i = n.x-1; i <= n.x+1; i++) {
                    for(int j = n.y-1; j <= n.y+1; j++) {
                        if(i >= 0 && j >= 0 && i < w && j < h && (i != n.x || j != n.y) && (i == n.x || j == n.y)) {
                            if(nodes[i,j] != null) {
                                n.neighbors.Add(nodes[i,j]);
                            }
                        }
                    }
                }
            }
        }
        /*
           nodes = new Node[mg.W,mg.H];
           for(int i = 0; i < mg.W; i++) {
           for(int j = 0; j < mg.H; j++) {
           nodes[i,j] = new Node(i,j);
           }
           }
           for(int i = 0; i < mg.W; i++) {
           for(int j = 0; j < mg.H; j++) {
           nodes[i,j].neighbors = new List<Node>();
           foreach(Cell c in mg.grid[i,j].neighbors) {
           nodes[i,j].neighbors.Add(nodes[c.x,c.y]);
           }
           }
           }
           */
    }

    public Node GetPath(Vector3Int Start, Vector3Int End) {
        if(nodes == null)   init();
        if(mg.grid == null)    return null;
        foreach(Node c in nodes) if(c != null)  c.Reset();
        OpenSet.Clear();
        ClosedSet.Clear();

        if(Start.x < 0 || Start.x > mg.W-1 || Start.y < 0 || Start.y > mg.H-1) {
            //return null;
        }

        OpenSet.Add(nodes[Start.x,Start.y]);
        //Debug.Log(nodes[Start.x,Start.y]);
        //Debug.Log("start: " + Start.x.ToString() + ',' + Start.y.ToString());
        //Debug.Log("end: " + End.x.ToString() + ',' + End.y.ToString());

        while(OpenSet.Count > 0) {
            current = null;
            foreach(Node c in OpenSet) {
                if(current == null || c.f < current.f) {
                    current = c;
                }
            }
            //Debug.Log("current: " + current.x.ToString() + ',' + current.y.ToString());
            OpenSet.Remove(current);
            ClosedSet.Add(current);

            if(current == nodes[End.x,End.y]) {
                //Debug.Log("path found");
                //Debug.Log(current);
                return current;
            }

            foreach(Node c in current.neighbors) {
                //Debug.Log("checking neighbor " + c.x.ToString() + ',' + c.y.ToString());
                bool clear = true;
                foreach(GameObject e in Maze.aiList) {
                    if(new Vector3Int((int)e.transform.position.x,(int)e.transform.position.y,0) == new Vector3Int(c.x,c.y,0) && e.transform.position != self.transform.position) {
                        clear = false;
                    }
                }
                if(clear) {
                    if(!ClosedSet.Contains(c)) {
                        float TempG = current.g+1;
                        if(OpenSet.Contains(c)) {
                            if(TempG < c.g) {
                                c.g = TempG;
                                c.parent = current;
                            }
                        }
                        else {
                            c.g = TempG;
                            c.parent = current;
                            OpenSet.Add(c);
                        }

                        c.h = dist(c,nodes[End.x,End.y]);
                        c.f = c.g + c.h;

                    }
                }
            }
        }
        return null;
    }

    float dist(Node a, Node b) {
        float dx = Math.Abs(a.x - b.x);
        float dy = Math.Abs(a.y - b.y);
        //return Mathf.Sqrt(Mathf.Pow(dx,2) + Mathf.Pow(dy,2));
        return dx + dy;
    }
}

public class Node {
    public float f,g,h;
    public int x,y;
    public Node parent;
    public List<Node> neighbors;

    public Node(int x, int y) {
        this.x = x;
        this.y = y;
    }
    public void Reset() {
        f = 0;
        g = 0;
        h = 0;
        parent = null;
    }
}
