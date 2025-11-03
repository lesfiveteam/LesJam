using FishingHim.Common;
using System.Collections;
using UnityEngine;
using UnityEngine.UI;

[RequireComponent(typeof(Animator), typeof(Button))]
public class Fisherman : MonoBehaviour
{
    private ProgressView _progressView;

    private const string ANIMATION_NAME = "Hook_yank";
    private float _maxTimeOffset = 5f;

    private Animator _animator;
    private Transform _hook;
    private Button _button;

    private void Awake()
    {
        _animator = GetComponent<Animator>();
        _button = GetComponent<Button>();

        _button.onClick.AddListener(PlaySound);
        _button.onClick.AddListener(CatchFish);
    }

    private void Start()
    {
        _hook = GetComponentInChildren<Hook>().transform;

        _progressView = FindFirstObjectByType<ProgressView>();

        StartCoroutine(StartAnimation(Random.Range(0, _maxTimeOffset)));
    }

    private IEnumerator StartAnimation(float time)
    {
        yield return new WaitForSeconds(time);

        _animator.Play(ANIMATION_NAME);
    }

    private void PlaySound()
    {
        SoundsManager.Instance.PlaySound(SoundType.MainClick);
    }

    private void CatchFish()
    {
        _progressView.FishToHook(_hook);
    }
}
