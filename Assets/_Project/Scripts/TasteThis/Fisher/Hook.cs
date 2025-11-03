using FishingHim.Common;
using System;
using System.Collections;
using UnityEngine;

namespace FishingHim.TasteThis
{
    public class Hook : MonoBehaviour
    {
        [SerializeField]
        private int _targetFishesNumber = 10;
        [SerializeField]
        GameObject _winPanel, _losePanel;
        [SerializeField]
        Fisher _fisher;
        [SerializeField]
        Transform _bait;
        private Animator _animator;
        private float _animDuration;
        private Collider2D _collider;
        private int _fishesNumber;
        private float _capacity = 2f;
        private AudioSource _audioSource;
        private bool _isGameOver = false;
        private float _waterYLevel = 0.8f;

        public static Hook Instance { get; private set; }

        private void Awake()
        {
            _animator = GetComponent<Animator>();
            _collider = GetComponent<Collider2D>();
            _audioSource = GetComponent<AudioSource>();
            _animDuration = _animator.GetCurrentAnimatorStateInfo(0).length;
            Instance = this;
        }

        public event EventHandler FishCaught;

        public delegate void ItemCaughtHandler(Item item);
        public event ItemCaughtHandler ItemCaught;

        [SerializeField]
        private Transform _tip;

        public Vector2 GetHookPosition() => transform.position.y < _waterYLevel ? _tip.position : _bait.position;
 
        private void OnTriggerEnter2D(Collider2D collision)
        {
            if (collision.TryGetComponent<SmallFish>(out var fish))
            {
                FishCaught?.Invoke(this, EventArgs.Empty);
                Catch(fish);
            }
        }

        public void Catch<T>(T caughtItem) where T : MonoBehaviour, ICatchable
        {
            if (_isGameOver)
                return;

            if (caughtItem is SmallFish)
            {
                _fishesNumber++;

                if (_fishesNumber >= _targetFishesNumber)
                {
                    ProgressManager.instance.Lose();
                    StartCoroutine(GameOver(false));
                }

            }
            else if (caughtItem is Item item)
            {
                ItemCaught?.Invoke(item);

                if (item.Weight > _capacity)
                {
                    ProgressManager.instance.Win(2);
                    StartCoroutine(GameOver(true));
                }
            }

            _collider.enabled = false;
            _animator.SetBool("IsCatching", true);
            _animator.SetBool("IsCasting", false);
            caughtItem.Drag(_tip);
            _audioSource.Play();
            Destroy(caughtItem.gameObject, 1f);

            if (!_isGameOver)
                StartCoroutine(Reset());
        }

        private IEnumerator Reset()
        {
            yield return new WaitForSeconds(1f);
            _collider.enabled = true;
            _animator.SetBool("IsCatching", false);
            _animator.SetBool("IsCasting", true);
            _fisher.Reset();
        }

        public IEnumerator GameOver(bool isWin)
        {
            _isGameOver = true;
            _winPanel.SetActive(isWin);
            _losePanel.SetActive(!isWin);

            if (isWin)
            {
                yield return new WaitForSeconds(1.5f);
                SceneLoader.Instance.Load_MainScene();
            }

            //Destroy(this);
        }
    }
}