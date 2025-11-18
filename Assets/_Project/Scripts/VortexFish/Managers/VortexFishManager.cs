using FishingHim.Common;
using FishingHim.VortexFish.Generator;
using System;
using System.Collections;
using System.Linq.Expressions;
using TMPro;
using UnityEditor;
using UnityEngine;
using UnityEngine.UI;

namespace FishingHim.VortexFish.Manager
{
    public class VortexFishManager : MonoBehaviour
    {
        // Singleton
        private static VortexFishManager _instance;
        public static VortexFishManager Instance { get { return _instance; } }
        public Fish Fish = null;
        public RowGenerator RowGenerator = null;
        public TMP_Text FishermanCountTMPro;
        public GameObject UITutorial = null;
        [SerializeField] private Button exitButton;
        [SerializeField] private float tutorialWaitTime;

        [Header("Баланс настройки")]
        private int boostCount = 0;
        [SerializeField]
        private int boostCountForRage = 3; // кол-во буста для входа в ярость

        private int deadFishermanCount = 0;
        [SerializeField]
        private int deadFishermanCountForWin = 3; // кол-во сбитых рыбаков для победы

        [Header("Турбо")]
        [SerializeField]
        private float turboTime = 3f;
        [SerializeField] 
        private float turboTimeForSpawnItems = 1f;
        private bool InTurbo = false;
        public float Speed = 2f;
        public float TurboSpeed = 4f;
        public float GeneratorTime = 2f; // Каждые столько секунд будет создавать объект на линии
        public float GeneratorTurboTime = 1f; // Каждые столько секунд будет создавать объект на линии в турбо
        [SerializeField] private Material turboMaterial;
        [SerializeField] private Material normalMeterial;
        [SerializeField] private MeshRenderer[] fishMeshes;

        [Header("Анимация рыбы")]
        [SerializeField]
        private float animationSpeedInTurbo = 3f;

        public bool IsEndGame = false;
        
        private bool isTutorialShown = false;
        
        public bool IsTutorialShown => isTutorialShown;

        private void Awake()
        {
            if (_instance == null)
            {
                _instance = this;
            }
            if (Fish == null)
            {
                GameObject fishGb = GameObject.FindGameObjectWithTag("Player");
                if (!fishGb)
                {
                    Debug.LogError("Не заполнен Fish! У Fish нет тега Player!");
                }
                Fish = fishGb.GetComponent<Fish>();
                if (!Fish)
                {
                    Debug.LogError("Не заполнен Fish!");
                }
            }
            if (!RowGenerator)
            {
                Debug.LogError("Не заполнен RowGenerator");
            }
            if (!FishermanCountTMPro)
            {
                Debug.LogError("Не заполнен FishermanCountTMPro");
            }
        }

        private void Start()
        {
            // UI текст
            _instance.FishermanCountTMPro.text =
                _instance.deadFishermanCount + "/" + _instance.deadFishermanCountForWin;
            SoundsManager.Instance.PlaySound(SoundType.VortexFishMusic);
            if (UITutorial)
            {
                StartCoroutine(WaitAndShowTutorial());
            }
        }

        /**
         * Прибавляет к количеству съеденного печенья +1
         */
        public static void AddBoost()
        {
            _instance.boostCount++;
            if (_instance.boostCount >= _instance.boostCountForRage)
            {
                _instance.boostCount = 0;
                _instance.EnterInTurbo();
            }
        }
        /**
         * Прибавляет к количеству сбитых рыбаков
         */
        public static void AddDeadFisherman()
        {
            _instance.deadFishermanCount++;
            // UI текст
            _instance.FishermanCountTMPro.text = 
                _instance.deadFishermanCount + "/" + _instance.deadFishermanCountForWin;
            if (_instance.deadFishermanCount >= _instance.deadFishermanCountForWin)
            {
                if (!Instance.IsEndGame)
                {
                    // Победа
                    Instance.IsEndGame = true;
                    ProgressManager.instance.Win(0);
                }
            }
        }
        /**
         * Возвращает истину, если игрок находится в состоянии турбо
         */
        public static bool InTurboMode()
        {
            return _instance.InTurbo;
        }

        /**
         * Подождать и выйти из турбо
         */
        private IEnumerator WaitAndExitFromTurbo()
        {
            yield return new WaitForSeconds(turboTime);
            ExitFromTurbo();
        }

        /** 
         * Выйти из турбо
         */
        private void ExitFromTurbo()
        {
            foreach (MeshRenderer renderer in fishMeshes)
            {
                renderer.material = normalMeterial;
            }

            InTurbo = false;
            SoundsManager.Instance.StopSound(SoundType.VortexFishTurboEnd);
            SoundsManager.Instance.PlaySound(SoundType.VortexFishSpeedUp);
            Fish.SetAnimationSpeed(1f);
            RowGenerator.CanGenerate = true;
        }
        /** 
         * Войти в турбо
         */
        private void EnterInTurbo()
        {
            foreach(MeshRenderer renderer in fishMeshes)
            {
                renderer.material = turboMaterial;
            }

            InTurbo = true;
            SoundsManager.Instance.PlaySound(SoundType.VortexFishTurboStart);
            SoundsManager.Instance.PlaySound(SoundType.VortexFishSpeedUp);
            StartCoroutine(WaitAndStopGenerate());
            StartCoroutine(WaitAndExitFromTurbo());
            Fish.SetAnimationSpeed(animationSpeedInTurbo);
        }

        /** 
         * Через время запрещает генерацию на время, чтобы игрок успел отойти от турбо режима и не умер быстро
         */
        private IEnumerator WaitAndStopGenerate()
        {
            yield return new WaitForSeconds(turboTimeForSpawnItems);
            RowGenerator.CanGenerate = false;
        }


        /**
         * Через X секунд ставит игру на паузу и показывает туториал
         */
        private IEnumerator WaitAndShowTutorial()
        {
            exitButton.interactable = false;
            yield return new WaitForSeconds(tutorialWaitTime);
            UITutorial.SetActive(true);
            Time.timeScale = 0f;
            isTutorialShown = true;
            exitButton.interactable = true;
        }
    }
}