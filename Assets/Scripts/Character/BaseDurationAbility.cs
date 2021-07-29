using System.Collections;
using Mirror;
using UnityEngine;

public abstract class BaseDurationAbility : BaseCooldownAbility {
    [SerializeField] protected float durationTime;
    [SerializeField] private SpriteRenderer durationVisualRenderer;

    [field: SyncVar] public bool IsActive { get; private set; }

    private IEnumerator DurationRoutine() {
        IsActive = true;
        yield return new WaitForSeconds(durationTime);
        IsActive = false;
    }

    private IEnumerator DurationVisualRoutine() {
        durationVisualRenderer.gameObject.SetActive(true);
        yield return new WaitForSeconds(durationTime);
        durationVisualRenderer.gameObject.SetActive(false);
    }

    [ClientRpc]
    private void ClientDurationVisualRoutine() {
        StartCoroutine(nameof(DurationVisualRoutine));
    }

    protected override void UseAllBaseRoutines() {
        base.UseAllBaseRoutines();
        StartCoroutine(nameof(DurationRoutine));
        if (durationVisualRenderer != null) {
            ClientDurationVisualRoutine();
        } else {
            Debug.Log("durationVisualRenderer is null");
        }
    }
}