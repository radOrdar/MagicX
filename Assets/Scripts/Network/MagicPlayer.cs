using System;
using Mirror;
using UnityEngine;

public class MagicPlayer : NetworkBehaviour {
    [SyncVar(hook = nameof(AuthorityHandlePartyOwnerStateUpdated))]
    public bool isPartyOwner;

    [field: SyncVar(hook = nameof(ClientHandleChosenCharacter))]
    public CharacterType chosenCharacterType { get; private set; } = CharacterType.None;

    [field: SyncVar(hook = nameof(ClientHandleDisplayNameUpdated))]
    public string DisplayName { get; [Server] set; }

    [field: SyncVar(hook = nameof(ClientHandleSelectedLevel))]
    public string SelectedLevel { get; private set; }

    public static event Action ClientOnInfoUpdated;
    public static event Action<bool> AuthorityOnPartyOwnerStateUpdated;
    public static event Action<string> OnLevelSelected;

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

        ((MagicNetworkManager) NetworkManager.singleton).StartGame();
    }

    [Command]
    public void CmdChoseCharacter(int val) {
        if (val < -1 || val > ((MagicNetworkManager) NetworkManager.singleton).charPrefabs.Length - 1) {
            Debug.LogError("Error: wrong character number chosen");
            return;
        }

        chosenCharacterType = (CharacterType) val;
    }

    [Command]
    public void CmdSelectLevel(string levelName) {
        if (!isPartyOwner) { return; }

        SelectedLevel = levelName;
    }

    private void ClientHandleDisplayNameUpdated(string oldDisplayName, string newDisplayName) {
        ClientOnInfoUpdated?.Invoke();
    }

    private void ClientHandleSelectedLevel(string oldLevel, string newLevel) {
        SelectedLevel = newLevel;
        OnLevelSelected?.Invoke(newLevel);
    }

    private void ClientHandleChosenCharacter(CharacterType oldVal, CharacterType newVal) {
        ClientOnInfoUpdated?.Invoke();
    }

    private void AuthorityHandlePartyOwnerStateUpdated(bool oldState, bool newState) {
        if (!hasAuthority) { return; }

        AuthorityOnPartyOwnerStateUpdated?.Invoke(newState);
    }

    public override void OnStartClient() {
        if (NetworkServer.active) { return; }

        DontDestroyOnLoad(gameObject);

        ((MagicNetworkManager) NetworkManager.singleton).Players.Add(this);
    }

    public override void OnStopClient() {
        ClientOnInfoUpdated?.Invoke();

        if (!isClientOnly) { return; }

        ((MagicNetworkManager) NetworkManager.singleton).Players.Remove(this);
    }
}