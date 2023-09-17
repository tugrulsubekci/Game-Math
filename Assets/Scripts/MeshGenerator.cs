using System.Collections.Generic;
using UnityEngine;

[RequireComponent(typeof(MeshFilter))]
[RequireComponent(typeof(MeshRenderer))]
public class MeshGenerator : MonoBehaviour
{
    private Mesh mesh;
    public StartType startType = StartType.Middle;
    public MeshType meshType = MeshType.Cube;
    // cube variables
    public float height;
    public float width;
    public float depth;

    public void CreateMesh() {
        if(mesh == null)
            mesh = new Mesh();

        mesh.Clear();
        mesh.name = meshType.ToString();

        var meshFilter = GetComponent<MeshFilter>();

        meshFilter.sharedMesh = mesh;

        float zValue = 0;
        
        if(startType == StartType.Middle)
            zValue = - depth / 2.0f;
        else if(startType == StartType.Front)
            zValue = 0;
        else if(startType == StartType.Back)
            zValue = -depth;

        // front side vertices
        var f0 = new Vector3(width / 2 , height / 2, zValue );
        var f1 = new Vector3(width / 2 , - height / 2, zValue );
        var f2 = new Vector3(- width / 2 , - height / 2, zValue );
        var f3 = new Vector3(- width / 2 , height / 2, zValue );

        if(startType == StartType.Middle)
            zValue = depth / 2.0f;
        else if(startType == StartType.Front)
            zValue = depth;
        else if(startType == StartType.Back)
            zValue = 0;

        // back side vertices
        var b0 = new Vector3(width / 2 , height / 2, zValue );
        var b1 = new Vector3(width / 2 , - height / 2, zValue );
        var b2 = new Vector3(- width / 2 , - height / 2, zValue );
        var b3 = new Vector3(- width / 2 , height / 2, zValue );

        // setting vertices for all faces
        var vertices = new Vector3[] {
            f0, f1, f2, f3, // front face
            f0, b0, b1, f1, // right face
            f1, b1, b2, f2, // bottom face
            f2, b2, b3, f3, // left face
            f3, b3, b0, f0, // up face
            b0, b1, b2, b3  // back face
        };

        // triangles according to the left hand rule
        int[] triangles = new int[]
        {
	        0, 1, 2,        0, 2, 3,
	        4, 5, 7,        5, 6, 7,
	        8, 9, 10,       11, 8, 10,
	        13, 14, 15,     13, 15, 12,
	        16, 17, 18,     16, 18, 19,
	        20, 23, 21,     21, 23, 22,
        };

        var uv00 = new Vector2(0, 0); 
        var uv01 = new Vector2(0, 1); 
        var uv10 = new Vector2(1, 0); 
        var uv11 = new Vector2(1, 1);

        var uvs = new List<Vector2>() {
            uv11, uv10, uv00, uv01, // front face
            uv01, uv11, uv10, uv00, // right face
            uv11, uv01, uv00, uv01, // bottom face
            uv01, uv11, uv10, uv00, // left face
            uv00, uv10, uv11, uv01, // up face
            uv10, uv00, uv10, uv11  // back face
        };

        // Define each vertex's Normal
        Vector3 up = Vector3.up;
        Vector3 down = Vector3.down;
        Vector3 forward = Vector3.forward;
        Vector3 back = Vector3.back;
        Vector3 left = Vector3.left;
        Vector3 right = Vector3.right;


        Vector3[] normals = new Vector3[]
        {
	        back, back, back, back,             // Back
	        right, right, right, right,         // Right
	        down, down, down, down,             // Bottom
	        left, left, left, left,             // Left
	        up, up, up, up,                     // Top
	        forward, forward, forward, forward	// Front
        };

        mesh.SetVertices(vertices);
        mesh.SetTriangles(triangles, 0);
        mesh.SetUVs(0 ,uvs);
        mesh.SetNormals(normals);
        
    }

    public enum MeshType {
        Cube
    }
    public enum StartType {
        Front,
        Middle,
        Back
    }
}
