using FishingHim.TasteThis;
using System;
using UnityEngine;

public class FishesSpawner : MonoBehaviour
{
    [SerializeField]
    private Vector2 _minPoisition, _maxPosition;

    [SerializeField]
    private float _slowFishesNumber = 5,
        _mediumFishesNumber = 3,
        _fastFishesNumber = 2;

    [SerializeField]
    private GameObject _fishPrefab;
    [SerializeField]
    private GameObject _playerFish;

    [Serializable]
    private class RestrictedArea
    {
        public Transform transform;
        public float radius;
        [NonSerialized]
        public Vector2 center;
    }

    [SerializeField]
    private RestrictedArea _restrictedArea;
    private GameObject _fishesContainer;

    private void Awake()
    {
        _fishesContainer = new GameObject("Fishes");
        _fishesContainer.transform.position = Vector3.zero;
        _restrictedArea.center = _restrictedArea.transform.position;

        for (int i = 0; i < _slowFishesNumber; i++)
            CreateFish(SmallFish.SmallFishType.Slow);

        for (int i = 0; i < _mediumFishesNumber; i++)
            CreateFish(SmallFish.SmallFishType.Medium);

        for (int i = 0; i < _fastFishesNumber; i++)
            CreateFish(SmallFish.SmallFishType.Fast);

        SetPosition(_playerFish.transform);
        Destroy(gameObject);
    }

    private void CreateFish(SmallFish.SmallFishType fishType)
    {
        GameObject fish = Instantiate(_fishPrefab, _fishesContainer.transform);
        fish.GetComponent<SmallFish>().FishType = fishType;
        SetPosition(fish.transform);
    }

    private void SetPosition(Transform transform)
    {
        Vector2 position = Vector2.zero;
        float beginTime = Time.realtimeSinceStartup;
        float timeLimit = 1f; // 1 sec max for spawn

        do
        {
            position.x = UnityEngine.Random.Range(_minPoisition.x, _maxPosition.x);
            position.y = UnityEngine.Random.Range(_minPoisition.y, _maxPosition.y);
        } while (Vector2.Distance(position, _restrictedArea.center) < _restrictedArea.radius &&
            Time.realtimeSinceStartup - beginTime < timeLimit);

        transform.position = position;
    }
}


