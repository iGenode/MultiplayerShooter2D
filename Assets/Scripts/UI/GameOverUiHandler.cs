using Fusion;
using TMPro;
using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UI;

public class GameOverUiHandler : MonoBehaviour
{
    [Header("Game over conditional text")]
    [SerializeField]
    private TextMeshProUGUI _winLooseText;

    [Header("Main menu button")]
    [SerializeField]
    private Button _mainMenu;

    // Making sure that this component subscribes to GameOverEvent even if disabled
    public GameOverUiHandler()
    {
        Debug.Log("Subscribing to GameOverEvent in GameOverUIHandler");
        GameManager.GameOverEvent += ShowGameOverUIForPlayer;
    }

    private void ShowGameOverUIForPlayer(GameObject player)
    {
        var networkPlayerRef = player.GetComponent<NetworkPlayer>();

        // TODO: main menu navigation
        //_mainMenu.onClick.AddListener(delegate ()
        //{
        //    networkPlayerRef.Runner.SetActiveScene(SceneManager.GetSceneByBuildIndex(0).name);
        //});

        Debug.Log("Showing gameOver UI");
        // Activating GameOver UI
        gameObject.SetActive(true);
        // Setting win/loose text
        if (networkPlayerRef == NetworkPlayer.Local)
        {
            _winLooseText.text = "You win!";
        } 
        else
        {
            _winLooseText.text = "You loose!";
        }
    }

    private void OnDisable()
    {
        Debug.Log("Unsubscribing from GameOverEvent in GameOverUIHandler");
        GameManager.GameOverEvent -= ShowGameOverUIForPlayer;
    }
}
