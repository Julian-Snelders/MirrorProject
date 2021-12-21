using UnityEngine;

public class PlayerSpawnPoint : MonoBehaviour
{
    private void Awake() => PlayerSpawnSystem.AddSpawnPoint(transform);          // next method = Addspawnpoint. and assign this spawnpoint to it
    private void OnDestroy() => PlayerSpawnSystem.RemoveSpawnPoint(transform);   // same here but removes it

    private void OnDrawGizmos()          // spawnpoint visualized
    {
        Gizmos.color = Color.blue;                       // position color
        Gizmos.DrawSphere(transform.position, 1f);       // position
        Gizmos.color = Color.green;                      // rotation color
        Gizmos.DrawLine(transform.position, transform.position + transform.forward * 2);  // rotation
    }
}
