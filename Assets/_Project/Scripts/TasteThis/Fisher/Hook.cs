using FishingHim.Common;
using System;
using System.Collections;
using UnityEngine;
using UnityEngine.SceneManagement;

namespace FishingHim.TasteThis
{
    public class Hook : MonoBehaviour
    {
        [SerializeField]
        private int _targetFishesNumber = 10;
        private Animator _animator;
        private float _animDuration;
        private Collider2D _collider;
        private int _fishesNumber;
        private float _capacity = 3f;

        public static Hook Instance { get; private set; }

        private void Awake()
        {
            _animator = GetComponent<Animator>();
            _collider = GetComponent<Collider2D>();
            _animDuration = _animator.GetCurrentAnimatorStateInfo(0).length;
            Instance = this;
        }

        public event EventHandler FishCaught;

        public delegate void ItemCaughtHandler(Item item);
        public event ItemCaughtHandler ItemCaught;

        [SerializeField]
        private Transform _tip;

        public Vector2 GetHookPosition()
        {
            return (Vector2)_tip.position;
        }

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
            bool _continueGame = true;

            if (caughtItem is SmallFish)
            {
                _fishesNumber++;

                if (_fishesNumber >= _targetFishesNumber)
                {
                    ProgressManager.instance.Lose();
                    StartCoroutine(GameOver(false));
                    _continueGame = false;
                }

            }
            else if (caughtItem is Item item)
            {
                ItemCaught?.Invoke(item);

                if (item.Weight > _capacity)
                {
                    ProgressManager.instance.Win(2);
                    StartCoroutine(GameOver(true));
                    _continueGame = false;
                }
            }

            _collider.enabled = false;
            _animator.SetBool("IsMoving", true);
            caughtItem.Drag(_tip);
            Destroy(caughtItem.gameObject, _animDuration / 2f);

            if (_continueGame)
                StartCoroutine(Reset());
        }

        private IEnumerator Reset()
        {
            yield return new WaitForSeconds(_animDuration);
            _collider.enabled = true;
            _animator.SetBool("IsMoving", false);
        }

        public IEnumerator GameOver(bool isWin)
        {
            yield return new WaitForSeconds(1f);
            FaderManager._instance.Load_MainScene();
            Destroy(this);
        }
    }
}