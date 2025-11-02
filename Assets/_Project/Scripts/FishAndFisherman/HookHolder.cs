using UnityEngine;
using FishingHim.FishAndFisherman.Fisherman;

namespace FishingHim.FishAndFisherman.Hook
{
    public class HookHolder : MonoBehaviour
    {
        private FishermanRotation _fishermanRotation;

        private void Awake()
        {
            _fishermanRotation = FindFirstObjectByType<FishermanRotation>();
        }

        private void Update()
        {
            if (_fishermanRotation != null)
            {
                transform.rotation = _fishermanRotation.transform.rotation;
            }
        }
    }
}