using System.Collections;
using UnityEngine;

namespace FishingHim.VortexFish 
{
    /** 
     * @deprecated с гд уточнили логику, придётся удалить его
     * Класс для создания движущихся объектов на уровне VortexFish
     */
    public class GeneratorOld : MonoBehaviour
    {
        [SerializeField]
        private float time = 2f; // Каждые столько секунд будет создавать объект
        [SerializeField]
        private GameObject prefab;

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
    }
}
