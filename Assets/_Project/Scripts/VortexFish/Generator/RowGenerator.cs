using FishingHim.VortexFish.CollectedItem;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace FishingHim.VortexFish.Generator
{
    /*
     * Класс для генерации бустов или рыбаков на всех 5-ти линиях
     */
    public class RowGenerator : MonoBehaviour
    {
        [SerializeField]
        private List<RowPattern> rowPatterns;
        private List<ItemGenerator> itemGenerators = new List<ItemGenerator>();
        [SerializeField]
        private List<Transform> lineGeneratorTransform = new List<Transform>();
        [SerializeField]
        private float time = 2f; // Каждые столько секунд будет создавать объект

        private void Start()
        {
            if (rowPatterns.Count < 1 || lineGeneratorTransform.Count < 1)
            {
                Debug.LogError("Не заполнен RowGenerator");
            }
            StartCoroutine(WaitAndGenerate());
        }

        /**
        * Рекурсивный. Создаём движующийся объект по префабу, затем вызывает сам себя
        */
        private IEnumerator WaitAndGenerate()
        {
            yield return new WaitForSeconds(time);
            Generate();
            StartCoroutine(WaitAndGenerate());
        }

        /**
         * Генерируем случайный шаблон с рыбаками и кормом
         */
        private void Generate()
        {
            int randomPattern = Random.Range(0, rowPatterns.Count);
            // Генерим линии
            Transform lineItem1 = GeneratorItem(rowPatterns[randomPattern].Line1, lineGeneratorTransform[0]);
            Transform lineItem2 = GeneratorItem(rowPatterns[randomPattern].Line2, lineGeneratorTransform[1]);
            Transform lineItem3 = GeneratorItem(rowPatterns[randomPattern].Line3, lineGeneratorTransform[2]);
            Transform lineItem4 = GeneratorItem(rowPatterns[randomPattern].Line4, lineGeneratorTransform[3]);
            Transform lineItem5 = GeneratorItem(rowPatterns[randomPattern].Line5, lineGeneratorTransform[4]);
            // Добавляем скрипт на движение
            AddEnviromentMovement(lineItem1);
            AddEnviromentMovement(lineItem2);
            AddEnviromentMovement(lineItem3);
            AddEnviromentMovement(lineItem4);
            AddEnviromentMovement(lineItem5);
        }

        /**
         * Генерирует конкретную линию
         */
        private Transform GeneratorItem(AbstractCollectedItem collectedItem, Transform line)
        {
            if (collectedItem)
            {
                return Instantiate(collectedItem.transform, line.transform.position, Quaternion.identity);
            }
            return null;
        }
        /** 
         * Добавляет скрипт на движение к рыбакам и корму
         */
        private void AddEnviromentMovement(Transform itemTransoform)
        {
            if (itemTransoform)
            {
                itemTransoform.gameObject.AddComponent<EnviromentMovement>();
            }
        }
    }
}
