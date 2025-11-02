using UnityEngine;
using FishingHim.FishAndFisherman.Sections;

namespace FishingHim.FishAndFisherman.Hook
{
    public class HooksSpawner : MonoBehaviour
    {
        [SerializeField] private HookHolder _leftHookHolder;
        [SerializeField] private HookHolder _rightHookHolder;
        [SerializeField] private Transform _spawnsHolder;
        [SerializeField] private Transform[] _spawnPositions;
        [SerializeField] private Vector3 _spawnDistanceDelta;

        private bool _isEven;
        private Vector3 _initialSpawnPosition;
        private HookHolder _currentHookHolder;

        private void Awake()
        {
            _initialSpawnPosition = _spawnsHolder.position;
            SpawnHook();
        }

        public void SpawnHook()
        {
            if (_currentHookHolder != null)
            {
                Destroy(_currentHookHolder.gameObject);
            }

            _isEven = !_isEven;
            HookHolder hookHolderToSpawn;
            int randomIndex = Random.Range(0, _spawnPositions.Length);
            Vector3 spawnPos = _initialSpawnPosition + (_spawnDistanceDelta * SectionsContoroller.Instance.SectionIndex);
            _spawnsHolder.position = spawnPos;

            if (_isEven)
            {
                hookHolderToSpawn = _leftHookHolder;
            }
            else
            {
                hookHolderToSpawn = _rightHookHolder;
            }

            _currentHookHolder = Instantiate(hookHolderToSpawn, _spawnPositions[randomIndex].position, _leftHookHolder.transform.rotation);
        }
    }
}