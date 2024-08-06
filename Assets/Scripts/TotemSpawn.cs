using UnityEngine;

public class TotemSpawn : MonoBehaviour
{
    public GameObject[] spawnerPrefabs;  // Array of spawner prefabs to instantiate
    public Transform[] spawnerLocations; // Array of locations where spawners should be instantiated

    private void OnTriggerEnter(Collider other)
    {
        if (other.CompareTag("Player"))
        {
            SpawnSpawners();
        }
    }

    private void SpawnSpawners()
    {
        if (spawnerPrefabs == null || spawnerPrefabs.Length == 0)
        {
            Debug.LogError("Spawner prefabs array is not assigned or empty!");
            return;
        }

        if (spawnerLocations == null || spawnerLocations.Length == 0)
        {
            Debug.LogError("Spawner locations array is not assigned or empty!");
            return;
        }

        foreach (Transform location in spawnerLocations)
        {
            int randomIndex = Random.Range(0, spawnerPrefabs.Length);
            GameObject spawnerPrefab = spawnerPrefabs[randomIndex];

            Instantiate(spawnerPrefab, location.position, location.rotation);
            Debug.Log("Spawned a spawner of type: " + spawnerPrefab.name + " at position: " + location.position);
        }
    }
}