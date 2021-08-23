using Mirror;
using UnityEngine;

public class TriggerDamageDestroy : MonoBehaviour {
    [SerializeField] private float dmg;

    [ServerCallback]
    private void OnTriggerEnter2D(Collider2D other) {
        if (other.CompareTag("Player")) {
            other.GetComponent<Health>().DealDamage(dmg, gameObject);
        }

        NetworkServer.Destroy(gameObject);
    }
}