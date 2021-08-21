using Mirror;
using UnityEngine;

public class ContactPusher : MonoBehaviour {
    [SerializeField] private Vector2 direction;
    [SerializeField] private float force;

    private Vector2 directionNormalized;

    private void Awake() {
        directionNormalized = direction.normalized;
    }

    [ServerCallback]
    private void OnCollisionEnter2D(Collision2D other) {
        if (!other.collider.CompareTag("Player")) { return; }

        other.gameObject.GetComponent<Rigidbody2D>().AddForce(directionNormalized * force, ForceMode2D.Impulse);
    }
}