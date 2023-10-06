using UnityEditor;
using UnityEngine;

public class ConeSensor : Sensor
{
    private Vector3 coneStartPosition;
    public ConeSensor(float angle, float minRadius, float maxRadius, Transform transformOrigin, Vector3 coneStartPosition) : base(angle, minRadius, maxRadius, transformOrigin)
    {
        this.coneStartPosition = coneStartPosition;
    }

    public override bool Check(Vector3 triggerPosition)
    {
        // radius check
        var distanceFromTurret = Vector3.Distance(triggerPosition, coneStartPosition);

        if(distanceFromTurret > maxRadius || distanceFromTurret < minRadius)
            return false;

        // angle check
        var dirToTargetWorld = triggerPosition - coneStartPosition;
        var dirToTargetLocal = transformOrigin.InverseTransformVector(dirToTargetWorld);

        if(Vector3.Angle(transformOrigin.InverseTransformVector(transformOrigin.forward), dirToTargetLocal) > angle / 2.0f)
            return false;
            
        return true;
    }

    public override void DrawGizmos(bool isTriggerInside)
    {
        Gizmos.color = isTriggerInside ? Color.red : Color.green;
        Handles.color = isTriggerInside ? Color.red : Color.green;

        var upRay = transformOrigin.forward * Mathf.Sin((90 - angle / 2.0f) * Mathf.Deg2Rad ) + transformOrigin.up * Mathf.Cos((90 - angle / 2.0f) * Mathf.Deg2Rad );
        var downRay = transformOrigin.forward * Mathf.Sin((90 - angle / 2.0f) * Mathf.Deg2Rad ) - transformOrigin.up * Mathf.Cos((90 - angle / 2.0f) * Mathf.Deg2Rad );

        var leftRay = transformOrigin.forward * Mathf.Sin((90 - angle / 2.0f) * Mathf.Deg2Rad ) - transformOrigin.right * Mathf.Cos((90 - angle / 2.0f) * Mathf.Deg2Rad );
        var rightRay = transformOrigin.forward * Mathf.Sin((90 - angle / 2.0f) * Mathf.Deg2Rad ) + transformOrigin.right * Mathf.Cos((90 - angle / 2.0f) * Mathf.Deg2Rad );

        Handles.DrawWireArc(coneStartPosition, transformOrigin.up, leftRay, angle, maxRadius);
        Handles.DrawWireArc(coneStartPosition, transformOrigin.up, leftRay, angle, maxRadius);

        Handles.DrawWireArc(coneStartPosition, transformOrigin.up, leftRay * minRadius, angle, minRadius);
        Handles.DrawWireArc(coneStartPosition, transformOrigin.up, leftRay * minRadius, angle, minRadius);

        Handles.DrawWireArc(coneStartPosition, transformOrigin.right, upRay, angle, maxRadius);
        Handles.DrawWireArc(coneStartPosition, transformOrigin.right, upRay, angle, maxRadius);

        Handles.DrawWireArc(coneStartPosition, transformOrigin.right, upRay * minRadius, angle, minRadius);
        Handles.DrawWireArc(coneStartPosition, transformOrigin.right, upRay * minRadius, angle, minRadius);

        Handles.DrawWireDisc(coneStartPosition + transformOrigin.forward * Vector3.Dot(leftRay, transformOrigin.forward) * minRadius, transformOrigin.forward, Mathf.Sin(angle / 2.0f * Mathf.Deg2Rad) * minRadius);
        Handles.DrawWireDisc(coneStartPosition + transformOrigin.forward * Vector3.Dot(leftRay, transformOrigin.forward) * maxRadius, transformOrigin.forward, Mathf.Sin(angle / 2.0f * Mathf.Deg2Rad) * maxRadius);

        var minRight = coneStartPosition + rightRay * minRadius;
        var maxRight = coneStartPosition + rightRay * maxRadius;
        var minLeft = coneStartPosition + leftRay * minRadius;
        var maxLeft = coneStartPosition + leftRay * maxRadius;

        Gizmos.DrawLine(minRight, maxRight);
        Gizmos.DrawLine(minLeft, maxLeft);

        var minUp = coneStartPosition + upRay * minRadius;
        var maxUp = coneStartPosition + upRay * maxRadius;
        var minDown = coneStartPosition + downRay * minRadius;
        var maxDown = coneStartPosition + downRay * maxRadius;

        Gizmos.DrawLine(minUp, maxUp);
        Gizmos.DrawLine(minDown, maxDown);
    }
}
