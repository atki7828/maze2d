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

    void Start() {
        mg = MazeGenerator.GetInstance();
        this.transform.position = mg.GetSpawnPosition();
        pf = new PathFinder();
        Player = GameObject.FindWithTag("Player");
        line = this.GetComponent<LineRenderer>();
        position = this.transform.position;
        line.startColor = line.endColor = new Color(0,0,0,0);   // transparent.
    }

    void Update() {
        playerPos = new Vector3Int((int)Player.transform.position.x/2,(int)Player.transform.position.y/2,0);
        aiPos = new Vector3Int((int)this.transform.position.x/2,(int)this.transform.position.y/2,0);

        Vector3 dir = (Player.transform.position - transform.position).normalized;

        //Debug.DrawRay(transform.position+dir,dir,Color.green,1.0f);
        //if(Physics2D.Raycast(transform.position+dir,dir).collider.gameObject == Player) {
            //Debug.Log(Physics2D.Raycast(transform.position+dir,dir).collider.gameObject);
            path = pf.GetPath(playerPos,aiPos);
        //}
        /*
        else {
            Vector3 target = mg.GetSpawnPosition();
            path = pf.GetPath(new Vector3Int((int)target.x/2,(int)target.y/2,0),aiPos);
        }
        */
        line.positionCount = 0;
        int i = 0;
        Node p = path;
        while(p != null) {
            line.positionCount++;
            line.SetPosition(i,new Vector3(p.x*2,p.y*2,0)+new Vector3(0.5f,0.5f,0));
            i++;
            p = p.parent;
        }
        if(transform.position == position && line.positionCount > 1) {
            position = line.GetPosition(1);
            RemovePoint();
        }
        if((transform.position - Player.transform.position).magnitude > 1) {
            transform.position = Vector3.MoveTowards(transform.position,position,Time.deltaTime*speed);
        }
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
}
