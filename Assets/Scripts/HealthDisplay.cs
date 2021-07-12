using UnityEngine;
using UnityEngine.UI;

public class HealthDisplay : MonoBehaviour {
   
    [SerializeField] private Health health;
    [SerializeField] private Image healthBarImage;

    private void Awake() {
        health.ClientOnHealthUpdated += HandleHealthUpdated;
    }

    private void OnDestroy() {
        health.ClientOnHealthUpdated -= HandleHealthUpdated;
    }

    private void HandleHealthUpdated(int currentHealth, int maxHealth) {
        healthBarImage.fillAmount = (float) currentHealth / maxHealth;
    }
}