using TMPro;
using UnityEngine;

public class WaitingForPlayersUIHandler : MonoBehaviour
{
    [SerializeField]
    private TextMeshProUGUI _waitingText;

    private bool _isIncreasing;
    private Color _alphaIncrement = new(0f, 0f, 0f, 0.001f);

    void Start()
    {
        GameManager.GameStartedEvent += HideSelf;
    }

    void Update()
    {
        if (_waitingText.color.a <= 0.2)
        {
            _isIncreasing = true;
        } 
        else if (_waitingText.color.a >= 1)
        {
            _isIncreasing = false;
        }
        
        if (_isIncreasing)
        {
            _waitingText.color += _alphaIncrement;
        } 
        else
        {
            _waitingText.color -= _alphaIncrement;
        }
    }

    private void HideSelf()
    {
        gameObject.SetActive(false);
    }

    private void OnDisable()
    {
        GameManager.GameStartedEvent -= HideSelf;
    }
}
