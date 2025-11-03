using UnityEngine;
using System.Collections;

[RequireComponent(typeof(Animator))]
public class Fisherman : MonoBehaviour
{
    private const string ANIMATION_NAME = "Hook_yank";
    private float _maxTimeOffset = 5f;

    private Animator _animator;

    private void Start()
    {
        _animator = GetComponent<Animator>();

        StartCoroutine(StartAnimation(Random.Range(0, _maxTimeOffset)));
    }

    private IEnumerator StartAnimation(float time)
    {
        yield return new WaitForSeconds(time);

        _animator.Play(ANIMATION_NAME);
    }
}
