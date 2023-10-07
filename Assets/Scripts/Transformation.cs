using UnityEditor;
using UnityEngine;

public class Transformation : MonoBehaviour
{
    [SerializeField] private Vector3 localPosition;

    private void OnDrawGizmos() {

        var worldPosition = MathUtils.LocalToWorld(localPosition, transform);
        Gizmos.DrawSphere(worldPosition, 0.1f);

        var worldToLocalPos = MathUtils.WorldToLocal(worldPosition, transform);

        var labelOffset = new Vector3(0, 0.2f, 0f);
        Handles.Label(worldPosition + labelOffset, new GUIContent("local position: " + worldToLocalPos + "\nworld position: " + worldPosition));
    }

}
