using UnityEngine;
using System.Collections;
using FishingHim.FishAndFisherman.Fisherman;
using FishingHim.FishAndFisherman.Sections;
using FishingHim.FishAndFisherman.Hook;

namespace FishingHim.FishAndFisherman.Timer
{
    public class TimerManager : MonoBehaviour
    {
        [Header("Timer Settings")]
        [SerializeField] private int _rotationsPerSection = 5;
        [SerializeField] private float _sectionTransitionDelay = 2f;
        [SerializeField] private float _rotationSpeed = 2f;
        [SerializeField] private float _rotationDelta = 5f;
        [SerializeField] private float _rotationSpeedIncrease = 0.5f;
        [SerializeField] private float _rotationDeltaDecrease = 0.5f;
        [SerializeField] private FishermanRotation _fishermanRotation;
        [SerializeField] private SectionsContoroller _sectionsContoroller;
        [SerializeField] private HooksSpawner _hooksSpawner;

        private float _waitTimer = 0f;
        private bool _isWaiting = false;
        private bool _hasSpawnedHook = false;
        private int _currentRotationCount = 0;
        private bool _isInTransition = false;

        private void Start()
        {
            StartCoroutine(FishermanRotationCoroutine());
        }

        private IEnumerator FishermanRotationCoroutine()
        {
            while (true)
            {
                if (_fishermanRotation == null || _isInTransition)
                    yield return null;

                if (_isWaiting)
                {
                    _waitTimer += Time.deltaTime;

                    if (_waitTimer >= _rotationDelta)
                    {
                        _isWaiting = false;
                        _waitTimer = 0f;
                        _fishermanRotation.SwitchDirection();
                        _hasSpawnedHook = false;
                        _currentRotationCount++;

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

                    _fishermanRotation.UpdateRotation(_rotationSpeed * Time.deltaTime);

                    if (_fishermanRotation.HasReachedTarget())
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

            _sectionsContoroller.GoToNextSection();
            IncreaseDifficulty();
            _currentRotationCount = 0;

            yield return new WaitForSeconds(_sectionTransitionDelay);

            _isInTransition = false;
        }

        public void IncreaseDifficulty()
        {
            _rotationSpeed += _rotationSpeedIncrease;
            _rotationDelta = Mathf.Max(0.1f, _rotationDelta - _rotationDeltaDecrease);
        }
    }
}