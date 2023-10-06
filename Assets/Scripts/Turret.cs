using UnityEditor;
using UnityEngine;

public class Turret : MonoBehaviour
{
    private Camera MainCamera => SceneView.lastActiveSceneView.camera;
    [SerializeField] private GameObject trigger;
    [SerializeField] private GameObject turretHead;
    [SerializeField] private float angle = 30;
    [SerializeField] private float maxRadius = 3.0f;
    [SerializeField] private float minRadius = 1.0f;
    [SerializeField] private float height = 3.0f;
    [SerializeField] private float headRotationSpeed = 1.0f;
    [SerializeField] private float maxFireRange = 10.0f;
    private Vector3 turretHeadPosition => turretHead.transform.position;
    private Quaternion defaultOrientationOfHead => Quaternion.LookRotation(transform.forward, transform.up);
    [HideInInspector] public bool fire = false;
    [SerializeField] private TriggerType triggerType = TriggerType.Wedge;
    private ISensor Sensor => GetSensor(triggerType);

    // Turret Placement Gizmos
    private void OnDrawGizmosSelected()
    {
        Physics.Raycast(MainCamera.transform.position, MainCamera.transform.forward, out RaycastHit hitInfo, 100f);

        Gizmos.DrawSphere(hitInfo.point, 0.1f);
        
        Gizmos.color = Color.green;
        Gizmos.DrawRay(hitInfo.point, hitInfo.normal);

        Gizmos.color = Color.blue;
        Gizmos.DrawRay(hitInfo.point, Vector3.Cross(MainCamera.transform.right, hitInfo.normal));
    }

    // Turret Trigger Check Gizmos
    private void OnDrawGizmos()
    {
        bool isTargetInside = Sensor.Check(trigger.transform.position);

        Sensor.DrawGizmos(isTargetInside);

        // Turret Head Rotation Section (DO NOT FORGET ENABLE "ALWAYS REFRESH" IN SCENE VIEW)----------------------------
        var fromRotation = turretHead.transform.rotation;
        var toRotation = isTargetInside ? Quaternion.LookRotation(trigger.transform.position - turretHeadPosition, transform.up) : defaultOrientationOfHead;
        var deltaRotation = Quaternion.Angle(fromRotation, toRotation);

        turretHead.transform.rotation = Quaternion.Slerp(fromRotation, toRotation, headRotationSpeed / deltaRotation * Time.deltaTime * 10.0f);
        // Turret Head Rotation Section ----------------------------------------------------------------------------------

        Gizmos.color = Color.yellow;

        if(fire)
            MathUtils.BounceLaser(maxFireRange, turretHeadPosition, turretHead.transform.forward);
    }

    public void PlaceTurret()
    {
        bool raycastSucceeded = Physics.Raycast(MainCamera.transform.position, MainCamera.transform.forward, out RaycastHit hitInfo, 100f);

        if(raycastSucceeded)
            transform.SetPositionAndRotation(hitInfo.point, Quaternion.LookRotation(Vector3.Cross(MainCamera.transform.right, hitInfo.normal), hitInfo.normal));
    }

    public void Fire()
    {
        fire = !fire;
    }
    private ISensor GetSensor(TriggerType triggerType)
    {
        switch (triggerType)
        {
            case TriggerType.Wedge:
                return new WedgeSensor(angle, minRadius, maxRadius, height, transform);
            case TriggerType.Sphere:
                return new SphereSensor(angle, minRadius, maxRadius, transform);
            case TriggerType.Cone:
                return new ConeSensor(angle, minRadius, maxRadius, transform, turretHeadPosition);
            default:
                return new WedgeSensor(angle, minRadius, maxRadius, height, transform);
        }
    }

    public enum TriggerType
    {
        Wedge,
        Sphere,
        Cone
    }
}
