﻿using System.Collections;
using Mirror;
using UnityEngine;
using UnityEngine.UI;

public abstract class BaseCooldownAbility : NetworkBehaviour {
    [SerializeField] private float coolDownTime;
    [SerializeField] private Image cooldownIndicator;

    public InputType InputVal { protected get; set; }

    [SyncVar] protected bool isReady = true;

    [ClientCallback]
    private void Update() {
        if (InputVal != InputType.Canceled) { return; }

        InputVal = InputType.None;
        if (isReady) {
            TriggerAction();
        }
    }

    [Command]
    protected virtual void CmdActivate() {
        UseAllBaseRoutines();
    }

    protected virtual void TriggerAction() {
        CmdActivate();
    }

    protected virtual void UseAllBaseRoutines() {
        ClientReloadIndicatorRoutine();
        StartCoroutine(nameof(CoolDownRoutine));
    }

    [ClientRpc]
    private void ClientReloadIndicatorRoutine() {
        if (cooldownIndicator == null) {
            Debug.Log("cooldownIndicator is null");
            return;
        }

        StartCoroutine(nameof(CooldownIndicatorRoutine));
    }

    private IEnumerator CoolDownRoutine() {
        isReady = false;
        yield return new WaitForSeconds(coolDownTime);
        isReady = true;
    }

    private IEnumerator CooldownIndicatorRoutine() {
        cooldownIndicator.fillAmount = 0;
        cooldownIndicator.gameObject.SetActive(true);
        float startTime = Time.time;
        while (Time.time - startTime < coolDownTime) {
            cooldownIndicator.fillAmount = (Time.time - startTime) / coolDownTime;
            yield return null;
        }

        cooldownIndicator.gameObject.SetActive(false);
    }
}