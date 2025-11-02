using UnityEngine;

namespace FishingHim.TasteThis
{
    public class HeavyItemActivator : MonoBehaviour
    {
        [SerializeField]
        private Color _inactiveColor = Color.gray, _activeColor = Color.white;
        [SerializeField]
        private PlayerController _player;
        private SpriteRenderer _renderer;
        private float _mass;
        private Item _item;

        private void Start()
        {
            _renderer = GetComponentInChildren<SpriteRenderer>();
            _renderer.color = _inactiveColor;
            _player.OnLevelChange += PlayerChangeLevel;
            _mass = GetComponent<Rigidbody2D>().mass;
            _item = GetComponent<Item>();
        }

        private void PlayerChangeLevel(float capacity)
        {
            if (capacity < _mass)
            {
                _renderer.color = _inactiveColor;

                if (_item.IsRaised())
                    _item.Drop();
            }
            else
                _renderer.color = _activeColor;
        }

    }
}