using UnityEngine;

namespace FishingHim.VortexFish
{
    public class EnvironmentSpawner : MonoBehaviour
    {
        [SerializeField] private GameObject _floorPart;
        [SerializeField] private Transform _nextPartTrigger;
        [SerializeField] private GameObject _previousPart;
        [SerializeField] private float _floorPartLength;

        private void LateUpdate()
        {
            if (_previousPart.transform.position.z <= _nextPartTrigger.position.z)
            {
                Vector3 spawnPosition = _previousPart.transform.position + Vector3.forward * _floorPartLength;
                _previousPart = Instantiate(_floorPart, spawnPosition, Quaternion.identity, transform);
            }
        }
    }
}