using UnityEngine;

public abstract class Sensor : ISensor
{
    protected float angle;
    protected float minRadius;
    protected float maxRadius;
    protected Transform transformOrigin;
    protected Sensor(float angle, float minRadius, float maxRadius, Transform transformOrigin)
    {
        this.angle = angle;
        this.minRadius = minRadius;
        this.maxRadius = maxRadius;
        this.transformOrigin = transformOrigin;
    }

    public abstract bool Check(Vector3 triggerPosition);
    public abstract void DrawGizmos(bool isTriggerInside);
}
