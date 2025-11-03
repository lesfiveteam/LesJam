using System.Collections;
using UnityEngine;
using UnityEngine.UI;

namespace FishingHim.FishAndFisherman.Tutorial
{
    public class TutorialManager : MonoBehaviour
    {
        [SerializeField] private Tutorial _tutorial;
        [SerializeField] private Transform _tutorialHolder;
        [SerializeField] private float _fadeDelay;
        [SerializeField] private Button _exitButton;

        private void Awake()
        {
            StartCoroutine(TutorialRoutine());
        }

        private IEnumerator TutorialRoutine()
        {
            _exitButton.interactable = false;
            yield return new WaitForSeconds(_fadeDelay);
            Instantiate(_tutorial, _tutorialHolder);
            Time.timeScale = 0;
            _exitButton.interactable = true;
        }
    }
}