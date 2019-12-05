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
        pf = new PathFinder();
        Player = GameObject.FindWithTag("Player");
        mg = MazeGenerator.GetInstance();
        line = this.GetComponent<LineRenderer>();
        this.transform.position = mg.GetSpawnPosition();
        position = this.transform.position;
        //line.startColor = line.endColor = new Color(0,0,0,0);   // transparent.
    }

    void Update() {
        playerPos = new Vector3Int((int)Player.transform.position.x/2,(int)Player.transform.position.y/2,0);
        aiPos = new Vector3Int((int)this.transform.position.x/2,(int)this.transform.position.y/2,0);

        path = pf.GetPath(playerPos,aiPos);
        line.positionCount = 0;
        int i = 0;
        Node p = path;
        while(p != null) {
            line.positionCount++;
            line.SetPosition(i,new Vector3(p.x*2,p.y*2,0)+new Vector3(0.5f,0.5f,0));
            i++;
            p = p.parent;
        }
        if(transform.position == position && line.positionCount > 0) {
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
        for(int i = 0; i < line.positionCount-1; i++) {
            line.SetPosition(i,positions[i+1]);
        }
        line.positionCount--;
    }
}
