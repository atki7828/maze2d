using UnityEngine;
using System.IO;
using System;
using System.Collections;
using System.Collections.Generic;

public class PathFinder {

    Cell[,] grid;
    Node[,] nodes;
    static MazeGenerator mg;
    Node current;
    HashSet<Node> OpenSet, ClosedSet;
    public GameObject self;
    Vector2Int size;

    public PathFinder() {
        mg = MazeGenerator.GetInstance();
        grid = mg.grid;
        //init();
        OpenSet = new HashSet<Node>();
        ClosedSet = new HashSet<Node>();
    }

    void init() {
        size = mg.GetSize();
        int w = size.x;
        int h = size.y;
        nodes = new Node[w,h];
        for(int i = 0; i < w; i++) {
            for(int j = 0; j < h; j++) {
                nodes[i,j] = new Node(i,j);
            }
        }
        foreach(Node n in nodes) {
            n.neighbors = new List<Node>();
            foreach(Cell c in grid[n.x,n.y].neighbors) {
                n.neighbors.Add(nodes[c.x,c.y]);
            }
        }
    }

    public Node GetPath(Vector3Int Start, Vector3Int End) {
        if(nodes == null)   init();
        if(mg.grid == null)    return null;
        foreach(Node c in nodes) if(c != null)  c.Reset();
        OpenSet.Clear();
        ClosedSet.Clear();

        if(Start.x < 0 || Start.x > size.x-1 || Start.y < 0 || Start.y > size.y-1) {
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
