using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;
using UnityEngine.InputSystem;

namespace FishingHim.TasteThis
{
    public class PlayerController : MonoBehaviour
    {
        [SerializeField]
        private float _initialSpeed = 0.9f, _speedDelta = 0.07f;
        [SerializeField]
        private int _maxCapacity = 9;
        [SerializeField]
        private float _inertiaMovementFactor = 1f, _inertiaRotationFactor = 1f;
        private float _speed;
        private Rigidbody2D _rb;
        private List<Item> _items = new();
        private Item _takenItem;
        public float _capacity = 1f;
        private SpriteRenderer _renderer;
        private Vector2 _newSpeed;
        private Vector3 _newAngles;
        private AudioSource _audioSource;

        [SerializeField]
        private GameObject _3dModel;
        [SerializeField]
        private Transform _itemHandler;

        private Animator _animator;
        private float _auraMaxAlpha = 0.5f;
        private float _auraStep;

        public event Action<float> OnLevelChange;

        private List<string> _collectedItems = new();

        private void Start()
        {
            _rb = GetComponent<Rigidbody2D>();
            _renderer = GetComponent<SpriteRenderer>();
            _audioSource = GetComponent<AudioSource>();
            Physics2D.IgnoreLayerCollision(LayerMask.NameToLayer("Player"), LayerMask.NameToLayer("Props"), true);
            Physics2D.IgnoreLayerCollision(LayerMask.NameToLayer("Props"), LayerMask.NameToLayer("Props"), true);
            Hook.Instance.ItemCaught += ItemCaught;
            Hook.Instance.FishCaught += FishCaught;
            _animator = _3dModel.GetComponent<Animator>();
            _auraStep = _auraMaxAlpha / _maxCapacity;
            SetSpeed();
        }

        private void OnMove(InputValue value)
        {
            _newSpeed = value.Get<Vector2>().normalized * _speed;
            _animator.SetBool("IsSwimming", !Mathf.Approximately(_newSpeed.magnitude, 0f));
                

            if (_newSpeed.x != 0f)
            {
                bool newFlip = _newSpeed.x > 0f;

                if (newFlip != _renderer.flipX)
                {
                    var flipedRotation = transform.rotation;
                    flipedRotation.z = -flipedRotation.z;
                    transform.rotation = flipedRotation;
                }

                _renderer.flipX = newFlip;

                var modelRotation = _3dModel.transform.localEulerAngles;
                modelRotation.y = _renderer.flipX ? 90f : -90f;
                _3dModel.transform.localEulerAngles = modelRotation;

            }

            _newAngles = transform.eulerAngles;
            _newAngles.z = Vector2.SignedAngle(_renderer.flipX ? Vector2.right : Vector2.left, _newSpeed);
        }

        private void Update()
        {
            _rb.linearVelocity = Vector2.Lerp(_rb.linearVelocity, _newSpeed, _inertiaMovementFactor * Time.deltaTime);
            transform.rotation = Quaternion.Slerp(transform.rotation, Quaternion.Euler(_newAngles), _inertiaRotationFactor * Time.deltaTime);
        }

        private IEnumerator ChangeSpeedX(float newSpeed)
        {
            yield return new WaitForSeconds(0.5f);
            _rb.linearVelocity = new(newSpeed, _rb.linearVelocity.y);
        }

        private void OnJump()
        {

            if (_items.Count == 0 && _takenItem == null)
                return;

            if (_takenItem == null)
            {
                _takenItem = _items.Last();
                _takenItem.Drag(_itemHandler);
                _audioSource.Play();
            }
            else
            {
                _takenItem.Drop();
                _takenItem = null;
            }
        }

        private void OnTriggerEnter2D(Collider2D collision)
        {
            if (collision.TryGetComponent<Item>(out var item))
            {
                if (_capacity >= item.Weight && !_items.Contains(item))
                    _items.Add(item);
            }
        }

        private void OnTriggerExit2D(Collider2D collision)
        {
            if (collision.TryGetComponent<Item>(out var item))
                _items.Remove(item);
        }

        private void ItemCaught(Item item)
        {
            if (_collectedItems.Contains(item.gameObject.name))
                return;

            if (_capacity < _maxCapacity)
            {
                _capacity++;
                SetLevel();
            }

            _collectedItems.Add(item.gameObject.name);
        }

        private void FishCaught(object _, EventArgs __)
        {
            if (_capacity > 1)
            {
                //_capacity--;
                //SetLevel();
            }
        }

        private void SetLevel()
        {
            SetSpeed();
            Color newColor = _renderer.color;
            newColor.a = _capacity * _auraStep;
            _renderer.color = newColor;
            OnLevelChange?.Invoke(_capacity);
        }

        private void SetSpeed() => _speed = _initialSpeed + _speedDelta * (_capacity);
    }
}