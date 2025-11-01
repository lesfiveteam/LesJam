using System.Collections;
using UnityEngine;

namespace FishingHim.VortexFish 
{
    /** 
     * Класс для создания движущихся объектов на уровне VortexFish
     */
    public class Generator : MonoBehaviour
    {
        [SerializeField]
        private float time = 2f; // Каждые столько секунд будет создавать объект
        [SerializeField]
        private GameObject prefab;

        private SpriteRenderer lastSpriteRenderer;
        private float lastObjectRightEdge;

        void Start()
        {
            if (prefab == null)
            {
                Debug.LogError("Не назначили Prefab");
            }
            StartCoroutine(WaitAndCreateObject());
        }

        /**
         * Рекурсивный. Создаём движующийся объект по префабу, затем вызывает сам себя
         */
        IEnumerator WaitAndCreateObject()
        {
            yield return new WaitForSeconds(time);
            GameObject createdObject = Instantiate(prefab, transform);
            // Наделяем скриптом передвижения, чтобы не стоял
            createdObject.AddComponent<EnviromentMovement>();
            //CreateObject();
            StartCoroutine(WaitAndCreateObject());
        }

        /**
         * Может понадобится в случае, если нужно будет двигать пол
         * Создаёт объект и добавляет к нему движение
         * 
         */
        //void CreateObject()
        //{
        //    GameObject createdObject = Instantiate(prefab, transform);

        //    // Получаем SpriteRenderer для вычисления размеров
        //    SpriteRenderer spriteRenderer = createdObject.GetComponent<SpriteRenderer>();
        //    if (spriteRenderer == null)
        //    {
        //        Debug.LogError("Prefab не содержит SpriteRenderer");
        //        return;
        //    }

        //    // Вычисляем размеры спрайта
        //    Bounds bounds = spriteRenderer.bounds;
        //    float objectWidth = bounds.size.x;

        //    // Позиционируем объект так, чтобы он касался предыдущего
        //    Vector3 spawnPosition = Vector3.zero;

        //    if (lastSpriteRenderer != null)
        //    {
        //        // Вычисляем позицию для идеального касания
        //        float newObjectLeftEdge = lastObjectRightEdge;
        //        spawnPosition = new Vector3(newObjectLeftEdge + objectWidth * 0.5f,
        //                                  transform.position.y, 0f);
        //    }
        //    else
        //    {
        //        // Первый объект
        //        spawnPosition = transform.position;
        //    }

        //    createdObject.transform.position = spawnPosition;

        //    // Обновляем информацию о последнем объекте
        //    lastSpriteRenderer = spriteRenderer;
        //    lastObjectRightEdge = spawnPosition.x + objectWidth * 0.5f;

        //    // Добавляем движение
        //    createdObject.AddComponent<EnviromentMovement>();
        //}
    }
}
