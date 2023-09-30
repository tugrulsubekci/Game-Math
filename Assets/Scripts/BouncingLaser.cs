using System.Collections.Generic;
using UnityEngine;

public class BouncingLaser : MonoBehaviour
{
    public float maxLaserDistance = 100f;
    public bool useUnityLibrary = false;
    private float totalRayDistance;
    private bool raycastSucceeded;
    private Vector3 rayOrigin;
    private Vector3 rayDirection;
    private List<Vector3> hitPoints;
    private void OnDrawGizmos() {
        totalRayDistance = maxLaserDistance;
        raycastSucceeded = true;
        rayOrigin = transform.position;
        rayDirection = transform.forward;

        hitPoints = new List<Vector3>(){rayOrigin};

        while(totalRayDistance > 0f && raycastSucceeded)
        {
            raycastSucceeded = Physics.Raycast(rayOrigin, rayDirection, out RaycastHit hitInfo, totalRayDistance, Physics.AllLayers);

            var surfaceNormal = hitInfo.normal;

            hitPoints.Add(raycastSucceeded ? hitInfo.point : (rayOrigin + rayDirection * totalRayDistance));

            rayOrigin = hitInfo.point;
            rayDirection = useUnityLibrary ? Vector3.Reflect(rayDirection, surfaceNormal) : MathUtils.ReflectVector(rayDirection, surfaceNormal);

            totalRayDistance -= hitInfo.distance;
        }

        for(int i = 0; i < hitPoints.Count - 1; i++)
            Gizmos.DrawLine(hitPoints[i], hitPoints[i + 1]);

        foreach(var point in hitPoints)
            Gizmos.DrawSphere(point, 0.1f);
    }


}
