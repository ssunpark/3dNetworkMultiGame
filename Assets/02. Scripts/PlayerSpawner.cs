using UnityEngine;

public class PlayerSpawner : MonoBehaviour
{
    public GameObject PlayerPrefab;
    public Transform[] SpawnPoints;

    private void PlayerSpawn()
    {
        int r = Random.Range(0, SpawnPoints.Length);
        Instantiate(PlayerPrefab, SpawnPoints[r].position, SpawnPoints[r].rotation);
    }
}
