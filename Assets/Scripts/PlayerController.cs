using UnityEngine;
using UnityEngine.SceneManagement;
using System.Linq;
using System;

public class PlayerController : MonoBehaviour {

    float speed = 5;
    Rigidbody2D rb;
    public Vector3 position, prevPosition;

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
            RaycastHit2D hit = Physics2D.Linecast(position,position+Vector3.up);
            if(hit.collider == null || hit.collider.gameObject.name != "Walls") {
                prevPosition = position;
                position += Vector3.up;
            }
        }
        else if(Input.GetKey(KeyCode.S) && transform.position == position) {
            RaycastHit2D hit = Physics2D.Linecast(position,position-Vector3.up);
            if(hit.collider == null || hit.collider.gameObject.name != "Walls") {
                prevPosition = position;
                position -= Vector3.up;
            }
            //y = -1;
        }
        if(Input.GetKey(KeyCode.A) && transform.position == position) {
            RaycastHit2D hit = Physics2D.Linecast(position,position-Vector3.right);
            if(hit.collider == null || hit.collider.gameObject.name != "Walls") {
                prevPosition = position;
                position -= Vector3.right;
            }
            //x = -1;
        }
        else if(Input.GetKey(KeyCode.D) && transform.position == position) {
            RaycastHit2D hit = Physics2D.Linecast(position,position+Vector3.right);
            if(hit.collider == null || hit.collider.gameObject.name != "Walls") {
                prevPosition = position;
                position += Vector3.right;
            }
            //x = 1;
        }

        //Vector2 pos = rb.position;
        //pos += new Vector2(x,y)*speed*Time.deltaTime;
        //transform.position += new Vector3(x,y,0) * speed * Time.deltaTime;
        rb.transform.position = Vector3.MoveTowards(rb.transform.position,position,Time.deltaTime*speed);
        //rb.MovePosition(Vector3.MoveTowards(transform.position,position,Time.deltaTime*speed));
        //rb.MovePosition(pos);

    }

    void OnTriggerEnter2D(Collider2D other) {
        if(other.gameObject.name == "Walls")    position = prevPosition;
        if(other.gameObject.name == "AI") {
            SceneManager.LoadScene("startmenu");
        }
    }
}
