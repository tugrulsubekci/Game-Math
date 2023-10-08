using System.Collections.Generic;
using UnityEngine;

public class MathUtils
{
    public const float roundAngle = 360.0f * Mathf.Deg2Rad;
    public const float halfAngle = 180.0f * Mathf.Deg2Rad;
    public const float quarterAngle = 90.0f * Mathf.Deg2Rad;
    public const float totalHours = 12.0f;
    public const float totalMinutes = 60.0f;
    public const float totalSeconds = 60.0f;
    public const float totalMiliseconds = 1000.0f;
    public static Vector3 ReflectVector(Vector3 rayDirection, Vector3 surfaceNormal)
    {
        var scalarProjection = Vector3.Dot(rayDirection, surfaceNormal);
        return rayDirection - 2 * scalarProjection * surfaceNormal;
    }

    public static Vector3 LocalToWorld(Vector3 localPoint, Transform transform)
    {
        var position = transform.position;

        // FIRST WAY ---------------------------------------------------------------
        // var right = (Vector3)transform.worldToLocalMatrix.GetColumn(0).normalized;
        // var up = (Vector3)transform.worldToLocalMatrix.GetColumn(1).normalized;
        // var forward = (Vector3)transform.worldToLocalMatrix.GetColumn(2).normalized;
        // var dotRight = Vector3.Dot(localPoint, right);
        // var dotUp = Vector3.Dot(localPoint, up);
        // var dotForward = Vector3.Dot(localPoint, forward);
        // position += new Vector3(dotRight, dotUp, dotForward);

        // SECOND WAY ---------------------------------------------------------------
        position += localPoint.x * transform.right;
        position += localPoint.y * transform.up;
        position += localPoint.z * transform.forward;

        return position;
    }

    public static Vector3 WorldToLocal(Vector3 worldPoint, Transform transform)
    {
        var deltaVector = worldPoint - transform.position;
        return new Vector3(Vector3.Dot(deltaVector, transform.right), Vector3.Dot(deltaVector, transform.up), Vector3.Dot(deltaVector, transform.forward));
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

    /// <summary>
    /// Angle in radians. Returns normalized vector
    /// </summary>
    public static Vector3 AngleToDirection(float angle)
    {
        return new Vector3(Mathf.Cos(quarterAngle - angle), Mathf.Sin(quarterAngle - angle), 0);
    }
}
