using FishingHim.FishAndFisherman.Fish;
using UnityEngine;

namespace FishingHim.FishAndFisherman.Hook
{
    public class Hook : MonoBehaviour
    {
        private void OnTriggerEnter(Collider other)
        {
            if(other.TryGetComponent<FishHealth>(out FishHealth fishHealth))
            {
                fishHealth.GetHit(transform);
            }
        }
    }
}