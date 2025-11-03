using FishingHim.VortexFish.CollectedItem;
using FishingHim.VortexFish.Manager;
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
        // Singleton
        private static VortexFishManager _instance;
        public static VortexFishManager Instance { get { return _instance; } }
        [SerializeField]
        private List<RowPattern> rowPatterns;
        [SerializeField]
        private List<Transform> lineGeneratorTransform = new List<Transform>();


        [SerializeField]
        // Сколько раз нужно воспроизвести паттерн
        private int replayInTurboCount = 5;
        // Сколько раз уже воспроизвели паттерн
        private int currentReplayNumber = 0;
        // Прошлый индекс паттерна
        private int lastPatternIndex = 0;

        public bool CanGenerate = true;

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
            float time = VortexFishManager.InTurboMode()
                ? VortexFishManager.Instance.GeneratorTurboTime
                : VortexFishManager.Instance.GeneratorTime;
            yield return new WaitForSeconds(time);
            if (CanGenerate)
            {
                Generate();
            }
            StartCoroutine(WaitAndGenerate());
        }

        /**
         * Генерируем случайный шаблон с рыбаками и кормом
         */
        private void Generate()
        {
            // Случайный шаблон для строки
            int patternIndex = 0;
            if (VortexFishManager.InTurboMode() && currentReplayNumber < replayInTurboCount)
            {
                currentReplayNumber++;
                patternIndex = lastPatternIndex;
            }
            else
            {
                patternIndex = Random.Range(0, rowPatterns.Count);
                lastPatternIndex = patternIndex;
                currentReplayNumber = 0;
            }
            // Генерим линии
            Transform lineItem1 = GeneratorItem(rowPatterns[patternIndex].Line1, lineGeneratorTransform[0]);
            Transform lineItem2 = GeneratorItem(rowPatterns[patternIndex].Line2, lineGeneratorTransform[1]);
            Transform lineItem3 = GeneratorItem(rowPatterns[patternIndex].Line3, lineGeneratorTransform[2]);
            Transform lineItem4 = GeneratorItem(rowPatterns[patternIndex].Line4, lineGeneratorTransform[3]);
            Transform lineItem5 = GeneratorItem(rowPatterns[patternIndex].Line5, lineGeneratorTransform[4]);
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
                return Instantiate(collectedItem.transform, line.transform.position, collectedItem.transform.rotation);
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
