using TMPro;
using UnityEngine;

public class LobbyUIHandler : MonoBehaviour
{
    [Header("Lobbies")]
    [SerializeField]
    private TMP_InputField _createLobbyInput;
    [SerializeField]
    private TMP_InputField _joinLobbyInput;

    [Header("Player")]
    [SerializeField]
    private TMP_InputField _nicknameInput;

    [Header("Network")]
    [SerializeField]
    private NetworkRunnerHandler _networkRunnerHandler;

    // Start is called before the first frame update
    void Start()
    {
        if (PlayerPrefs.HasKey("PlayerNickname"))
        {
            _nicknameInput.text = PlayerPrefs.GetString("PlayerNickname");
        }
    }

    public void OnJoinGameClicked()
    {
        SaveNickname();
        // Trying to join the session typed by the user
        _networkRunnerHandler.JoinGame(_joinLobbyInput.text);

        // TODO: Hide UI, start loading
    }

    public void OnCreateGameClicked()
    {
        SaveNickname();
        // Trying to create a session
        _networkRunnerHandler.CreateGame(_createLobbyInput.text, "SampleScene");

        // TODO: Hide UI, start loading
    }

    private void SaveNickname()
    {
        PlayerPrefs.SetString("PlayerNickname", _nicknameInput.text);
        PlayerPrefs.Save();
    }
}
