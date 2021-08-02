using Mirror;
using UnityEngine;

public class DamagerPlatform : MonoBehaviour {
    [SerializeField] private int damagePerSecondToDeal = 30;

    [ServerCallback]
    private void OnCollisionStay2D(Collision2D other) {
        if (other.gameObject.TryGetComponent(out Health health)) {
            health.DealDamage(damagePerSecondToDeal * Time.deltaTime, gameObject);
        }
    }
}