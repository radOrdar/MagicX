using System.Collections;
using UnityEngine;
using UnityEngine.UI;

public class CharacterVisual : MonoBehaviour {

    [SerializeField] private SpriteRenderer spriteRenderer;
    [SerializeField] private Health health;
    [SerializeField] private Image healthBarImage;
    [SerializeField] private Color32 colorToChangeWhenGainDamage = Color.red;

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
        healthBarImage.fillAmount = currentHealth / maxHealth;
        if (!attackColorCurrentlyChanging) {
            StartCoroutine(ChangeColorRoutine());
        }
    }

    private IEnumerator ChangeColorRoutine() {
        attackColorCurrentlyChanging = true;
        spriteRenderer.color = colorToChangeWhenGainDamage;
        yield return new WaitForSeconds(.2f);
        spriteRenderer.color = defaultColor;
        attackColorCurrentlyChanging = false;
    }
}
