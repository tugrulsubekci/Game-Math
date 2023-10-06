using UnityEngine;

public interface ISensor
{
    void DrawGizmos(bool isTriggerInside);
    bool Check(Vector3 triggerPosition);
}
