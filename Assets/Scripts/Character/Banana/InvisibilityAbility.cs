using System.Collections;
using Mirror;
using UnityEngine;

public class InvisibilityAbility : BaseDurationAbility {
    [SerializeField] private CanvasGroup[] canvasGroupsToHide;
    [SerializeField] private SpriteRenderer[] renderersToHide;
    [SerializeField] private float partialTransparency;
    [SerializeField] private float onEnemyFadeOutDuration;

    private float[] defaultRendererTransparancies;
    private bool isOwner = false;

    [Client]
    private void Start() {
        defaultRendererTransparancies = new float[renderersToHide.Length];
        for (int i = 0; i < renderersToHide.Length; i++) {
            defaultRendererTransparancies[i] = renderersToHide[i].color.a;
        }
    }

    public override void OnStartAuthority() {
        base.OnStartAuthority();
        isOwner = true;
    }

    [Command]
    private void CmdInvisForEnemy() {
        ClientInvis();
    }

    [ClientRpc]
    private void ClientInvis() {
        if (isOwner) { return; }

        StartCoroutine(EnemyInvisRoutine());
    }

    private IEnumerator EnemyInvisRoutine() {
        float startTime = Time.time;
        while (Time.time - startTime < onEnemyFadeOutDuration) {
            float transValue = 1 - (Time.time - startTime) / onEnemyFadeOutDuration;
            ChangeOpacityToAllVisibleElem(transValue);
            yield return null;
        }

        ChangeOpacityToAllVisibleElem(0);
        yield return new WaitForSeconds(durationTime - onEnemyFadeOutDuration);
        ChangeOpacityToAllVisibleElem(1);
    }

    protected override void TriggerAction() {
        base.TriggerAction();
        CmdInvisForEnemy();
        StartCoroutine(OwnerInvisRoutine());
    }

    private IEnumerator OwnerInvisRoutine() {
        float startTime = Time.time;
        while (Time.time - startTime < onEnemyFadeOutDuration) {
            float transValue = 1.5f - (Time.time - startTime) / onEnemyFadeOutDuration;
            ChangeOpacityToAllVisibleElem(transValue);
            yield return null;
        }

        ChangeOpacityToAllVisibleElem(partialTransparency);

        yield return new WaitForSeconds(durationTime - onEnemyFadeOutDuration);

        ChangeOpacityToAllVisibleElem(1);
    }

    private void ChangeOpacityToAllVisibleElem(float newVal) {
        foreach (var canvasGroup in canvasGroupsToHide) {
            canvasGroup.alpha = newVal;
        }

        for (int i = 0; i < renderersToHide.Length; i++) {
            var tempColor = renderersToHide[i].color;
            tempColor.a = newVal * defaultRendererTransparancies[i];
            renderersToHide[i].color = tempColor;
        }

        // foreach (var spriteRenderer in renderersToHide) {
        //     var tempColor = spriteRenderer.color;
        //     tempColor.a = newVal;
        //     spriteRenderer.color = tempColor;
        // }
    }

    // private IEnumerator CooldownIndicatorRoutine() {
    //     cooldownIndicator.fillAmount = 0;
    //     cooldownIndicator.gameObject.SetActive(true);
    //     float startTime = Time.time;
    //     while (Time.time - startTime < coolDownTime) {
    //         cooldownIndicator.fillAmount = (Time.time - startTime) / coolDownTime;
    //         yield return null;
    //     }
    //
    //     cooldownIndicator.gameObject.SetActive(false);
    // }
}