using FishingHim.FishAndFisherman.Fisherman;
using UnityEngine;

namespace FishingHim.FishAndFisherman.Hook
{
    public class HookHolder : MonoBehaviour
    {
        private void Update()
        {
            transform.rotation = FishermanRotation.Instance.transform.rotation;
        }
    }
}