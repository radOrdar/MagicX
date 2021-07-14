using System;
using Mirror;
using UnityEngine;

public class Health : NetworkBehaviour {
    [SerializeField] private int maxHealth = 100;

    [SyncVar(hook = nameof(HandleHealthUpdated))]
    private float currentHealth;

    public event Action<float, float> ClientOnHealthUpdated;

    #region Server

    public override void OnStartServer() {
        currentHealth = maxHealth;
    }

    [Server]
    public void DealDamage(float damageAmount) {
        if (currentHealth == 0) { return; }

        currentHealth = Mathf.Max(currentHealth - damageAmount, 0);

        if (currentHealth == 0) {
            NetworkServer.Destroy(gameObject);
        }
    }

    #endregion

    #region Client

    private void HandleHealthUpdated(float oldValue, float newValue) {
        ClientOnHealthUpdated?.Invoke(newValue, maxHealth);
    }

    #endregion
}
