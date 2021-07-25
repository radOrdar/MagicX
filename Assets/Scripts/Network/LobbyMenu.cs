using System.Collections.Generic;
using System.Linq;
using Mirror;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

public class LobbyMenu : MonoBehaviour {
    [SerializeField] private Button startGameButton;
    [SerializeField] private GameObject lobbyUi;
    [SerializeField] private TMP_Text[] playerNameTexts = new TMP_Text[2];
    [SerializeField] private TMP_Text[] playerHeroTexts = new TMP_Text[2];

    private void Start() {
        MagicPlayer.AuthorityOnPartyOwnerStateUpdated += AuthorityHandlePartyOwnerStateUpdated;
        MagicPlayer.ClientOnInfoUpdated += ClientHandleInfoUpdated;
        MagicNetworkManager.OnClientConnected += ClientHandleClientConnected;
    }

    private void OnDestroy() {
        MagicPlayer.AuthorityOnPartyOwnerStateUpdated -= AuthorityHandlePartyOwnerStateUpdated;
        MagicPlayer.ClientOnInfoUpdated -= ClientHandleInfoUpdated;
        MagicNetworkManager.OnClientConnected -= ClientHandleClientConnected;
    }

    private void AuthorityHandlePartyOwnerStateUpdated(bool state) {
        startGameButton.gameObject.SetActive(state);
    }

    private void ClientHandleInfoUpdated() {
        List<MagicPlayer> players = ((MagicNetworkManager) NetworkManager.singleton).Players;
        for (int i = 0; i < players.Count; i++) {
            playerNameTexts[i].text = players[i].DisplayName;
            playerHeroTexts[i].text = players[i].chosenCharacterType == MagicPlayer.CharacterType.None ? "-" : players[i].chosenCharacterType.ToString();
        }

        for (int i = players.Count; i < playerNameTexts.Length; i++) {
            playerNameTexts[i].text = "Waiting For Player...";
            playerHeroTexts[i].text = "-";
        }

        startGameButton.interactable = players.Count >= 2 && !players.Any(p => p.chosenCharacterType < 0);
    }

    private void ClientHandleClientConnected() {
        lobbyUi.SetActive(true);
    }

    public void StartGame() {
        NetworkClient.connection.identity.GetComponent<MagicPlayer>().CmdStartGame();
    }

    public void SelectCharacter(int val) {
        NetworkClient.connection.identity.GetComponent<MagicPlayer>().CmdChoseCharacter(val - 1);
    }
}