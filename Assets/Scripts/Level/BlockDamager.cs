using System.Collections.Generic;
using Mirror;
using UnityEngine;

public class BlockDamager : MonoBehaviour {
    [SerializeField] private float dmgMultiplier = 0.1f;
    private Rigidbody2D rb;
    private List<ContactPoint2D> contactPoint2Ds = new List<ContactPoint2D>(2);

    private void Start() {
        rb = GetComponent<Rigidbody2D>();
        rb.mass = transform.lossyScale.x * transform.lossyScale.y * 10;
    }

    [ServerCallback]
    private void OnCollisionEnter2D(Collision2D other) {
        if (!other.collider.CompareTag("Player")) { return; }

        if (!other.gameObject.TryGetComponent(out Health health)) { return; }

        if (other.GetContacts(contactPoint2Ds) != 1) { return; }

        // Debug.Log(other.contacts.Length);
        // Debug.Log(other.relativeVelocity.magnitude);
        if (contactPoint2Ds[0].normal.y > 0) {
            health.DealDamage(other.relativeVelocity.magnitude * rb.mass * dmgMultiplier);
        }

        Physics2D.Raycast(transform.position, transform.forward);
    }
}