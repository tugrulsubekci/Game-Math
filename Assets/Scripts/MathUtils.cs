using System.Collections.Generic;
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

    public static List<Vector3> BounceLaser(float maxLaserDistance, Vector3 rayStartPoint, Vector3 rayStartDirection, bool useUnityLibrary = false, bool drawGizmos = true) {
        var totalRayDistance = maxLaserDistance;
        var raycastSucceeded = true;
        var rayOrigin = rayStartPoint;
        var rayDirection = rayStartDirection;

        var hitPoints = new List<Vector3>(){rayOrigin};

        while(totalRayDistance > 0f && raycastSucceeded)
        {
            raycastSucceeded = Physics.Raycast(rayOrigin, rayDirection, out RaycastHit hitInfo, totalRayDistance, Physics.AllLayers);

            var surfaceNormal = hitInfo.normal;

            hitPoints.Add(raycastSucceeded ? hitInfo.point : (rayOrigin + rayDirection * totalRayDistance));

            rayOrigin = hitInfo.point;
            rayDirection = useUnityLibrary ? Vector3.Reflect(rayDirection, surfaceNormal) : ReflectVector(rayDirection, surfaceNormal);

            totalRayDistance -= hitInfo.distance;
        }

        if(drawGizmos)
        {
            for(int i = 0; i < hitPoints.Count - 1; i++)
                Gizmos.DrawLine(hitPoints[i], hitPoints[i + 1]);

            foreach(var point in hitPoints)
                Gizmos.DrawSphere(point, 0.1f);
        }

        return hitPoints;
    }
}
