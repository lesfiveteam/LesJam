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
        private AudioSource _audioSource;
        private bool _isActive = false;

        private void Start()
        {
            _renderer = GetComponentInChildren<SpriteRenderer>();
            _audioSource = GetComponent<AudioSource>();
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
                _isActive = false;

                if (_item.IsRaised())
                    _item.Drop();
            }
            else if (!_isActive)
            {
                _renderer.color = _activeColor;
                _audioSource.Play();
                _isActive = true;
            }
        }
    }
}