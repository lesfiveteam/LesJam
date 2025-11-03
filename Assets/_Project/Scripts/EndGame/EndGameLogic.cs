using FishingHim.Common;
using UnityEngine;
using UnityEngine.UI;

public class EndGameLogic : MonoBehaviour
{
    private Image _background;

    [SerializeField] private Sprite _winMinigameSprite;
    [SerializeField] private Sprite _defeatMinigameSprite;
    [SerializeField] private Sprite _winGameSprite;
    [SerializeField] private Sprite _defeatGameSprite;

    private Sprite _currentSprite;

    private void Awake()
    {
        _background = GetComponentInChildren<Image>();
    }

    private void Start()
    {
        SetCurrentStateSprite();
    }

    public void SetCurrentStateSprite()
    {
        if (ProgressManager.instance.GetNumberOfAliveFishes() <= 0)
        {
            _currentSprite = ProgressManager.instance.IsWinGame ? _winGameSprite : _defeatGameSprite;
        }
        else
        {
            _currentSprite = ProgressManager.instance.IsWinGame ? _winMinigameSprite : _defeatMinigameSprite;
        }
        
        _background.sprite = _currentSprite;
    }

    public void Restart()
    {
        ProgressManager.instance.RestartGame();
    }
}
