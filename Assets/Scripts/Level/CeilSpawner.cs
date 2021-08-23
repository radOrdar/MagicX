using System.Collections;
using Mirror;
using UnityEngine;

public class CeilSpawner : NetworkBehaviour {
    [SerializeField] private GameObject prefabToSpawn;
    [SerializeField] private float xRange;
    [SerializeField] private float minSpawnDelay;
    [SerializeField] private float maxSpawnDelay;

    [SerializeField] private bool permanentSpawn;
    // [SerializeField] private float maxTorque;

    public override void OnStartServer() {
        if (permanentSpawn) {
            StartCoroutine(SpawnRoutine());
        }
    }

    [Server]
    private IEnumerator SpawnRoutine() {
        while (true) {
            yield return new WaitForSeconds(Random.Range(minSpawnDelay, maxSpawnDelay));
            var instantiate = Instantiate(prefabToSpawn, new Vector2(Random.Range(-xRange, xRange), transform.position.y), Quaternion.identity);
            // instantiate.GetComponent<Rigidbody2D>().AddTorque(Random.Range(-maxTorque, maxTorque), ForceMode2D.Impulse);
            NetworkServer.Spawn(instantiate);
        }
    }

    [Server]
    public void InstantSpawn(int quantity) {
        for (int i = 0; i < quantity; i++) {
            float x = Random.Range(-xRange, xRange);
            float y = Random.Range(transform.position.y, transform.position.y + 5);
            var instantiate = Instantiate(prefabToSpawn, new Vector2(x, y), Quaternion.identity);
            NetworkServer.Spawn(instantiate);
        }
    }
}