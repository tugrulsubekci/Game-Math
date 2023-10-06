using UnityEditor;
using UnityEngine;

public class WedgeSensor : Sensor
{
    private float height;
    private Vector3 downPosition => transformOrigin.position;
    private Vector3 upPosition => transformOrigin.position + transformOrigin.up * height;
    public WedgeSensor(float angle, float minRadius, float maxRadius, float height, Transform transformOrigin) : base(angle, minRadius, maxRadius, transformOrigin)
    {
        this.height = height;
    }

    public override bool Check(Vector3 triggerPosition)
    {
        var dirToTargetWorld = triggerPosition - transformOrigin.position;
        var dirToTargetLocal = transformOrigin.InverseTransformVector(dirToTargetWorld);

        // height check
        if(dirToTargetLocal.y < 0 || dirToTargetLocal.y > height)
            return false;

        // cylindirical radius check
        var ignoreYAxis = new Vector3(dirToTargetLocal.x, 0, dirToTargetLocal.z);

        if(ignoreYAxis.magnitude > maxRadius || ignoreYAxis.magnitude < minRadius)
            return false;

        // angle check
        if(Vector3.Angle(transformOrigin.TransformVector(ignoreYAxis.normalized), transformOrigin.forward) > angle / 2.0f)
            return false;

        return true;
    }

    public override void DrawGizmos(bool isTriggerInside)
    {
        var rightRay = transformOrigin.forward * Mathf.Sin((90 - angle / 2.0f) * Mathf.Deg2Rad ) + transformOrigin.right * Mathf.Cos((90 - angle / 2.0f) * Mathf.Deg2Rad );
        var leftRay = transformOrigin.forward * Mathf.Sin((90 - angle / 2.0f) * Mathf.Deg2Rad ) - transformOrigin.right * Mathf.Cos((90 - angle / 2.0f) * Mathf.Deg2Rad );

        var rightDown = downPosition + rightRay * maxRadius;
        var rightUp = upPosition + rightRay * maxRadius;
        var leftDown = downPosition + leftRay * maxRadius;
        var leftUp = upPosition + leftRay * maxRadius;

        Gizmos.color = isTriggerInside ? Color.red : Color.green;
        Handles.color = isTriggerInside ? Color.red : Color.green;

        Gizmos.DrawLine(downPosition + rightRay * minRadius, rightDown);
        Gizmos.DrawLine(downPosition + leftRay * minRadius, leftDown);

        Gizmos.DrawLine(upPosition + rightRay * minRadius, rightUp);
        Gizmos.DrawLine(upPosition + leftRay * minRadius, leftUp);

        Gizmos.DrawLine(rightDown, rightUp);
        Gizmos.DrawLine(leftDown, leftUp);

        Gizmos.DrawLine(downPosition + rightRay * minRadius, upPosition + rightRay * minRadius);
        Gizmos.DrawLine(downPosition + leftRay * minRadius, upPosition + leftRay * minRadius);

        Handles.DrawWireArc(upPosition, transformOrigin.up, leftRay, angle, maxRadius);
        Handles.DrawWireArc(downPosition, transformOrigin.up, leftRay, angle, maxRadius);

        Handles.DrawWireArc(upPosition, transformOrigin.up, leftRay * minRadius, angle, minRadius);
        Handles.DrawWireArc(downPosition, transformOrigin.up, leftRay * minRadius, angle, minRadius);
    }
}
