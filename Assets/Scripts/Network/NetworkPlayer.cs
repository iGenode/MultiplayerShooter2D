using Fusion;
using TMPro;
using UnityEngine;

public class NetworkPlayer : NetworkBehaviour, IPlayerLeft
{
    public static NetworkPlayer Local { get; set; }

    [Header("Player UI")]
    [SerializeField]
    private GameObject _mobileInputUI;
    //[SerializeField]
    //private GameObject _messagesUi;

    [Header("Player nickname")]
    //[SerializeField]
    //private PlayerData _playerData;
    [SerializeField]
    private TextMeshProUGUI _playerNicknameText;

    //private bool _didSendJoinMessage = false;
    //private NetworkInGameMessages _networkInGameMessages;

    private string _nickname;

    [Networked(OnChanged = nameof(OnNicknameChanged))]
    [HideInInspector]
    public NetworkString<_16> Nickname { get; set; }

    //private void Awake()
    //{
    //    // TODO: move to a separate object
    //    _networkInGameMessages = GetComponent<NetworkInGameMessages>();
    //}

    public override void Spawned()
    {
        base.Spawned();

        if (Object.HasInputAuthority)
        {
            // Logic for local player
            Local = this;

            //RPC_SetNickname(_playerData.Nickname);
            _nickname = PlayerPrefs.GetString("PlayerNickname");

            RPC_SetNickname(_nickname);

            // Removing UI controls from player to fix input lag while changin player position
            _mobileInputUI.transform.SetParent(null, true);
            // Disabling UI controls until the game starts, unless it's already started
            if (!GameManager.IsGameStarted)
            {
                _mobileInputUI.SetActive(false);
                GameManager.GameStartedEvent += () => _mobileInputUI.SetActive(true);
                GameManager.GameOverEvent += _ => _mobileInputUI.SetActive(false);
            }

            //if (!_didSendJoinMessage)
            //{
            //    _networkInGameMessages.SendInGameRPCMessage(_nickname, "joined");

            //    _didSendJoinMessage = true;
            //}

            Debug.Log("Spawned local player");
        }
        else
        {
            // Logic for remote player
            // Hide controls of other players
            _mobileInputUI.SetActive(false);
            //// Hide messages of other players
            //_messagesUi.SetActive(false);

            Debug.Log("Spawned remote player");
        }

        // Set the player as a player object
        Runner.SetPlayerObject(Object.InputAuthority, Object);

        gameObject.name = $"Player {Nickname}";

        PlayerList.AddToList(gameObject);
    }

    public void PlayerLeft(PlayerRef player)
    {
        // If client is still on the server (has input authority) - despawn the player that left
        if (player.IsValid == Object.HasInputAuthority)
        {
            Runner.Despawn(Object);
        }
        if (Object.HasStateAuthority)
        {
            if (Runner.TryGetPlayerObject(player, out NetworkObject playerLeft))
            {
                Debug.Log($"In PlayerLeft, got {playerLeft} from TryGetPlayerObject");
                if (playerLeft == Object)
                {
                    // TODO: send message
                }
                // TODO: this might be wrond, test and clear this comment
                PlayerList.RemoveFromList(playerLeft.gameObject);
            }
        }
    }

    static void OnNicknameChanged(Changed<NetworkPlayer> changed)
    {
        Debug.Log($"@{Time.time} OnNicknameChanged value {changed.Behaviour.Nickname}");

        changed.Behaviour.OnNicknameChanged();
    }

    private void OnNicknameChanged()
    {
        Debug.Log($"Nickname changed to {Nickname} for player {gameObject.name}");

        _playerNicknameText.text = Nickname.ToString();
    }

    // Rpc from input authority to state authority for setting the nickname on the server
    [Rpc(RpcSources.InputAuthority, RpcTargets.StateAuthority)]
    public void RPC_SetNickname(string nickname, RpcInfo info = default)
    {
        Debug.Log($"[RPC] SetNickname {nickname}");
        this.Nickname = nickname;
    }
}
