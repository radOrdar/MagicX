using Mirror;
using UnityEngine;

public class Bullet : NetworkBehaviour {
    [SerializeField] private Rigidbody2D rb;
    [SerializeField] private ParticleSystem explosion;
    [SerializeField] private float impulseOnCollisionMultiplier;

    [SyncVar] private Vector2 startSpeed;
    private GameObject owner;
    private float damageToDeal;

    public void Start() {
        rb.AddForce(startSpeed * rb.mass, ForceMode2D.Impulse);
    }

    private void FixedUpdate() {
        RotateTowardMoveDirection();
    }

    private void RotateTowardMoveDirection() {
        var currentVelocity = rb.velocity;
        rb.MoveRotation(Mathf.Atan2(currentVelocity.y, currentVelocity.x) * Mathf.Rad2Deg);
    }

    public void Initialize(Vector2 speed, GameObject owner, float damageToDeal) {
        startSpeed = speed;
        this.owner = owner;
        this.damageToDeal = damageToDeal;
    }

    public override void OnStopClient() {
        base.OnStopClient();
        Instantiate(explosion, transform.position, Quaternion.identity);
    }

    #region Server

    [ServerCallback]
    private void OnTriggerEnter2D(Collider2D other) {
        // if (other.TryGetComponent(out NetworkIdentity identity)) {
        //     if (identity.connectionToClient == connectionToClient) {
        //         return;
        //     }
        // }
        if (other.CompareTag("Floor")) {
            // var closestPoint = other.ClosestPoint(transform.position);
            if (other.TryGetComponent(out Rigidbody2D otherRb)) {
                // otherRb.AddForceAtPosition(transform.right * rb.mass * impulseOnCollisionMultiplier, closestPoint);
                otherRb.AddForce(transform.right * rb.mass * impulseOnCollisionMultiplier);
            }
        }

        if (other.TryGetComponent(out Health health)) {
            health.DealDamage(damageToDeal, owner);
        }

        DestroySelf();
    }

    [Server]
    private void DestroySelf() {
        NetworkServer.Destroy(gameObject);
    }

    #endregion
}