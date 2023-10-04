using UnityEngine;

public class BouncingLaser : MonoBehaviour
{
    public float maxLaserDistance = 100f;
    public bool useUnityLibrary = false;
    private void OnDrawGizmos() {
        MathUtils.BounceLaser(maxLaserDistance, transform.position, transform.forward, useUnityLibrary);
    }


}
