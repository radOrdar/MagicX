using System;
using System.Collections.Generic;
using Mirror;
using UnityEngine;
using UnityEngine.SceneManagement;

public class MagicNetworkManager : NetworkManager {
    [SerializeField] public GameObject[] charPrefabs;

    public List<MagicPlayer> Players { get; } = new List<MagicPlayer>();

    public static event Action OnClientConnected;

    public override void OnServerDisconnect(NetworkConnection conn) {
        MagicPlayer player = conn.identity.GetComponent<MagicPlayer>();

        Players.Remove(player);

        base.OnServerDisconnect(conn);
    }

    public override void OnStopServer() {
        Players.Clear();
    }

    public void StartGame() {
        if (Players.Count < 2) { return; }

        foreach (var player in Players) {
            if (player.chosenCharacterType == MagicPlayer.CharacterType.None) {
                return;
            }
        }

        ServerChangeScene("Ragim 1");
    }

    public override void OnServerAddPlayer(NetworkConnection conn) {
        base.OnServerAddPlayer(conn);

        MagicPlayer player = conn.identity.GetComponent<MagicPlayer>();

        Players.Add(player);

        player.DisplayName = $"Player {Players.Count}";
        player.SetPartyOwner(Players.Count == 1);
    }

    public override void OnServerSceneChanged(string sceneName) {
        if (SceneManager.GetActiveScene().name.StartsWith("Ragim 1")) {
            foreach (MagicPlayer player in Players) {
                GameObject characterInstance = Instantiate(
                    charPrefabs[(int) player.chosenCharacterType],
                    GetStartPosition().position,
                    Quaternion.identity);

                NetworkServer.Spawn(characterInstance, player.connectionToClient);
            }
        }
    }

    public override void OnClientConnect(NetworkConnection conn) {
        base.OnClientConnect(conn);
        OnClientConnected?.Invoke();
    }

    public override void OnClientDisconnect(NetworkConnection conn) {
        SceneManager.LoadScene(offlineScene);
    }

    public override void OnStopClient() {
        Players.Clear();
    }
}