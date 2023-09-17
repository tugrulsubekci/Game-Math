using UnityEditor;
using UnityEngine;

[CustomEditor(typeof(MeshGenerator))]
[CanEditMultipleObjects]
public class MeshGeneratorEditor : Editor
{
    public override void OnInspectorGUI()
    {
        DrawDefaultInspector();

        var targetObject = (MeshGenerator)target;
        if(GUILayout.Button("Generate Mesh"))
            targetObject.CreateMesh();

    }
}
