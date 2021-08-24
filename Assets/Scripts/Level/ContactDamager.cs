using Mirror;
using UnityEngine;

public class ContactDamager : MonoBehaviour {
    [SerializeField] private int damagePerSecondToDeal = 30;

    [ServerCallback]
    private void OnTriggerStay2D(Collider2D other) {
        if (other.TryGetComponent(out Health health)) {
            health.DealDamage(damagePerSecondToDeal * Time.deltaTime, gameObject);
        }
    }
}