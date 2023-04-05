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

    private void Start()
    {
        _networkRunner = Instantiate(_networkRunnerPrefab);
        _networkRunner.name = "Network Runner";

        var clientTask = InitializeNetworkRunner(_networkRunner, GameMode.AutoHostOrClient, NetAddress.Any(), SceneManager.GetActiveScene().buildIndex, null);

        Debug.Log("Server NetworkRunner started");
    }

    protected virtual Task InitializeNetworkRunner(NetworkRunner runner, GameMode gameMode, NetAddress address, SceneRef scene, Action<NetworkRunner> onInitialized)
    {
        var sceneManager = runner.GetComponents(typeof(MonoBehaviour)).OfType<INetworkSceneManager>().FirstOrDefault();

        sceneManager ??= runner.gameObject.AddComponent<NetworkSceneManagerDefault>();

        runner.ProvideInput = true;

        return runner.StartGame(new StartGameArgs
        {
            GameMode = gameMode,
            Address = address,
            Scene = scene,
            SessionName = "ChangeMe",
            Initialized = onInitialized,
            SceneManager = sceneManager
        });
    }
}
