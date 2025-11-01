using FishingHim.VortexFish.Manager;
using UnityEngine;

namespace FishingHim.VortexFish.CollectedItem
{
    public class Fisherman : MonoBehaviour
    {
        private void OnCollisionEnter(Collision collision)
        {
            if (collision.transform.tag == "Player")
            {
                if (VortexFishManager.IsPlayerInRage())
                {
                    // Сбивает рыбаков
                    Destroy(gameObject);
                }
                else
                {
                    Debug.Log("Реализуй проигрыш, когда будет готов GameManager от Аллана");
                }
            }
        }
    }
}