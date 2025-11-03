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
        private float _capacity = 3f;
        //private AudioSource _audioSource;
        private bool _isGameOver = false;
        private float _waterYLevel = 0.8f;

        public static Hook Instance { get; private set; }

        private void Awake()
        {
            _animator = GetComponent<Animator>();
            _collider = GetComponent<Collider2D>();
            //_audioSource = GetComponent<AudioSource>();
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
                    //ProgressManager.instance.Lose();
                    StartCoroutine(GameOver(false));
                }

            }
            else if (caughtItem is Item item)
            {
                ItemCaught?.Invoke(item);

                if (item.Weight > _capacity)
                {
                    // ProgressManager.instance.Win(2);
                    StartCoroutine(GameOver(true));
                }
            }

            _collider.enabled = false;
            _animator.SetBool("IsCatching", true);
            _animator.SetBool("IsCasting", false);
            caughtItem.Drag(_tip);
            //_audioSource.Play();
            SoundsManager.Instance.PlaySound(SoundType.TasteThisHook);
            Destroy(caughtItem.gameObject, 1f);

            if (!_isGameOver)
                StartCoroutine(Reset());
        }

        private IEnumerator Reset()
        {
            yield return new WaitForSeconds(1f);
            //_collider.enabled = true;
            _animator.SetBool("IsCatching", false);
            _animator.SetBool("IsCasting", true);
            _fisher.Reset();
            yield return new WaitForSeconds(6f);
            _collider.enabled = true;
        }

        public IEnumerator GameOver(bool isWin)
        {
            //_isGameOver = true;
            //_winPanel.SetActive(isWin);
            //_losePanel.SetActive(!isWin);

            //SoundsManager.Instance.PlaySound(isWin ? SoundType.TasteThisWin : SoundType.TasteThisLose);

            //if (isWin)
            //{
            //    yield return new WaitForSeconds(2f);
            //    SceneLoader.Instance.Load_MainScene();
            //}

            //Destroy(this);

            _isGameOver = true;

            if (isWin)
                yield return Win();
            else
                yield return Lose();
        }

        private IEnumerator Win()
        {
            StartCoroutine(MoveFisherDown(_fisher.gameObject.transform));
            SoundsManager.Instance.PlaySound(SoundType.TasteThisWin);
            yield return new WaitForSeconds(1f);
            _winPanel.SetActive(true);
            yield return new WaitForSeconds(3.5f);
            ProgressManager.instance.Win(2);
            //SceneLoader.Instance.Load_MainScene();
        }

        private IEnumerator MoveFisherDown(Transform fisher)
        {
            _fisher.Afall();
            fisher.Translate(new(0f, -1f, 0f));
            yield return new WaitForSeconds(2f);

            foreach (var renderer in GetComponentsInChildren<SpriteRenderer>())
                renderer.enabled = false;

            while (fisher.position.y > -3.5f)
            {
                yield return new WaitForSeconds(0.03f);
                fisher.Translate(new(0.07f, -0.1f, 0f));
            }
        }

        private IEnumerator Lose()
        {
            _losePanel.SetActive(true);
            SoundsManager.Instance.PlaySound(SoundType.TasteThisLose);
            yield return new WaitForSeconds(2f);
            ProgressManager.instance.Lose();
        }
    }
}