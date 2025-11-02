using UnityEngine;
using UnityEngine.Events;

namespace FishingHim
{
    public class Pause : MonoBehaviour
    {
        [SerializeField, Header("—обытие при выходе в главное меню из паузы (можно например зафиксировать поражение в этом случае)")] 
        UnityEvent ExitFromPause;

        [SerializeField]
        private GameObject _pauseMenu;

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
                FaderManager._instance.Load_MainScene();
                Destroy(this);
            }
        }
    }
}