using UnityEngine;
using System.Collections.Generic;

// Binary Space Partioning procedural map generator
//

public class BSP {
    int W = 24;
    int H = 16;
    public Vector2Int GetSize() { return new Vector2Int(this.W,this.H); }
    List<Room> Rooms;
    public List<Room> GetRooms() { return this.Rooms; }
    int MinRoomArea = 4;

    public BSP() {
        Rooms = new List<Room>();
        Room r = new Room(0,0,W,H);
        Rooms.Add(r);
        BuildRoom(r,0);
        //BufferRooms();
    }

    void BufferRooms() {
        List<Room> newRooms = new List<Room>();
        foreach(Room r in Rooms) {
            Room r2 = r;
            r2.x += Random.Range(r2.width/4,r2.width/2);
            r2.width -= Random.Range(r2.width/4,r2.width/2);
            r2.y += Random.Range(r2.height/4,r2.height/2);
            r2.height -= Random.Range(r2.height/4,r2.height/2);
            newRooms.Add(r2);
        }
        this.Rooms.Clear();
        this.Rooms.AddRange(newRooms);
    }
    void BuildRoom(Room r, int depth) {
        if((r.width) * (r.height) <= MinRoomArea )  return;
        //Debug.Log(r.width*r.height);
        Room r1, r2;
        if(r.width > r.height) {
            int rWidth = r.width/2+Random.Range(-r.width/8,r.width/8);
            r1 = new Room(r.x,r.y,rWidth,r.height);
            r2 = new Room(r.x+rWidth,r.y,r.width-rWidth,r.height);
            int d = Random.Range(r.y+r.height/4,r.y+r.height-r.height/4);
            r1.doors.Add(new Vector3Int(r.x+rWidth,d,0));
            r2.doors.Add(new Vector3Int(r.x+rWidth,d,0));
        }
        else {
            int rHeight = r.height/2+Random.Range(-r.height/8,r.height/8);
            r1 = new Room(r.x,r.y,r.width,rHeight);
            r2 = new Room(r.x,r.y+rHeight,r.width,r.height-rHeight);
            int d = Random.Range(r.x+r.width/4,r.x+r.width-r.width/4);
            r1.doors.Add(new Vector3Int(d,r.y+rHeight,0));
            r2.doors.Add(new Vector3Int(d,r.y+rHeight,0));
        }
        r1.doors.AddRange(r.doors);
        r2.doors.AddRange(r.doors);
        Rooms.Remove(r);
        Rooms.Add(r1);
        Rooms.Add(r2);
        BuildRoom(r1,depth+1);
        BuildRoom(r2,depth+1);
    }


    
}

public class Room {
    public int x, y, width, height;
    public List<Vector3Int> doors;

    public Room(int x, int y, int width, int height) {
        doors = new List<Vector3Int>();
        this.x = x;
        this.y = y;
        this.width = width;
        this.height = height;
    }
    
}
