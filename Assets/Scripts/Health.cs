using System;
using Cinemachine;
using Mirror;
using UnityEngine;

public class Health : NetworkBehaviour {
    [SerializeField] private int maxHealth = 100;

    [SyncVar(hook = nameof(HandleHealthUpdated))]
    private float currentHealth;
    private CinemachineImpulseSource impulseSource; 
    
    public event Action<float, float> ClientOnHealthUpdated;

    public override void OnStartAuthority() {
        base.OnStartAuthority();
        impulseSource = GetComponent<CinemachineImpulseSource>();
    }

    private void HandleHealthUpdated(float oldValue, float newValue) {
        ClientOnHealthUpdated?.Invoke(newValue, maxHealth);
        if (impulseSource != null) {
            impulseSource.GenerateImpulse();
        }
    }

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
}
