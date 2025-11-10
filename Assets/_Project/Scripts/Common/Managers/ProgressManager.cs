using FishingHim.Common;
using System;
using UnityEngine;
using UnityEngine.Events;

namespace FishingHim.Common
{
    public class ProgressManager : MonoBehaviour
    {
        public UnityEvent eventToPlayOnWin;   //сюда можно (необязательно) добавить функции загрузки уровня, визуальные эффекты и прочее,
                                              //что хочется вызывать по успешному завершении мини-игры.
        public UnityEvent eventToPlayOnLose;  //аналогично для проигрыша
        public UnityEvent eventToEndGame;  //аналогично для проигрыша

        private const int NUMBER_OF_GAMES = 3;
        private const int NUMBERS_OF_FISHES = 5;

        private int numOfCompletedGames;
        private int numOfDeaths;

        public bool[] CompletedGamesArray { get; private set; }

        public bool IsWinGame { get; private set; }
        public bool IsWinMiniGame { get; private set; }

        public int GetNumberOfFishes() { return NUMBERS_OF_FISHES; }
        public int GetNumberOfAliveFishes() { return NUMBERS_OF_FISHES - numOfDeaths; }


        public static ProgressManager instance;

        private void Awake()
        {
            if (instance == null)
            {
                instance = this;

                NewGame();

                LoadPlayerData();
            }
            else if (instance != this)
            {
                Destroy(this.gameObject);
            }

            DontDestroyOnLoad(instance);
        }

        private void NewGame()
        {
            CompletedGamesArray = new bool[NUMBER_OF_GAMES];
            numOfCompletedGames = 0;
            numOfDeaths = 0;
        }

        //completedLevelNumber начинается с 0
        public void Win(int completedLevelNumber)
        {
            if (!CompletedGamesArray[completedLevelNumber])  //если уровень не был пройден
                numOfCompletedGames++;

            if (numOfCompletedGames >= NUMBER_OF_GAMES)
            {
                IsWinGame = true;

                eventToEndGame?.Invoke();
            }
            else
            {
                IsWinMiniGame = true;

                CompletedGamesArray[completedLevelNumber] = true;

                SavePlayerData();

                eventToPlayOnWin?.Invoke();
            }
        }

        public void Lose()
        {
            Debug.Log("do: " + numOfDeaths);
            numOfDeaths++;
            Debug.Log("posle: " + numOfDeaths);

            if (numOfDeaths >= NUMBERS_OF_FISHES)
            {
                IsWinGame = false;

                eventToEndGame?.Invoke();
                DeletePlayerData();
            }
            else
            {
                IsWinMiniGame = false;

                SavePlayerData();

                eventToPlayOnLose?.Invoke();
            }
        }

        private void SavePlayerData()
        {
            PlayerPrefs.SetInt("numOfSuccessfulGames", numOfCompletedGames);
            PlayerPrefs.SetInt("numOfDeaths", numOfDeaths);

            for (int i = 0; i < CompletedGamesArray.Length; i++)
            {
                string playerPrefKey = $"level{i}Complete";

                if (CompletedGamesArray[i])
                {
                    PlayerPrefs.SetInt(playerPrefKey, 1);
                }
                else
                {
                    PlayerPrefs.SetInt(playerPrefKey, 0);
                }
            }
        }

        private void LoadPlayerData()
        {
            numOfCompletedGames = PlayerPrefs.GetInt("numOfSuccessfulGames");
            numOfDeaths = PlayerPrefs.GetInt("numOfDeaths");

            for (int i = 0; i < CompletedGamesArray.Length; i++)
            {
                string playerPrefKey = $"level{i}Complete";
                CompletedGamesArray[i] = Convert.ToBoolean(PlayerPrefs.GetInt(playerPrefKey));
            }
        }

        public void RestartGame()
        {
            if (GetNumberOfAliveFishes() <= 0 || IsWinGame)
            {
                DeletePlayerData();
                NewGame();
            }
        }

        private void DeletePlayerData()
        {
            PlayerPrefs.DeleteAll();
        }
    }
}
