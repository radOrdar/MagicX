using System;
using Mirror;
using UnityEngine;

public class MagicPlayer : NetworkBehaviour {
    [SyncVar(hook = nameof(AuthorityHandlePartyOwnerStateUpdated))]
    public bool isPartyOwner;

    [field: SyncVar] public int ChosenCharacter { get; set; } = -1;

    [field: SyncVar(hook = nameof(ClientHandleDisplayNameUpdated))]
    public string DisplayName { get; [Server] set; }

    public static event Action ClientOnInfoUpdated;
    public static event Action<bool> AuthorityOnPartyOwnerStateUpdated;

    public override void OnStartServer() {
        DontDestroyOnLoad(gameObject);
    }

    [Server]
    public void SetPartyOwner(bool state) {
        isPartyOwner = state;
    }

    [Command]
    public void CmdStartGame() {
        if (!isPartyOwner) { return; }

        ((MyNetworkManager) NetworkManager.singleton).StartGame();
    }

    [Command]
    public void CmdChoseCharacter(int val) {
        if (val < 0 || val > ((MyNetworkManager) NetworkManager.singleton).charPrefabs.Length - 1) {
            Debug.Log("Error: wrong character number chosen");
            return;
        }

        ChosenCharacter = val;
    }

    private void ClientHandleDisplayNameUpdated(string oldDisplayName, string newDisplayName) {
        ClientOnInfoUpdated?.Invoke();
    }

    private void AuthorityHandlePartyOwnerStateUpdated(bool oldState, bool newState) {
        if (!hasAuthority) { return; }

        AuthorityOnPartyOwnerStateUpdated?.Invoke(newState);
    }
}