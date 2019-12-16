using UnityEngine;
using System.Collections.Generic;

public class RoomBuilder : MonoBehaviour {
    BSP bsp;
    List<Room> rooms; 
    [SerializeField]
        GameObject wall;

    void Start() {
        rooms = new List<Room>();
        bsp = new BSP();
        rooms = bsp.GetRooms();
        foreach(Room r in rooms) {
        }
        DrawRooms();
    }

    void DrawRooms() {
        GameObject ro = new GameObject("rooms");
        foreach(Room r in rooms) {
            GameObject room = new GameObject("room");
            room.transform.parent = ro.transform;
            for(int i = r.x; i < r.x+r.width; i++) {
                if(!r.doors.Contains(new Vector3Int(i,r.y,0))) {
                    GameObject w = GameObject.Instantiate(wall,new Vector3(i,r.y,0)+new Vector3(0.5f,0,0),Quaternion.identity,room.transform);
                    w.name = "Walls";
                }
            }
            for(int i = r.y; i < r.y+r.height; i++) {
                if(!r.doors.Contains(new Vector3Int(r.x,i,0))) {
                    GameObject w = GameObject.Instantiate(wall,new Vector3(r.x,i,0)+new Vector3(0,0.5f,0),Quaternion.Euler(0,0,90),room.transform);
                    w.name = "Walls";
                }
            }
            for(int i = r.x; i < r.x+r.width; i++) {
                if(!r.doors.Contains(new Vector3Int(i,r.y+r.height,0))) {
                    GameObject w = GameObject.Instantiate(wall,new Vector3(i,r.y+r.height,0)+new Vector3(0.5f,0,0),Quaternion.identity,room.transform);
                    w.name = "Walls";
                }
            }
            for(int i = r.y; i < r.y+r.height; i++) {
                if(!r.doors.Contains(new Vector3Int(r.x+r.width,i,0))) {
                    GameObject w = GameObject.Instantiate(wall,new Vector3(r.x+r.width,i,0)+new Vector3(0,0.5f,0),Quaternion.Euler(0,0,90),room.transform);
                    w.name = "Walls";
                }
            }
        }
    }
}
