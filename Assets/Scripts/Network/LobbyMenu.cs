using System.Collections.Generic;
using Mirror;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

public class LobbyMenu : MonoBehaviour {
    [SerializeField] private Button startGameButton;
    [SerializeField] private TMP_Text[] playerNameTexts = new TMP_Text[2];

    private void Start() {
        MagicPlayer.AuthorityOnPartyOwnerStateUpdated += AuthorityHandlePartyOwnerStateUpdated;
        MagicPlayer.ClientOnInfoUpdated += ClientHandleInfoUpdated;
    }

    private void OnDestroy() {
        MagicPlayer.AuthorityOnPartyOwnerStateUpdated -= AuthorityHandlePartyOwnerStateUpdated;
        MagicPlayer.ClientOnInfoUpdated -= ClientHandleInfoUpdated;
    }

    private void AuthorityHandlePartyOwnerStateUpdated(bool state) {
        startGameButton.gameObject.SetActive(state);
    }

    private void ClientHandleInfoUpdated() {
        List<MagicPlayer> players = ((MyNetworkManager) NetworkManager.singleton).Players;

        for (int i = 0; i < players.Count; i++) {
            playerNameTexts[i].text = players[i].DisplayName;
        }

        for (int i = players.Count; i < playerNameTexts.Length; i++) {
            playerNameTexts[i].text = "Waiting For Player...";
        }

        startGameButton.interactable = players.Count >= 2;
    }

    public void StartGame() {
        NetworkClient.connection.identity.GetComponent<MagicPlayer>().CmdStartGame();
    }

    public void SelectCharacter(int val) {
        Debug.Log(val);
        NetworkClient.connection.identity.GetComponent<MagicPlayer>().CmdChoseCharacter(val - 1);
    }
}