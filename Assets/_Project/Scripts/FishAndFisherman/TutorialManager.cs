using System.Collections;
using UnityEngine;

namespace FishingHim.FishAndFisherman.Tutorial
{
    public class TutorialManager : MonoBehaviour
    {
        [SerializeField] private Tutorial _tutorial;
        [SerializeField] private Transform _tutorialHolder;
        [SerializeField] private float _fadeDelay;

        private void Awake()
        {
            if (PlayerPrefs.GetInt("FishAndFishermanTutorialComplete") == 0)
            {
                StartCoroutine(TutorialRoutine());
            }
        }

        private IEnumerator TutorialRoutine()
        {
            yield return new WaitForSeconds(_fadeDelay);
            Instantiate(_tutorial, _tutorialHolder);
            PlayerPrefs.SetInt("FishAndFishermanTutorialComplete", 1);
            Time.timeScale = 0;
        }
    }
}