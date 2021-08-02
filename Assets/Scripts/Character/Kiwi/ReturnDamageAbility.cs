using UnityEngine;

public class ReturnDamageAbility : BaseDurationAbility {
    private Health myHealth;

    public override void OnStartServer() {
        base.OnStartServer();
        myHealth = GetComponent<Health>();
        myHealth.ServerOnHealthUpdated += HandleServerHealthUpdated;
    }

    public override void OnStopServer() {
        base.OnStopServer();
        myHealth.ServerOnHealthUpdated -= HandleServerHealthUpdated;
    }

    private void HandleServerHealthUpdated(GameObject obj, float dmg) {
        if (!IsActive) { return; }

        if (obj.TryGetComponent(out Health health)) {
            health.DealDamage(dmg, gameObject);
        }
    }
}