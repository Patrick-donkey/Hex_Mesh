using System.Collections.Generic;
using UnityEngine;

public struct Face
{
    public List<Vector3> vertices { get; private set; }
    public List<int> triangle { get; private set; }
    public List<Vector2> uvs { get; private set; }

    public Face(List<Vector3> vertices, List<int> triangle,List<Vector2>uvs)
    {
        this.vertices = vertices;
        this.triangle = triangle;
        this.uvs = uvs;
    }
}

[RequireComponent(typeof(MeshFilter))]
[RequireComponent(typeof(MeshRenderer))]
public class HexMesh : MonoBehaviour
{
    Mesh mesh;
    MeshFilter meshFilter;
    MeshRenderer meshRenderer;
    [SerializeField]
    Material material;

    List<Face> faces;

    private void Start()
    {
        meshFilter = GetComponent<MeshFilter>();
        meshRenderer = GetComponent<MeshRenderer>();

        mesh = new Mesh();
        mesh.name = "Hex";

        meshFilter.mesh = mesh;
        meshRenderer.material = material;
    }

    private void OnEnable()
    {
        DrawMesh();
        CombineFaces();
    }

    private void OnValidate()
    {
        if (Application.isPlaying)
        {
            DrawMesh();
        }
    }

    public void DrawMesh()
    {
        faces = new List<Face>();

        for (int i = 0;i<6; i++)
        {

        }
    }

    Face CreateFace(float inner, float outer, float heightA, float heightB, int point, bool reverse = false)
    {
        return new Face();
    }

    protected Vector3 GetPoint(float size,float height,int index)
    {
        float angle_deg = 60 * index;
        float angle_rad = Mathf.PI/180f*angle_deg;
        return new Vector3(size*Mathf.Cos(angle_rad),height,size*Mathf.Sin(angle_rad));
    }

    void CombineFaces()
    {
        List<Vector3> vertices = new List<Vector3>();
        List<int> triangle = new List<int>();
        List<Vector2> uvs = new List<Vector2>();

        for(int i = 0; i < faces.Count; i++)
        {
            vertices.AddRange(faces[i].vertices);
            uvs.AddRange(faces[i].uvs);

            int offset = (4* i);
            foreach(int tris in faces[i].triangle)
            {
                triangle.Add(tris+offset);
            }
        }
        mesh.vertices = vertices.ToArray();
        mesh.triangles = triangle.ToArray();
        mesh.uv = uvs.ToArray();
        mesh.RecalculateNormals();
    }
}
