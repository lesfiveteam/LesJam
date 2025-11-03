using UnityEngine;

namespace FishingHim.FishAndFisherman.Tutorial
{
    public class TutorialManager : MonoBehaviour
    {
        [SerializeField] private Tutorial _tutorial;
        [SerializeField] private Transform _tutorialHolder;

        private void Awake()
        {
            if (PlayerPrefs.GetInt("FishAndFishermanTutorialComplete") == 0)
            {
                Instantiate(_tutorial, _tutorialHolder);
                PlayerPrefs.SetInt("FishAndFishermanTutorialComplete", 1);
                Time.timeScale = 0;
            }
        }
    }
}