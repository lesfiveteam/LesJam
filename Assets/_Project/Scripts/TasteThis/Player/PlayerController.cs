using System.Collections;
using UnityEngine;
using UnityEngine.InputSystem;
using UnityEngine.UIElements;

namespace FishingHim.TasteThis
{
    public class PlayerController : MonoBehaviour
    {
        [SerializeField]
        private float _speed = 1f;
        //[SerializeField]
        //private float _rotateSpeed = 1f;
        [SerializeField]
        private float _rotateMaxAngle = 25f;
        private Rigidbody2D _rb;
        private Item _item;
        private float _capacity = 1f;
        private SpriteRenderer _renderer;
       // private float _rotationDelay = 0.25f;
        //private IEnumerator rotateCoroutine;

        private void Start()
        {
            _rb = GetComponent<Rigidbody2D>();
            _renderer = GetComponent<SpriteRenderer>();
            Physics2D.IgnoreLayerCollision(LayerMask.NameToLayer("Player"), LayerMask.NameToLayer("Props"), true);
            Physics2D.IgnoreLayerCollision(LayerMask.NameToLayer("Props"), LayerMask.NameToLayer("Props"), true);
        }

        private void OnMove(InputValue value)
        {
            var movement = value.Get<Vector2>().normalized * _speed;

            if (movement.x != 0f && _rb.linearVelocity.x * movement.x <= 0f)
                _renderer.flipX = movement.x > 0f;

            //var angles = transform.eulerAngles;
            
            //if (movement.y == 0f)
            //    angles.z = 0f;
            //else if (movement.y > 0f)
            //    angles.z = _rotateMaxAngle * (movement.x > 0f ? 1 : -1);
            //else
            //    angles.z = _rotateMaxAngle * (movement.x < 0f ? 1 : -1);

            //transform.eulerAngles = angles;


            //StartCoroutine(RotateFish(_rotateMaxAngle));

            _rb.linearVelocity = movement;
        }

        //private IEnumerator RotateFish(float angle)
        //{

        //    while (transform.eulerAngles.z != angle)
        //    {
        //        transform.Rotate(transform.forward, _rotateSpeed * _rotationDelay);
        //        yield return new WaitForSeconds(_rotationDelay);
        //    }
        //}

        private void OnInteract()
        {
            if (_item == null)
                return;

            if (_item.IsRaised())
            {
                _item.Drop();
                _item.OnItemCaught -= OnItemCaught;
            }
            else
            {
                _item.Drag(transform);
                _item.OnItemCaught += OnItemCaught;
            }
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

        private void OnItemCaught(Item _)
        {
            _capacity++;
        }
    }
}