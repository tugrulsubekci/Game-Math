using UnityEditor;
using UnityEngine;

public class Turret : MonoBehaviour
{
    private Camera MainCamera => SceneView.lastActiveSceneView.camera;
    [SerializeField] private GameObject platform;
    [SerializeField] private GameObject trigger;
    [SerializeField] private float angle = 30;
    [SerializeField] private float radius = 3.0f;
    [SerializeField] private float height = 3.0f;
    private Vector3 downPosition => transform.position;
    private Vector3 upPosition => transform.position + transform.up * height;
    private void OnDrawGizmosSelected() {
        Physics.Raycast(MainCamera.transform.position, MainCamera.transform.forward, out RaycastHit hitInfo, 100f);

        Gizmos.DrawSphere(hitInfo.point, 0.1f);
        
        Gizmos.color = Color.green;
        Gizmos.DrawRay(hitInfo.point, hitInfo.normal);

        Gizmos.color = Color.blue;
        Gizmos.DrawRay(hitInfo.point, Vector3.Cross(MainCamera.transform.right, hitInfo.normal));
    }

    private void OnDrawGizmos() {
        Handles.DrawWireDisc(downPosition, transform.up, radius);
        Handles.DrawWireDisc(upPosition, transform.up, radius);

        var rightRay = transform.forward * Mathf.Sin((90 - angle / 2.0f) * Mathf.Deg2Rad ) + transform.right * Mathf.Cos((90 - angle / 2.0f) * Mathf.Deg2Rad );
        var leftRay = transform.forward * Mathf.Sin((90 - angle / 2.0f) * Mathf.Deg2Rad ) - transform.right * Mathf.Cos((90 - angle / 2.0f) * Mathf.Deg2Rad );

        var rightDown = downPosition + rightRay * radius;
        var rightUp = upPosition + rightRay * radius;
        var leftDown = downPosition + leftRay * radius;
        var leftUp = upPosition + leftRay * radius;

        Gizmos.color = TriggerCheck(trigger.transform.position) ? Color.red : Color.green;

        Gizmos.DrawLine(downPosition, rightDown);
        Gizmos.DrawLine(downPosition, leftDown);

        Gizmos.DrawLine(upPosition, rightUp);
        Gizmos.DrawLine(upPosition, leftUp);

        Gizmos.DrawLine(rightDown, rightUp);
        Gizmos.DrawLine(leftDown, leftUp);
        
    }

    public void PlaceTurret() {
        bool raycastSucceeded = Physics.Raycast(MainCamera.transform.position, MainCamera.transform.forward, out RaycastHit hitInfo, 100f);

        if(raycastSucceeded)
        {
            transform.position = hitInfo.point;
            transform.rotation = Quaternion.LookRotation(Vector3.Cross(MainCamera.transform.right, hitInfo.normal), hitInfo.normal);
        }
    }

    public bool TriggerCheck(Vector3 triggerPosition) {
        var dirToTargetWorld = triggerPosition - downPosition;
        var dirToTargetLocal = transform.InverseTransformVector(dirToTargetWorld);

        // height check
        if(dirToTargetLocal.y < 0 || dirToTargetLocal.y > height)
            return false;

        // cylindirical radius check
        var ignoreYAxis = new Vector3(dirToTargetLocal.x, 0, dirToTargetLocal.z);

        if(ignoreYAxis.magnitude > radius)
            return false;

        // angle check
        if(Vector3.Angle(transform.TransformVector(ignoreYAxis.normalized), transform.forward) > angle / 2.0f)
            return false;

        return true;
    }

}
