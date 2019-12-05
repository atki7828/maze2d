using UnityEngine;
using System;
using System.Collections;
using System.Collections.Generic;

public class PathFinder {

    Cell[,] grid;
    Node[,] nodes;
    static MazeGenerator mg;
    Node current;
    HashSet<Node> OpenSet, ClosedSet;

    public PathFinder() {
        mg = MazeGenerator.GetInstance();
        grid = mg.grid;
        //init();
        OpenSet = new HashSet<Node>();
        ClosedSet = new HashSet<Node>();
    }

    void init() {
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
    }

    public Node GetPath(Vector3Int Start, Vector3Int End) {
        if(nodes == null)   init();
        if(mg.grid == null)    return null;
        foreach(Node c in nodes) c.Reset();
        OpenSet.Clear();
        ClosedSet.Clear();

        if(Start.x < 0 || Start.x > mg.W-1 || Start.y < 0 || Start.y > mg.H-1) {
            //return null;
        }

        OpenSet.Add(nodes[Start.x,Start.y]);

        while(OpenSet.Count > 0) {
            current = null;
            foreach(Node c in OpenSet) {
                if(current == null || c.f < current.f) {
                    current = c;
                }
            }
            OpenSet.Remove(current);
            ClosedSet.Add(current);

            if(current == nodes[End.x,End.y]) {
                return current;
            }

            foreach(Node c in current.neighbors) {
                if(!ClosedSet.Contains(c)) {
                    float TempG = current.g+1;
                    if(c.x != current.x && c.y != current.y) {
                        TempG += 1;
                    }
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
