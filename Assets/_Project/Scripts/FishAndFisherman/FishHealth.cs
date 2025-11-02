using FishingHim.FishAndFisherman.Sections;
using UnityEngine;
using UnityEngine.UI;

namespace FishingHim.FishAndFisherman.Fish
{
    public class FishHealth : MonoBehaviour
    {
        [SerializeField] private Image[] _hpImages;
        [SerializeField] private Sprite _emptyHpSprite;
        [SerializeField] private SectionsContoroller _sectionsContoroller;

        private int _health;

        private void Awake()
        {
            _health = _hpImages.Length;
        }

        public void LoseHp()
        {
            if (_health <= 0)
                return;

            _hpImages[_health - 1].sprite = _emptyHpSprite;
            _health--;
            _sectionsContoroller.RestartSection();

            if (_health == 0)
                Lose();
        }

        private void Lose()
        {
            Debug.Log("Lose");
        }
    }
}