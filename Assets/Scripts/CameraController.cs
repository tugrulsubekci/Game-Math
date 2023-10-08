using UnityEngine;

public class CameraController : MonoBehaviour
{
    private float pitch;
    private float yaw;

    private void Awake()
    {
        var startAngles = transform.eulerAngles;
        pitch = startAngles.x;
        yaw = startAngles.y;
    }

    private void Update()
    {
        var xDelta = Input.GetAxis("Mouse X");
        var yDelta = Input.GetAxis("Mouse Y");

        pitch -= yDelta;
        yaw += xDelta;

        transform.rotation = Quaternion.Euler(new Vector3(pitch, yaw, 0));
    }
}
