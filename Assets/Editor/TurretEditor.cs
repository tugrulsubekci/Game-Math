using UnityEditor;
using UnityEngine;

[CustomEditor(typeof(Turret))]
[CanEditMultipleObjects]
public class TurretEditor : Editor
{
    public override void OnInspectorGUI()
    {
        DrawDefaultInspector();

        var targetObject = (Turret)target;
        if(GUILayout.Button("Place Turret"))
            targetObject.PlaceTurret();

        if(GUILayout.Button(targetObject.fire ? "Stop Fire" : "Start Fire"))
            targetObject.Fire();
    }
}
