using UnityEngine;
using UnityEngine.SceneManagement;
using System.Linq;
using System;

public class PlayerController2 : MonoBehaviour {

    [SerializeField]
        float speed = 5;
    Rigidbody2D rb;

    void Start() {
        rb = GetComponent<Rigidbody2D>();
    }

    void FixedUpdate() {
        //float x = Input.GetAxisRaw("Horizontal");
        //float y = Input.GetAxisRaw("Vertical");
        //int x = 0;
        //int y = 0;
        /* only moves if current position is integer; moves along grid. */ 
        if(Input.GetKey(KeyCode.Escape))
            SceneManager.LoadScene("startmenu");
        if(Input.GetKey(KeyCode.W)) {
                rb.transform.position = Vector3.MoveTowards(rb.transform.position,rb.transform.position + Vector3.up,Time.deltaTime*speed);
        }
        else if(Input.GetKey(KeyCode.S)) {
                rb.transform.position = Vector3.MoveTowards(rb.transform.position,rb.transform.position - Vector3.up,Time.deltaTime*speed);
            //y = -1;
        }
        if(Input.GetKey(KeyCode.A)) {
                rb.transform.position = Vector3.MoveTowards(rb.transform.position,rb.transform.position - Vector3.right,Time.deltaTime*speed);
            //x = -1;
        }
        else if(Input.GetKey(KeyCode.D)) {
                rb.transform.position = Vector3.MoveTowards(rb.transform.position,rb.transform.position + Vector3.right,Time.deltaTime*speed);
            //x = 1;
        }

        //Vector2 pos = rb.position;
        //pos += new Vector2(x,y)*speed*Time.deltaTime;
        //transform.position += new Vector3(x,y,0) * speed * Time.deltaTime;
        //rb.MovePosition(Vector3.MoveTowards(transform.position,position,Time.deltaTime*speed));
        //rb.MovePosition(pos);

    }

    void OnTriggerEnter2D(Collider2D other) {
        if(other.gameObject.name == "AI") {
            SceneManager.LoadScene("startmenu");
        }
    }
}
