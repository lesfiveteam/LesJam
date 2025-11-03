using FishingHim.Common;
using System.Collections.Generic;
using UnityEngine;

public class ProgressView : MonoBehaviour
{
    [SerializeField] private Transform[] _fishSpawnPoints;
    [SerializeField] private GameObject[] _fishermans;

    [SerializeField] private GameObject _fishPrefab;

    private ProgressManager progressManager;
    private List<Transform> _fishes;

    private void Start()
    {
        progressManager = ProgressManager.instance;
        _fishes = new List<Transform>();

        UpdateSceneView();
    }

    public void UpdateSceneView()
    {
        UpdateFishermans();
        UpdateFishes();
    }

    private void UpdateFishermans()
    {
        bool[] completedGamesArray = progressManager.CompletedGamesArray;

        if (_fishermans.Length != completedGamesArray.Length)
        {
            Debug.Log("CRITICAL: Fisherman prefabs count not equal to numbers of game!");
            return;
        }

        for (int i = 0; i < completedGamesArray.Length; i++)
        {
            _fishermans[i].SetActive(!completedGamesArray[i]);
        }
    }

    private void UpdateFishes()
    {
        if (_fishSpawnPoints.Length != progressManager.GetNumberOfFishes())
        {
            Debug.Log("CRITICAL: FishSpawnPoints count not equal to numbers of fishes!");
            return;
        }

        if (_fishes.Count > 0)
        {
            for (int i = 0; i < _fishes.Count; i++)
            {
                Destroy(_fishes[i].gameObject);
            }
        }

        _fishes.Clear();

        int currentFishCount = progressManager.GetNumberOfAliveFishes();

        for (int i = 0; i < currentFishCount; i++)
        {
            GameObject newFish = Instantiate(_fishPrefab, _fishSpawnPoints[i]);
            newFish.transform.SetParent(_fishSpawnPoints[i]);
            _fishes.Add(newFish.GetComponent<Transform>());
        }
    }

    public void FishToHook(Transform hook)
    {
        if (_fishes == null || _fishes.Count == 0 || hook == null)
            return;

        Transform nearestFish = FindNearestFish(hook.position);

        if (nearestFish != null)
        {
            Fish fishComponent = nearestFish.GetComponent<Fish>();

            if (fishComponent != null)
            {
                fishComponent.GoToHook(hook);
            }
        }
    }

    private Transform FindNearestFish(Vector3 hookPosition)
    {
        if (_fishes.Count == 0)
            return null;

        Transform nearestFish = _fishes[0];
        float nearestDistance = Vector3.Distance(nearestFish.position, hookPosition);

        for (int i = 1; i < _fishes.Count; i++)
        {
            float distance = Vector3.Distance(_fishes[i].position, hookPosition);

            if (distance < nearestDistance)
            {
                nearestDistance = distance;
                nearestFish = _fishes[i];
            }
        }

        return nearestFish;
    }
}
