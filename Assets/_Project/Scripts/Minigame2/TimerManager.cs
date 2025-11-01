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
        [SerializeField] private float _nextSectionTime = 5f;
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

        private void Awake()
        {
            StartCoroutine(FishermanRotationCoroutine());
            StartCoroutine(NextSectionCoroutine());
        }

        private IEnumerator FishermanRotationCoroutine()
        {
            while (true)
            {
                if (_fishermanRotation == null)
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

        private IEnumerator NextSectionCoroutine()
        {
            while (true)
            {
                yield return new WaitForSeconds(_nextSectionTime);
                _sectionsContoroller.GoToNextSection();
            }
        }

        public void IncreaseDifficulty()
        {
            _rotationSpeed += _rotationSpeedIncrease;
            _rotationDelta = Mathf.Max(0.1f, _rotationDelta - _rotationDeltaDecrease);
        }
    }
}