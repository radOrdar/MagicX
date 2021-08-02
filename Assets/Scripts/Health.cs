using System;
using Cinemachine;
using Mirror;
using UnityEngine;

public class Health : NetworkBehaviour {
    [SerializeField] private int maxHealth = 100;

    [SyncVar(hook = nameof(HandleHealthUpdated))]
    private float currentHealth;
    private CinemachineImpulseSource impulseSource; //TODO not best place to handle impulses 

    public event Action<float, float> ClientOnHealthUpdated;
    public event Action<GameObject, float> ServerOnHealthUpdated;

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

    [Command]
    public void CmdDealDmgNotKillable(float damageAmount) {
        currentHealth = Mathf.Max(currentHealth - damageAmount, 0);
    }

    [Server]
    public void DealDamage(float damageAmount, GameObject attacker) {
        currentHealth = Mathf.Max(currentHealth - damageAmount, 0);
        ServerOnHealthUpdated?.Invoke(attacker, damageAmount);

        if (currentHealth == 0) {
            NetworkServer.Destroy(gameObject);
        }
    }

    #endregion
}