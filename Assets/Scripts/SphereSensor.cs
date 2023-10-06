using UnityEngine;

public class SphereSensor : Sensor
{
    public SphereSensor(float angle, float minRadius, float maxRadius, Transform transformOrigin) : base(angle, minRadius, maxRadius, transformOrigin)
    {
    }

    public override bool Check(Vector3 triggerPosition)
    {
        var distanceFromTurret = Vector3.Distance(triggerPosition, transformOrigin.position);
        // radius check
        if(distanceFromTurret > maxRadius || distanceFromTurret < minRadius)
            return false;

        return true;
    }

    public override void DrawGizmos(bool isTriggerInside)
    {
        Gizmos.color = isTriggerInside ? Color.red : Color.green;

        Gizmos.DrawWireSphere(transformOrigin.position, maxRadius);
        Gizmos.DrawWireSphere(transformOrigin.position, minRadius);
    }
}
