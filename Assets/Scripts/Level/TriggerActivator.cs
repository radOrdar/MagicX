using System.Collections;
using Mirror;
using UnityEngine;
using UnityEngine.Events;

public class TriggerActivator : NetworkBehaviour {
    [SerializeField] private float cooldownTime;

    public UnityEvent OnTrigger;

    private float nextReadyTime;

    private SpriteRenderer spriteRenderer;
    private Color32 defaultColor;

    public override void OnStartClient() {
        spriteRenderer = GetComponent<SpriteRenderer>();
        defaultColor = spriteRenderer.color;
    }

    [ServerCallback]
    private void OnTriggerEnter2D(Collider2D other) {
        if (!other.CompareTag("Player")) { return; }

        if (nextReadyTime > Time.time) { return; }

        OnTrigger?.Invoke();
        nextReadyTime = Time.time + cooldownTime;
        RpcHandleCooldown();
    }

    [ClientRpc]
    private void RpcHandleCooldown() {
        StartCoroutine(CooldownRoutine());
    }

    private IEnumerator CooldownRoutine() {
        spriteRenderer.color = Color.black;
        yield return new WaitForSeconds(cooldownTime);
        spriteRenderer.color = defaultColor;
    }
}