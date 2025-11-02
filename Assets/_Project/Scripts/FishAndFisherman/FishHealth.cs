using FishingHim.FishAndFisherman.Timer;
using UnityEngine;
using UnityEngine.UI;

namespace FishingHim.FishAndFisherman.Fish
{
    [RequireComponent(typeof(FishController))]
    public class FishHealth : MonoBehaviour
    {
        [SerializeField] private Image[] _hpImages;
        [SerializeField] private Sprite _emptyHpSprite;
        [SerializeField] private TimerManager _timerManager;
        [SerializeField] private FishController _fishController;
        [SerializeField] private float _rollingDangerDistance;

        private int _health;

        private void Awake()
        {
            _health = _hpImages.Length;
        }

        public void GetHit(Transform hitObject)
        {
            if (_health <= 0)
                return;

            if (_fishController.IsRolling && hitObject.position.y - transform.position.y > _rollingDangerDistance)
                return;

            LoseHp();
        }

        private void LoseHp()
        {
            _hpImages[_health - 1].sprite = _emptyHpSprite;
            _health--;
            _timerManager.RestartSection();

            if (_health == 0)
                Lose();
        }

        private void Lose()
        {
            Debug.Log("Lose");
        }
    }
}