using System.Collections;
using Mirror;
using UnityEngine;
using UnityEngine.UI;

public class ReturnDamage : NetworkBehaviour {
    [SerializeField] private float durationTime;
    [SerializeField] private float coolDownTime;

    [SerializeField] private Image cooldownIndicator;
    [SerializeField] private SpriteRenderer shieldRenderer;

    public InputType ShootInputVal { private get; set; } = InputType.None;

    private bool isActive = false;
    [SyncVar] private bool isReady = true;

    [ClientCallback]
    private void Update() {
        if (ShootInputVal != InputType.Canceled) { return; }

        ShootInputVal = InputType.None;
        if (isReady) {
            CmdActivate();
        }
    }

    [ServerCallback]
    private void OnTriggerEnter2D(Collider2D other) {
        Debug.Log("Trigger");
        if (!isActive) { return; }

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

    [ClientRpc]
    private void ClientReloadIndicatorRoutine() {
        StartCoroutine(nameof(ReloadIndicatorRoutine));
    }

    [ClientRpc]
    private void ClientShieldVisualRoutine() {
        StartCoroutine(nameof(ShieldVisualRoutine));
    }

    [Command]
    private void CmdActivate() {
        if (!isReady) { return; }

        ClientReloadIndicatorRoutine();
        ClientShieldVisualRoutine();
        StartCoroutine(nameof(DurationRoutine));
        StartCoroutine(nameof(CoolDownRoutine));
    }

    private IEnumerator DurationRoutine() {
        isActive = true;
        yield return new WaitForSeconds(durationTime);
        isActive = false;
    }

    private IEnumerator CoolDownRoutine() {
        isReady = false;
        yield return new WaitForSeconds(coolDownTime);
        isReady = true;
    }

    private IEnumerator ReloadIndicatorRoutine() {
        cooldownIndicator.fillAmount = 0;
        cooldownIndicator.gameObject.SetActive(true);
        float startTime = Time.time;
        while (Time.time - startTime < coolDownTime) {
            cooldownIndicator.fillAmount = (Time.time - startTime) / coolDownTime;
            yield return null;
        }

        cooldownIndicator.gameObject.SetActive(false);
    }

    private IEnumerator ShieldVisualRoutine() {
        shieldRenderer.gameObject.SetActive(true);
        yield return new WaitForSeconds(durationTime);
        shieldRenderer.gameObject.SetActive(false);
    }
}