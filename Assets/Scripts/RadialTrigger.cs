using UnityEngine;

public class RadialTrigger : MonoBehaviour
{
    public float radius = 1.0f;
    public Transform player;
    private void OnDrawGizmos() {
        var distanceFromPlayer = Vector3.Distance(transform.position, player.position);
        Gizmos.color = distanceFromPlayer > radius ? Color.green : Color.red;
        
        Gizmos.DrawWireSphere(transform.position, radius);
    }
}
