using UnityEngine;

// adapted from https://catlikecoding.com/unity/tutorials/noise/
//
// noise : https://github.com/keijiro/PerlinNoise/

public class background : MonoBehaviour {
    Texture2D txt;
    SpriteRenderer sr;
    [SerializeField]
    public static Vector2Int size = new Vector2Int(48,32);
    float freq = 32f;

    Vector3 pt00, pt01,pt10,pt11;
    float stepSize; 

    GameObject player;

    void Start() {
        pt00 = transform.TransformPoint(new Vector3(-0.5f,-0.5f));
        pt10 = transform.TransformPoint(new Vector3( 0.5f,-0.5f));
        pt01 = transform.TransformPoint(new Vector3(-0.5f, 0.5f));
        pt11 = transform.TransformPoint(new Vector3( 0.5f, 0.5f));
        stepSize = 1f / max(size.y,size.x);
        player = GameObject.FindWithTag("Player");
        CreateTexture();
        FillTexture();
        sr = GetComponent<SpriteRenderer>();
        //sr.sprite = Sprite.Create(txt, new Rect(0, 0, size.x,size.y), new Vector2(0, 0), max(txt.width,txt.height));
        sr.sprite = Sprite.Create(txt, new Rect(0, 0, txt.width,txt.height), new Vector2(0, 0), 1);
        this.transform.position = new Vector3(-1f,-1f,0);
    }

    float t = 0.0f;
    void Update() {
        FillTexture();
        t += Time.deltaTime/8f;
    }

    private void CreateTexture() {
        txt = new Texture2D(size.x,size.y,TextureFormat.RGB24,false);
        //txt.wrapMode = TextureWrapMode.Clamp;
        //txt.filterMode = FilterMode.Point;
        txt.filterMode = FilterMode.Trilinear;
    }

    private void FillTexture() {
        /*
           for (int y = 0; y < size.y; y++) {
           Vector3 pt0 = Vector3.Lerp(pt00, pt01, (y + 0.5f) * stepSize);
           Vector3 pt1 = Vector3.Lerp(pt10, pt11, (y + 0.5f) * stepSize);
           for (int x = 0; x < size.x; x++) {
           Vector3 pt = Vector3.Lerp(pt0, pt1, (x + 0.5f) * stepSize);
           txt.SetPixel(x, y, new Color(pt.x,pt.y,pt.z));
           }
           }
        /**/
        for(int i = 0; i < size.x; i++) {
            Vector3 pt0 = Vector3.Lerp(pt00,pt01,(i+0.5f)*stepSize);
            Vector3 pt1 = Vector3.Lerp(pt10,pt11,(i+0.5f)*stepSize);
            for(int j = 0; j < size.y; j++) {
                Vector3 pt = Vector3.Lerp(pt0,pt1,(j+0.5f)*stepSize);
                float n = Mathf.Abs(GetNoiseValue(pt));
                Color c = new Color(n,0.5f-n,1-n)/1;
                //txt.SetPixel(i,j,Color.blue*(0.5f-n));
                txt.SetPixel(i,j,c);
                //txt.SetPixel(j,i,Color.blue*(1f-Mathf.Abs(Perlin.Noise(i/(float)size.x,j/(float)size.y,t))));
                //txt.SetPixel(i,j,new Color(pt.x,pt.y,pt.z));
            }
        }
        txt.Apply();
    }

    private float GetNoiseValue(Vector3 pt) {
        pt = new Vector3(pt.x,pt.y,t);
        return Perlin.Fbm(pt,32);
    }

    int max(int a, int b) {
        return (a > b ? a : b);
    }
}
