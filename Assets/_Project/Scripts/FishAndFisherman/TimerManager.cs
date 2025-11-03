using UnityEngine;
using System.Collections;
using FishingHim.FishAndFisherman.Fisherman;
using FishingHim.FishAndFisherman.Sections;
using FishingHim.FishAndFisherman.Hook;
using FishingHim.Common;
using TMPro;

namespace FishingHim.FishAndFisherman.Timer
{
    public class TimerManager : Singleton<TimerManager>
    {
        [SerializeField] private int _rotationsPerSection = 5;
        [SerializeField] private float _sectionTransitionDelay = 2f;
        [SerializeField] private float _rotationSpeed = 2f;
        [SerializeField] private float _rotationDelta = 5f;
        [SerializeField] private float _rotationSpeedIncrease = 0.5f;
        [SerializeField] private float _rotationDeltaDecrease = 0.5f;
        [SerializeField] private float _startingSequenceTime = 3f;
        [SerializeField] private HooksSpawner _hooksSpawner;
        [SerializeField] private TMP_Text _hooksCountText;

        private float _waitTimer = 0f;
        private bool _isWaiting = false;
        private bool _hasSpawnedHook = false;
        private int _currentRotationCount = 0;
        private bool _isInTransition = false;

        private void Start()
        {
            StartCoroutine(FishermanRotationCoroutine());
            UpdateHookCountText();
        }

        private IEnumerator FishermanRotationCoroutine()
        {
            yield return new WaitForSeconds(_startingSequenceTime);

            while (true)
            {
                if (_isInTransition)
                    yield return null;

                if (_isWaiting)
                {
                    _waitTimer += Time.deltaTime;

                    if (_waitTimer >= _rotationDelta)
                    {
                        _isWaiting = false;
                        _waitTimer = 0f;
                        FishermanRotation.Instance.SwitchDirection();
                        _hasSpawnedHook = false;
                        _currentRotationCount++;
                        UpdateHookCountText();

                        if (_currentRotationCount >= _rotationsPerSection)
                        {
                            yield return StartCoroutine(SectionTransitionCoroutine());
                        }
                    }

                    yield return null;
                }
                else
                {
                    if (!_hasSpawnedHook)
                    {
                        _hooksSpawner.SpawnHook();
                        _hasSpawnedHook = true;
                    }

                    FishermanRotation.Instance.UpdateRotation(_rotationSpeed * Time.deltaTime);

                    if (FishermanRotation.Instance.HasReachedTarget())
                    {
                        _isWaiting = true;
                    }

                    yield return null;
                }
            }
        }

        private IEnumerator SectionTransitionCoroutine()
        {
            _isInTransition = true;
            SectionsContoroller.Instance.GoToNextSection();
            IncreaseDifficulty();
            _currentRotationCount = 0;
            UpdateHookCountText();
            yield return new WaitForSeconds(_sectionTransitionDelay);
            _isInTransition = false;
        }

        public void RestartSection()
        {
            _currentRotationCount = 0;
            UpdateHookCountText();
        }

        private void UpdateHookCountText()
        {
            _hooksCountText.text = (_currentRotationCount + 1) + "/" + _rotationsPerSection;
        }

        public void IncreaseDifficulty()
        {
            _rotationSpeed += _rotationSpeedIncrease;
            _rotationDelta = Mathf.Max(0.1f, _rotationDelta - _rotationDeltaDecrease);
        }
    }
}