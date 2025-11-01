using UnityEngine;

namespace FishingHim.VortexFish.Manager
{
    public class VortexFishManager : MonoBehaviour
    {
        // Singleton
        private static VortexFishManager _instance;
        public VortexFishManager Instance { get { return _instance; } }
        public Fish Fish = null;

        private int boostCount = 0;
        [SerializeField]
        private int boostCountForRage = 3; // кол-во буста для входа в ярость

        private int deadFishermanCount = 0;
        [SerializeField]
        private int deadFishermanCountForWin = 3; // кол-во сбитых рыбаков для победы

        private void Start()
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
        }

        /**
         * Прибавляет к количеству съеденного печенья +1
         */
        public void AddBoost()
        {
            boostCount++;
            if (boostCount >= boostCountForRage)
            {
                boostCount = 0;
                Fish.EnterInRage();
            }
        }
        /**
         * Прибавляет к количеству сбитых рыбаков
         */
        public void AddDeadFisherman()
        {
            deadFishermanCount++;
            if (deadFishermanCount >= deadFishermanCountForWin)
            {
                // Победа
                Debug.Log("Реализуй победу, когда будет готов GameManager от Аллана");
            }
        }
        /**
         * Возвращает истину, если игрок находится в состоянии ярости
         */
        public static bool IsPlayerInRage()
        {
            return _instance.Fish.InRage;
        }
    }
}

