using System.Collections;
using Mirror;
using UnityEngine;

public class CeilSpawner : NetworkBehaviour {
    [SerializeField] private GameObject prefabToSpawn;
    [SerializeField] private float xRange;
    [SerializeField] private float minSpawnDelay;
    [SerializeField] private float maxSpawnDelay;
    [SerializeField] private float maxTorque;

    public override void OnStartServer() {
        StartCoroutine(SpawnRoutine());
    }

    private IEnumerator SpawnRoutine() {
        while (true) {
            yield return new WaitForSeconds(Random.Range(minSpawnDelay, maxSpawnDelay));
            var instantiate = Instantiate(prefabToSpawn, new Vector2(Random.Range(-xRange, xRange), transform.position.y), Quaternion.identity);
            instantiate.GetComponent<Rigidbody2D>().AddTorque(Random.Range(-maxTorque, maxTorque), ForceMode2D.Impulse);
            NetworkServer.Spawn(instantiate);
        }
    }
}