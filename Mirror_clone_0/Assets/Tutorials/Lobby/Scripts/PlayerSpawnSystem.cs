using Mirror;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;

public class PlayerSpawnSystem : NetworkBehaviour
{
    [SerializeField] private GameObject playerPrefab = null;          // reference playerprefab

    private static List<Transform> spawnPoints = new List<Transform>(); // everybody shares this list because its static

    private int nextIndex = 0;   // spawnindex. when a player spawns, go up 1 in index for next spawnpoint

    public static void AddSpawnPoint(Transform transform) // find the spawned spawnpoints
    {
        spawnPoints.Add(transform);                       // add them

        spawnPoints = spawnPoints.OrderBy(x => x.GetSiblingIndex()).ToList(); // add next in line
    }
    public static void RemoveSpawnPoint(Transform transform) => spawnPoints.Remove(transform); // remove unused spawnpoints

    public override void OnStartServer() => NetworkManagerLobby.OnServerReadied += SpawnPlayer;  // when someone is readied. assign to SpawnPlayer
    
    [ServerCallback]
    private void OnDestroy() => NetworkManagerLobby.OnServerReadied -= SpawnPlayer; // remove assignment to SpawnPlayer after leave

    [Server]
    public void SpawnPlayer(NetworkConnection conn)
    {
        Transform spawnPoint = spawnPoints.ElementAtOrDefault(nextIndex); // get next spawnpoint based on index

        if(spawnPoint == null) // its never null, but making sure theres no errors
        {
            Debug.LogError($"Missing spawn point for player {nextIndex}");
            return;
        }

        GameObject playerInstance = Instantiate(playerPrefab, spawnPoints[nextIndex].position, spawnPoints[nextIndex].rotation); // spawn player through the spawnpoints pos/rot with index
        NetworkServer.Spawn(playerInstance, conn); // spawn your player for other clients

        nextIndex++;
    }




}
