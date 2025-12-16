using UnityEngine;

public class NPCSpawner : MonoBehaviour
{
    public GameObject[] npcPrefabs;
    public Transform spawnPoint;

    void Start()
    {
        SpawnNPC();
    }

    void SpawnNPC()
    {
        int index = Random.Range(0, npcPrefabs.Length);
        Instantiate(
            npcPrefabs[index],
            spawnPoint.position,
            spawnPoint.rotation
        );
    }
}
