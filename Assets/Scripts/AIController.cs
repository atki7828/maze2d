using UnityEngine;

public class AIController : MonoBehaviour {

    float speed = 2.5f;
    PathFinder pf;
    Node path = null;
    static MazeGenerator mg;
    GameObject Player;
    Vector3Int playerPos, aiPos;
    LineRenderer line;
    Vector3 position;
    Vector3Int home;
    bool escape = false;

    void Start() {
        mg = MazeGenerator.GetInstance();
        this.transform.position = mg.GetSpawnPosition();
        pf = new PathFinder();
        pf.self = this.gameObject;
        Player = GameObject.FindWithTag("Player");
        line = this.GetComponent<LineRenderer>();
        line.startWidth = line.endWidth = 0.1f;
        position = this.transform.position;
        home = Vector3Int.FloorToInt(position);
        //line.startColor = line.endColor = new Color(0,0,0,0);   // transparent.
        bool escape;
    }

    void Update() {
        if(!escape) {
            playerPos = new Vector3Int((int)Player.transform.position.x,(int)Player.transform.position.y,0);
            aiPos = new Vector3Int((int)this.transform.position.x,(int)this.transform.position.y,0);

            Vector3 dir = (Player.transform.position - transform.position).normalized;

            path = pf.GetPath(playerPos,aiPos);
        }
        DrawLine();
        if(transform.position == position && path != null) {
            if(path.parent != null)
                position = new Vector3(path.parent.x,path.parent.y,0) + new Vector3(0.5f,0.5f,0);
            home = new Vector3Int(path.x,path.y,0);
            path = path.parent;
        }
        else if(path == null) {
            escape = false;
        }
        transform.position = Vector3.MoveTowards(transform.position,position,Time.deltaTime*speed);
        DrawLine();
    }

    void RemovePoint() {
        Vector3[] positions = new Vector3[line.positionCount];
        line.GetPositions(positions);
        line.positionCount--;
        line.SetPositions(positions);
        for(int i = 0; i < line.positionCount; i++) {
            line.SetPosition(i,positions[i+1]);
        }
    }

    void OnTriggerEnter2D(Collider2D other) {
        // this is probably no longer necessary,
        // since PathFinder now avoids other ai gameobjects.
        // leave it here for extra security tho.
        if(other.gameObject.name == "AI" && !escape) {
            //Debug.Log("ai hit");
            aiPos = new Vector3Int((int)this.transform.position.x,(int)this.transform.position.y,0);
            escape = true;
            path = pf.GetPath(home,aiPos);
        }
    }

    void DrawLine() {
        line.positionCount = 0;
        Node p = path;
        int i = 0;
        while(p != null) {
            line.positionCount++;
            line.SetPosition(i++,new Vector3(p.x,p.y,0)+new Vector3(0.5f,0.5f,0));
            p = p.parent;
        }
    }
}
