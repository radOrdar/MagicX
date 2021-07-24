using System.Collections.Generic;
using Mirror;
using UnityEngine;
using UnityEngine.SceneManagement;

/*
	Documentation: https://mirror-networking.gitbook.io/docs/components/network-manager
	API Reference: https://mirror-networking.com/docs/api/Mirror.NetworkManager.html
*/

public class MyNetworkManager : NetworkManager {
    [SerializeField] public GameObject[] charPrefabs;

    public List<MagicPlayer> Players { get; } = new List<MagicPlayer>();

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
            if (player.ChosenCharacter == -1) {
                Debug.Log(player.DisplayName + " " + player.ChosenCharacter);
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
                    charPrefabs[player.ChosenCharacter],
                    GetStartPosition().position,
                    Quaternion.identity);

                NetworkServer.Spawn(characterInstance, player.connectionToClient);
            }
        }
    }

    public override void OnStopClient() {
        Players.Clear();
    }
}