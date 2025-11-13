using FishingHim.VortexFish.Manager;
using UnityEngine;

namespace FishingHim.VortexFish
{
    // Класс для движение окружения
    public class EnviromentMovement : MonoBehaviour
    {
        [SerializeField] private float _lifeTime = 8f;

        private void Awake()
        {
            Destroy(gameObject, _lifeTime);
        }

        private void FixedUpdate()
        {
            float speed = VortexFishManager.InTurboMode()
                ? VortexFishManager.Instance.TurboSpeed 
                : VortexFishManager.Instance.Speed;
            // Обновляем позицию напрямую
            transform.position += Vector3.back * speed * Time.deltaTime;
        }
    }
}