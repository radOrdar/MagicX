using System.Collections;
using UnityEngine;
using UnityEngine.UI;

public class CharacterVisual : MonoBehaviour {
    [SerializeField] private Image healthBarImage;
    [SerializeField] private Material _flashMaterial;

    private SpriteRenderer spriteRenderer;
    private Health health;
    private Material defaultMaterial;
    private bool isColorBeingChanging;

    private void OnEnable() {
        health = GetComponent<Health>();
        spriteRenderer = GetComponent<SpriteRenderer>();
        defaultMaterial = spriteRenderer.material;
        health.ClientOnHealthUpdated += HandleHealthUpdated;
    }

    private void OnDestroy() {
        health.ClientOnHealthUpdated -= HandleHealthUpdated;
    }

    private void HandleHealthUpdated(float currentHealth, float maxHealth) {
        healthBarImage.fillAmount = currentHealth / maxHealth;
        if (!isColorBeingChanging) {
            StartCoroutine(ChangeColorRoutine());
        }
    }

    private IEnumerator ChangeColorRoutine() {
        isColorBeingChanging = true;
        spriteRenderer.material = _flashMaterial;
        yield return new WaitForSeconds(.2f);
        spriteRenderer.material = defaultMaterial;
        isColorBeingChanging = false;
    }
}