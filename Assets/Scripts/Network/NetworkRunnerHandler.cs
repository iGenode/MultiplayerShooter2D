using Fusion;
using Fusion.Sockets;
using System;
using System.Linq;
using System.Threading.Tasks;
using UnityEngine;
using UnityEngine.SceneManagement;

public class NetworkRunnerHandler : MonoBehaviour
{
    [SerializeField]
    private NetworkRunner _networkRunnerPrefab;

    private NetworkRunner _networkRunner;

    private void Awake()
    {
        var networkRunnerInScene = FindObjectOfType<NetworkRunner>();

        if (networkRunnerInScene != null)
        {
            _networkRunner = networkRunnerInScene;
        }
    }

    private void Start()
    {
        if (_networkRunner == null)
        {
            _networkRunner = Instantiate(_networkRunnerPrefab);
            _networkRunner.name = "Network Runner";

            if (SceneManager.GetActiveScene().name != "Lobby")
            {
                // Is this for when the game starts on game scene instead of lobby?
                // TODO: change to actual name of session
                var clientTask = InitializeNetworkRunner(_networkRunner, GameMode.AutoHostOrClient, "TestSession", NetAddress.Any(), SceneManager.GetActiveScene().buildIndex, true, null);
            }

            var gameOverCondition = new LastManStandingCondition();
            var gameManager = new GameManager(gameOverCondition);
            // TODO: not removing this currenlty
            PlayerList.PlayerListChanged += _ => gameOverCondition.CheckCondition();
            //GameManager.GameOverEvent += delegate (GameObject test) { Debug.Log("GAME OVER!"); };

            Debug.Log("Server NetworkRunner started");
        }
    }

    protected virtual Task InitializeNetworkRunner(NetworkRunner runner, GameMode gameMode, string sessionName, NetAddress address, SceneRef scene, bool shouldHost, Action<NetworkRunner> onInitialized)
    {
        var sceneManager = runner.GetComponents(typeof(MonoBehaviour)).OfType<INetworkSceneManager>().FirstOrDefault();

        sceneManager ??= runner.gameObject.AddComponent<NetworkSceneManagerDefault>();

        runner.ProvideInput = true;

        return runner.StartGame(new StartGameArgs
        {
            GameMode = gameMode,
            Address = address,
            Scene = scene,
            SessionName = sessionName,
            DisableClientSessionCreation = shouldHost,
            Initialized = onInitialized,
            SceneManager = sceneManager
        });
    }

    public void CreateGame(string sessionName, string sceneName)
    {
        Debug.Log($"Create session {sessionName} scene {sceneName}");

        // Create a game as a host
        var clientTask = InitializeNetworkRunner(
            _networkRunner,
            GameMode.Host,
            sessionName,
            NetAddress.Any(),
            SceneUtility.GetBuildIndexByScenePath($"scenes/{sceneName}"),
            true,
            null
        );
    }

    public void JoinGame(string sessionName)
    {
        Debug.Log($"Join session {sessionName}");

        // Join an existing game as a client
        var clientTask = InitializeNetworkRunner(
            _networkRunner,
            GameMode.Client,
            sessionName,
            NetAddress.Any(),
            SceneManager.GetActiveScene().buildIndex,
            false,
            null
        );
    }

    // Join by sessionInfo      // Not used because it requires lobbies and searching for sessions
    //public void JoinGame(SessionInfo sessionInfo)
    //{
    //    Debug.Log($"Join session {sessionInfo.Name}");

    //    // Join an existing game as a client
    //    var clientTask = InitializeNetworkRunner(
    //        _networkRunner,
    //        GameMode.Client,
    //        sessionInfo.Name,
    //        NetAddress.Any(),
    //        SceneManager.GetActiveScene().buildIndex,
    //        null
    //    );
    //}
}
