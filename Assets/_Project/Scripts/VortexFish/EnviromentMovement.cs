using FishingHim.VortexFish.Manager;
using System.Collections;
using UnityEngine;

namespace FishingHim.VortexFish
{
    // Класс для движение окружения
    public class EnviromentMovement : MonoBehaviour
    {
        private float lifeTime = 8f;

        private void Start()
        {
            // Даём время жизни объекту - чтобы не улетал в бесконечность
            StartCoroutine(WaitAndDestroy());
        }

        void FixedUpdate()
        {
            float speed = VortexFishManager.InTurboMode()
                ? VortexFishManager.Instance.TurboSpeed 
                : VortexFishManager.Instance.Speed;
            // Обновляем позицию напрямую
            transform.position += Vector3.back * speed * Time.deltaTime;
        }

        // Ждёт и уничтожает объект
        private IEnumerator WaitAndDestroy()
        {
            yield return new WaitForSeconds(lifeTime);
            Destroy(gameObject);
        }
    }

}
