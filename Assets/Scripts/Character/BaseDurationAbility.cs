using System.Collections;
using Mirror;
using UnityEngine;

public abstract class BaseDurationAbility : BaseCooldownAbility {
    [SerializeField] private float durationTime;
    [SerializeField] private SpriteRenderer durationVisualRenderer;

    [field: SyncVar] public bool IsActive { get; private set; }

    protected IEnumerator DurationRoutine() {
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
    private void ClientShieldVisualRoutine() {
        StartCoroutine(nameof(DurationVisualRoutine));
    }

    protected override void UseAllBaseRoutines() {
        base.UseAllBaseRoutines();
        ClientShieldVisualRoutine();
        StartCoroutine(nameof(DurationRoutine));
    }
}