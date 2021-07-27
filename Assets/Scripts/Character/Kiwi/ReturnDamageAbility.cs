using Mirror;
using UnityEngine;

public class ReturnDamageAbility : BaseDurationAbility {
    [ServerCallback]
    private void OnTriggerEnter2D(Collider2D other) {
        if (!IsActive) { return; }

        if (other.TryGetComponent(out NetworkIdentity identity)) {
            if (identity.connectionToClient == connectionToClient) {
                return;
            }
        }

        if (!other.TryGetComponent(out IDamageable damageable)) { return; }

        if (damageable.GetOwner().TryGetComponent(out Health health)) {
            health.DealDamage(damageable.GetDamage());
        }
    }
}