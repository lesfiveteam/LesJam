using FishingHim.Common;
using FishingHim.VortexFish.Manager;
using TMPro;
using UnityEngine;

namespace FishingHim.VortexFish.Managers
{
    public class Timer : MonoBehaviour
    {
        [SerializeField] private float _time;
        [SerializeField] private TMP_Text _timerText;

        private void Update()
        {
            if (VortexFishManager.Instance.IsEndGame)
                return;

            if(_time <= 0)
            {
                _time = 0;
                UpdateTimerText();
                Lose();
                return;
            }

            _time -= Time.deltaTime;
            UpdateTimerText();
        }

        private void UpdateTimerText()
        {
            int minutes = Mathf.FloorToInt(_time / 60f);
            int seconds = Mathf.FloorToInt(_time % 60f);
            _timerText.text = string.Format("{0:00}:{1:00}", minutes, seconds);
        }

        private void Lose()
        {
            VortexFishManager.Instance.IsEndGame = true;
            ProgressManager.instance.Lose();
        }
    }
}