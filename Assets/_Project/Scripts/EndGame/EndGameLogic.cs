using FishingHim.Common;
using UnityEngine;
using UnityEngine.UI;

public class EndGameLogic : MonoBehaviour
{
    private Image _background;

    [SerializeField] private Sprite _winSprite;
    [SerializeField] private Sprite _defeatSprite;

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
        
        _currentSprite = ProgressManager.instance.IsWin ? _winSprite : _defeatSprite;

        _background.sprite = _currentSprite;
    }

    public void Restart()
    {
        ProgressManager.instance.RestartGame();
    }
}
