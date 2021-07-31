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
    [SerializeField] private TMP_Text selectedLevelText;
    [SerializeField] private GameObject levelSelectionPanel;
    [SerializeField] private int numOfGameLevels;

    private void Start() {
        MagicPlayer.AuthorityOnPartyOwnerStateUpdated += AuthorityHandlePartyOwnerStateUpdated;
        MagicPlayer.ClientOnInfoUpdated += ClientHandleInfoUpdated;
        MagicPlayer.OnLevelSelected += ClientHandleSelectedLevel;
        MagicNetworkManager.OnClientConnected += ClientHandleClientConnected;

        TMP_Dropdown dropdown = levelSelectionPanel.transform.GetChild(1).GetComponent<TMP_Dropdown>();
        var optionNames = new List<string> {"-"};
        for (int i = 1; i <= numOfGameLevels; i++) {
            optionNames.Add($"GameLevel {i}");
        }

        dropdown.AddOptions(optionNames);
        dropdown.value = 0;
    }

    private void OnDestroy() {
        MagicPlayer.AuthorityOnPartyOwnerStateUpdated -= AuthorityHandlePartyOwnerStateUpdated;
        MagicPlayer.ClientOnInfoUpdated -= ClientHandleInfoUpdated;
        MagicPlayer.OnLevelSelected -= ClientHandleSelectedLevel;
        MagicNetworkManager.OnClientConnected -= ClientHandleClientConnected;
    }

    private void AuthorityHandlePartyOwnerStateUpdated(bool state) {
        startGameButton.gameObject.SetActive(state);
        levelSelectionPanel.gameObject.SetActive(state);
    }

    private void ClientHandleInfoUpdated() {
        List<MagicPlayer> players = ((MagicNetworkManager) NetworkManager.singleton).Players;
        for (int i = 0; i < players.Count; i++) {
            playerNameTexts[i].text = players[i].DisplayName;
            playerHeroTexts[i].text = players[i].chosenCharacterType == CharacterType.None ? "-" : players[i].chosenCharacterType.ToString();
        }

        for (int i = players.Count; i < playerNameTexts.Length; i++) {
            playerNameTexts[i].text = "Waiting For Player...";
            playerHeroTexts[i].text = "-";
        }

        startGameButton.interactable = players.Count >= 2
                                       && !players.Any(p => p.chosenCharacterType < 0)
                                       && selectedLevelText.text.StartsWith("GameLevel");
    }

    private void ClientHandleSelectedLevel(string newLevel) {
        selectedLevelText.text = newLevel;
        List<MagicPlayer> players = ((MagicNetworkManager) NetworkManager.singleton).Players;
        startGameButton.interactable = players.Count >= 2
                                       && !players.Any(p => p.chosenCharacterType < 0)
                                       && selectedLevelText.text.StartsWith("GameLevel");
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

    public void SelectLevel(int val) {
        string selectedLevel = val == 0 ? "-" : $"GameLevel {val}";
        NetworkClient.connection.identity.GetComponent<MagicPlayer>().CmdSelectLevel(selectedLevel);
    }
}