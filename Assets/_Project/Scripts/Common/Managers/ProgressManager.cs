using FishingHim.Common;
using System;
using UnityEngine;
using UnityEngine.Events;

namespace FishingHim.Common
{
    public class ProgressManager : MonoBehaviour
    {
        [SerializeField] UnityEvent eventToPlayOnWin;   //сюда можно (необязательно) добавить функции загрузки уровня, визуальные эффекты и прочее,
                                                        //что хочется вызывать по успешному завершении мини-игры.
        [SerializeField] UnityEvent eventToPlayOnLose;  //аналогично для проигрыша

        const int NUMBER_OF_GAMES = 3;

        int numOfCompletedGames;
        int numOfDeaths;
        bool[] completedGamesArray;

        int GetNumberOfSuccessfulGames() { return numOfCompletedGames; }
        int GetNumberOfDeaths() { return numOfDeaths; }
        bool[] GetCompletedLevelsArray() { return completedGamesArray; }

        public static ProgressManager instance;

        void Awake()
        {
            completedGamesArray = new bool[NUMBER_OF_GAMES];

            if (instance == null)
            {
                instance = this;
            }
            else if (instance != this)
            {
                Destroy(this.gameObject);
            }

            DontDestroyOnLoad(instance);
        }

        //completedLevelNumber начинается с 0
        public void Win(int completedLevelNumber)
        {
            if (completedGamesArray[completedLevelNumber])  //если уровень уже был пройден
                return;
            numOfCompletedGames++;
            completedGamesArray[completedLevelNumber] = true;
            SavePlayerData();

            eventToPlayOnWin?.Invoke();
        }

        public void Lose()
        {
            numOfDeaths++;
            SavePlayerData();

            eventToPlayOnLose?.Invoke();
        }

        void SavePlayerData()
        {
            PlayerPrefs.SetInt("numOfSuccessfulGames", numOfCompletedGames);
            PlayerPrefs.SetInt("numOfDeaths", numOfDeaths);
            for (int i = 0; i < completedGamesArray.Length; i++)
            {
                string playerPrefKey = $"level{i}Complete";
                if (completedGamesArray[i])
                {
                    PlayerPrefs.SetInt(playerPrefKey, 1);
                }
                else
                {
                    PlayerPrefs.SetInt(playerPrefKey, 0);
                }
            }
        }

        public void LoadPlayerData()
        {
            numOfCompletedGames = PlayerPrefs.GetInt("numOfSuccessfulGames");
            numOfDeaths = PlayerPrefs.GetInt("numOfDeaths");
            for (int i = 0; i < completedGamesArray.Length; i++)
            {
                string playerPrefKey = $"level{i}Complete";
                completedGamesArray[i] = Convert.ToBoolean(PlayerPrefs.GetInt(playerPrefKey));
            }
        }

        public void DeletePlayerData()
        {
            PlayerPrefs.DeleteAll();
        }
    }
}
