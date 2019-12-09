using UnityEngine;
using UnityEngine.SceneManagement;
using System.Linq;
using System;

public class PlayerController : MonoBehaviour {

    float speed = 5;
    Rigidbody2D rb;
    public Vector3 position;

    void Start() {
        System.Random rand = new System.Random();
        rb = GetComponent<Rigidbody2D>();
        position = transform.position;
    }

    void FixedUpdate() {
        //float x = Input.GetAxisRaw("Horizontal");
        //float y = Input.GetAxisRaw("Vertical");
        //int x = 0;
        //int y = 0;
        /* only moves if current position is integer; moves along grid. */ 
        if(Input.GetKey(KeyCode.W) && transform.position == position) {
            if(Maze.WallMap.GetTile(Vector3Int.FloorToInt(position+Vector3.up)) == null) {
                position += Vector3.up;
            }
        }
        else if(Input.GetKey(KeyCode.S) && transform.position == position) {
            if(Maze.WallMap.GetTile(Vector3Int.FloorToInt(position-Vector3.up)) == null) {
                position -= Vector3.up;
            }
            //y = -1;
        }
        if(Input.GetKey(KeyCode.A) && transform.position == position) {
            if(Maze.WallMap.GetTile(Vector3Int.FloorToInt(position-Vector3.right)) == null) {
                position -= Vector3.right;
            }
            //x = -1;
        }
        else if(Input.GetKey(KeyCode.D) && transform.position == position) {
            if(Maze.WallMap.GetTile(Vector3Int.FloorToInt(position+Vector3.right)) == null) {
                position += Vector3.right;
            }
            //x = 1;
        }

        //Vector2 pos = rb.position;
        //pos += new Vector2(x,y)*speed*Time.deltaTime;
        //transform.position += new Vector3(x,y,0) * speed * Time.deltaTime;
        transform.position = Vector3.MoveTowards(transform.position,position,Time.deltaTime*speed);
        //rb.MovePosition(Vector3.MoveTowards(transform.position,position,Time.deltaTime*speed));
        //rb.MovePosition(pos);


    }

    void OnTriggerEnter2D(Collider2D other) {
        if(other.gameObject.name == "AI") {
            SceneManager.LoadScene("startmenu");
        }
    }
}
