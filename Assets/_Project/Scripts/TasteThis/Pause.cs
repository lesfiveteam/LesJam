using FishingHim.Common;
using UnityEngine;
using UnityEngine.Events;

namespace FishingHim
{
    public class Pause : MonoBehaviour
    {
        [SerializeField, Header("Событие при выходе в главное меню из паузы (можно например зафиксировать поражение в этом случае)")] 
        UnityEvent ExitFromPause;

        [SerializeField]
        private GameObject _pauseMenu;

        [SerializeField, Header("True если при выходе из паузы в меню надо автоматически засчитывать поражение")]
        private bool _autoLose = false;

        private void OnPause()
        {
            Time.timeScale = 1f - Time.timeScale;
            _pauseMenu.SetActive(!_pauseMenu.activeSelf);
        }

        private void OnExit()
        {
            if (_pauseMenu.activeSelf)
            {
                Time.timeScale = 1f;
                ExitFromPause?.Invoke();

                if (_autoLose)
                    ProgressManager.instance.Lose();
                else
                    SceneLoader._instance.Load_MainScene();

                Destroy(this);
            }
        }
    }
}