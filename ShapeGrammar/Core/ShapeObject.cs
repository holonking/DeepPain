using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using SGGeometry;

public class ShapeObject : MonoBehaviour {
    public Vector3 Size
    {
        get
        {
            return transform.localScale;
        }
        set
        {
            transform.localScale = value;
        }
    }
    public Vector3[] Vects
    {
        get
        {
            Vector3 z = transform.forward;
            Vector3 y = transform.up;
            Vector3 x = Vector3.Cross(y,z);
            return new Vector3[] { x, y, z };
        }
    }
    public Meshable meshable;

    private MeshFilter meshFilter;
    private MeshRenderer meshRenderer;
    // Use this for initialization
    void Start () {
		
	}
	
	// Update is called once per frame
	void Update () {
		
	}

    private void OnRenderObject()
    {
        GLDrawScope();
    }
    Vector3[] makeBoxPoints()
    {
        Vector3[] opts = new Vector3[8];
        Vector3[] vects = Vects;
        Vector3 size = Size;

        foreach(Vector3 v in vects)
        {
            print(v);
        }

        opts[0] = new Vector3(-0.5f,-0.5f,-0.5f);
        opts[1] = opts[0] + (vects[0]);
        opts[2] = opts[1] + (vects[2]);
        opts[3] = opts[0]+ (vects[2]);
        for( int i = 0; i < 4; i++)
        {
            opts[i + 4] = opts[i] + vects[1];
        }
        return opts;

    }
    void GLDrawScope()
    {
        Color color = Color.black;
        Material mat = UserStats.LineMat;
        mat.SetPass(0);
        Vector3[] pts = makeBoxPoints();
        GL.PushMatrix();
        if (transform != null)
        {
            GL.MultMatrix(transform.localToWorldMatrix);
        }
        GL.Begin(GL.LINES);
        GL.Color(Color.red);
        GL.Vertex(pts[0]);
        GL.Vertex(pts[1]);
        GL.Color(Color.blue);
        GL.Vertex(pts[0]);
        GL.Vertex(pts[3]);
        GL.Color(Color.green);
        GL.Vertex(pts[0]);
        GL.Vertex(pts[4]);


        GL.Vertex(pts[1]);
        GL.Vertex(pts[2]);
        GL.Vertex(pts[2]);
        GL.Vertex(pts[3]);
        GL.Vertex(pts[3]);
        GL.Vertex(pts[0]);

        GL.Vertex(pts[4]);
        GL.Vertex(pts[5]);
        GL.Vertex(pts[5]);
        GL.Vertex(pts[6]);
        GL.Vertex(pts[6]);
        GL.Vertex(pts[7]);
        GL.Vertex(pts[7]);
        GL.Vertex(pts[4]);

        for (int i = 1; i < 4; i++)
        {
            GL.Vertex(pts[i]);
            GL.Vertex(pts[i + 4]);
        }


        GL.End();
        GL.PopMatrix();
    }
    void SetMeshable(Meshable imeshable, Vector3? direction=null)
    {
        meshable = imeshable;
        Vector3 vectu;
        if (direction.HasValue) vectu = direction.Value;
        else vectu = new Vector3(1, 0, 0);
        BoundingBox bbox = meshable.GetBoundingBox(vectu);
        transform.position = bbox.position;
        transform.localScale = bbox.size;
        Vector3[] vects = bbox.vects;
        transform.forward = vects[2];
        transform.up = vects[1];

        //Mesh mesh = meshable.GetMeshForm();
        Mesh normalMesh = meshable.GetNormalizedMesh(bbox);
        meshFilter.mesh = normalMesh;

    }
    public static ShapeObject CreateBasic()
    {
        GameObject o = new GameObject();
        ShapeObject so = o.AddComponent<ShapeObject>();
        so.meshFilter = o.AddComponent<MeshFilter>();
        so.meshRenderer = o.AddComponent<MeshRenderer>();
        return so;
    }
    public static ShapeObject CreatePolygon(Vector3[] pts)
    {
        Polygon pg = new Polygon(pts);
        ShapeObject so = ShapeObject.CreateBasic();
        Vector3? ld = PointsBase.LongestDirection(pts);
        so.SetMeshable(pg, ld);

        return so;

    }
}
