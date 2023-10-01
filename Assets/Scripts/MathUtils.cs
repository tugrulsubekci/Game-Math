using UnityEngine;

public class MathUtils
{
    public static Vector3 ReflectVector(Vector3 rayDirection, Vector3 surfaceNormal)
    {
        var scalarProjection = Vector3.Dot(rayDirection, surfaceNormal);
        return rayDirection - 2 * scalarProjection * surfaceNormal;
    }

    public static Vector3 LocalToWorld(Vector3 localPoint, Transform transform)
    {
        var right = (Vector3)transform.worldToLocalMatrix.GetColumn(0).normalized;
        var up = (Vector3)transform.worldToLocalMatrix.GetColumn(1).normalized;
        var forward = (Vector3)transform.worldToLocalMatrix.GetColumn(2).normalized;
        var position = transform.position;

        var dotRight = Vector3.Dot(localPoint, right);
        var dotUp = Vector3.Dot(localPoint, up);
        var dotForward = Vector3.Dot(localPoint, forward);
        
        position += new Vector3(dotRight, dotUp, dotForward);

        return position;
    }

    public static Vector3 WorldToLocal(Vector3 worldPoint, Vector3 referencePoint)
    {
        return worldPoint - referencePoint;
    }
}
