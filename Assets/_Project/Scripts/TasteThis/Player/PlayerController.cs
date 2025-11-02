using System;
using System.Collections;
using UnityEngine;
using UnityEngine.InputSystem;
using UnityEngine.UIElements;

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
        private Item _item;
        private float _capacity = 1f;
        private SpriteRenderer _renderer;
        private Vector2 _newSpeed;
        private Vector3 _newAngles;

        public event Action<float> OnLevelChange;

        private void Start()
        {
            _rb = GetComponent<Rigidbody2D>();
            _renderer = GetComponent<SpriteRenderer>();
            Physics2D.IgnoreLayerCollision(LayerMask.NameToLayer("Player"), LayerMask.NameToLayer("Props"), true);
            Physics2D.IgnoreLayerCollision(LayerMask.NameToLayer("Props"), LayerMask.NameToLayer("Props"), true);
            Hook.Instance.ItemCaught += ItemCaught;
            Hook.Instance.FishCaught += FishCaught;
            SetSpeed();
        }

        private void OnMove(InputValue value)
        {
            _newSpeed = value.Get<Vector2>().normalized * _speed;

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

        private void OnInteract()
        {
            if (_item == null)
                return;

            if (_item.IsRaised())
                _item.Drop();
            else
                _item.Drag(transform);
        }

        private void OnTriggerEnter2D(Collider2D collision)
        {
            if (collision.TryGetComponent<Item>(out var item))
            {
                if (_item == null && _capacity >= item.Weight)
                    _item = item;
            }
        }

        private void OnTriggerExit2D(Collider2D collision)
        {
            if (collision.TryGetComponent<Item>(out var item))
            {
                if (_item == item)
                    _item = null;
            }
        }

        private void ItemCaught(Item _)
        {
            if (_capacity < _maxCapacity)
            {
                _capacity++;
                SetLevel();
            }
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
            OnLevelChange?.Invoke(_capacity);
        }

        private void SetSpeed() => _speed = _initialSpeed + _speedDelta * (_capacity);
    }
}