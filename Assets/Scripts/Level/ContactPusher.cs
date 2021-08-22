using Mirror;
using UnityEngine;

public class ContactPusher : MonoBehaviour {
    [SerializeField] private Vector2 direction;
    [SerializeField] private float forcePerMassMagnitude;

    private Vector2 forcePerMass;

    private void Awake() {
        forcePerMass = direction.normalized * forcePerMassMagnitude;
    }

    [ServerCallback]
    private void OnCollisionEnter2D(Collision2D other) {
        if (!other.gameObject.TryGetComponent(out Rigidbody2D rb)) { return; }

        if (other.gameObject.TryGetComponent(out BaseCharacterMovement baseCharacterMovement)) {
            baseCharacterMovement.RpcAddForce(forcePerMass * rb.mass);
        } else {
            rb.AddForce(forcePerMass * rb.mass, ForceMode2D.Impulse);
        }
    }
}