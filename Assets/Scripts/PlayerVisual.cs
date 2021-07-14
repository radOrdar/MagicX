using System.Collections;
using UnityEngine;
using UnityEngine.UI;

public class PlayerVisual : MonoBehaviour {

    [SerializeField] private SpriteRenderer spriteRenderer;
    [SerializeField] private Health health;
    [SerializeField] private Image healthBarImage;

    private Color defaultColor;
    private bool attackColorCurrentlyChanging;

    private void OnEnable() {
        health.ClientOnHealthUpdated += HandleHealthUpdated;
        defaultColor = spriteRenderer.color;
    }

    private void OnDestroy() {
        health.ClientOnHealthUpdated -= HandleHealthUpdated;
    }

    private void HandleHealthUpdated(float currentHealth, float maxHealth) {
        Debug.Log("healthUpdated");
        healthBarImage.fillAmount = currentHealth / maxHealth;
        if (!attackColorCurrentlyChanging) {
            StartCoroutine(ChangeColorRoutine());
        }
    }

    private IEnumerator ChangeColorRoutine() {
        attackColorCurrentlyChanging = true;
        spriteRenderer.color = Color.red;
        yield return new WaitForSeconds(.2f);
        spriteRenderer.color = defaultColor;
        attackColorCurrentlyChanging = false;
    }
}
