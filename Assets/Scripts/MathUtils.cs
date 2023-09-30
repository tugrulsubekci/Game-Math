using UnityEngine;

public class MathUtils
{
    public static Vector3 ReflectVector(Vector3 rayDirection, Vector3 surfaceNormal)
    {
        var scalarProjection = Vector3.Dot(rayDirection, surfaceNormal);
        return rayDirection - 2 * scalarProjection * surfaceNormal;
    }
}
