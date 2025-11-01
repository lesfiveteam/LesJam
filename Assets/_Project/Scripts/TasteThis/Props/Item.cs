using UnityEngine;

namespace FishingHim.TasteThis
{
    public class Item : MonoBehaviour
    {
        private Rigidbody2D _rb;

        public float Weight => _rb.mass;

        public delegate void ItemCaught(Item item);
        public event ItemCaught OnItemCaught;

        private void Start()
        {
            _rb = GetComponent<Rigidbody2D>();
        }

        public void Drop()
        {
            transform.parent = null;
            _rb.bodyType = RigidbodyType2D.Dynamic;
            _rb.freezeRotation = false;
        }

        public void Drag(Transform carrier)
        {
            _rb.bodyType = RigidbodyType2D.Kinematic;
            _rb.freezeRotation = true;
            _rb.linearVelocity = Vector2.zero;
            transform.parent = carrier;
            transform.localPosition = Vector2.zero;
        }

        public bool IsRaised() => _rb.bodyType == RigidbodyType2D.Kinematic;

        private void OnTriggerEnter2D(Collider2D collision)
        {
            if (collision.TryGetComponent<Hook>(out var hook))
            {
                Destroy(gameObject);
                OnItemCaught?.Invoke(this);
            }
        }
    }
}