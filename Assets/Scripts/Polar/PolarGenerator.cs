using UnityEngine;
using System;
using System.Collections.Generic;

public class PolarGenerator {
    int W;
    int H;
    public Vector2Int GetSize() { return new Vector2Int(this.W,this.H); }
    public PolarCell[,] grid;
    public int maxVisits = 1;
    static PolarGenerator pg;

    System.Random rand = new System.Random();

    public PolarGenerator(int W, int H) {
        pg = this;
        init(W,H);
    }

    PolarGenerator() { pg = this; }

    public void init(int W, int H) {
        this.W = W;
        this.H = H;
        grid = new PolarCell[W,H];
        for(int i = 0; i < W; i++) {
            for(int j = 0; j < H; j++) {
                grid[i,j] = new PolarCell(i,j);
            }
        }
    }
    public static PolarGenerator GetInstance() {
        if(pg == null)  pg = new PolarGenerator();
        return pg;
    }
}
