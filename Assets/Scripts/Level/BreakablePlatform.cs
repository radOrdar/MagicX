using System.Collections;
using Mirror;
using UnityEngine;

public class BreakablePlatform : NetworkBehaviour {
    [Tooltip("Should be NOT zero length. Otherwise I'll fight you and I'll kill you!")]
    [SerializeField] private Color32[] visualStates;

    [SyncVar(hook = nameof(HandleCurrentState))]
    private int currentState;
    private SpriteRenderer spriteRenderer;

    private ContactPoint2D[] contactPoint2Ds = new ContactPoint2D[1];
    private bool exhausted = false;

    private void Start() {
        spriteRenderer = GetComponent<SpriteRenderer>();
        spriteRenderer.color = visualStates[0];
    }

    [ServerCallback]
    private void OnCollisionEnter2D(Collision2D other) {
        if (!other.collider.CompareTag("Player")) { return; }

        other.GetContacts(contactPoint2Ds);
        if (exhausted || contactPoint2Ds[0].normal.y >= 0) { return; }

        if (currentState >= visualStates.Length - 1) {
            exhausted = true;
            StartCoroutine(DestroyRoutine(.2f));
            return;
        }

        currentState++;
    }

    private void HandleCurrentState(int oldVal, int newVal) {
        spriteRenderer.color = visualStates[newVal];
    }

    [Server]
    private IEnumerator DestroyRoutine(float delay) {
        yield return new WaitForSeconds(delay);
        NetworkServer.Destroy(gameObject);
    }
}