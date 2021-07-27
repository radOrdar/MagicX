using Mirror;
using UnityEngine;

public class Bullet : NetworkBehaviour, IDamageable {
    // [SerializeField] private int damageToDeal = 20;
    [SerializeField] private Rigidbody2D rb;
    [SerializeField] private ParticleSystem explosion;

    [SyncVar] private Vector2 startSpeed;
    private GameObject owner;
    private float damageToDeal;

    public void Start() {
        rb.AddForce(startSpeed, ForceMode2D.Impulse);
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
        if (other.TryGetComponent(out NetworkIdentity identity)) {
            if (identity.connectionToClient == connectionToClient) {
                return;
            }
        }

        if (other.TryGetComponent(out Health health)) {
            health.DealDamage(damageToDeal);
        }

        DestroySelf();
    }

    [Server]
    private void DestroySelf() {
        NetworkServer.Destroy(gameObject);
    }

    #endregion

    public float GetDamage() {
        return damageToDeal;
    }

    public GameObject GetOwner() {
        return owner;
    }
}