using UnityEngine;
using UnityEngine.InputSystem;

namespace FishingHim.VortexFish
{
    /**
     * Класс для управление рыбой в миниигре про VortexFish
     */
    public class Fish : MonoBehaviour
    {
        [Header("Movement Settings")]
        public float moveSpeed = 1f;
        public float leftBound = -2f;
        public float rightBound = 2f;
        public float decelerationSpeed = 1f;

        [Header("Physics Settings")]
        public ForceMode forceMode = ForceMode.VelocityChange;

        private Rigidbody rb;
        private Vector3 movement;

        void Start()
        {
            rb = GetComponent<Rigidbody>();
            if (rb == null)
            {
                rb = gameObject.AddComponent<Rigidbody>();
                rb.useGravity = false;
                rb.linearDamping = 5f;
            }
        }

        void Update()
        {
            HandleInput();
        }

        void FixedUpdate()
        {
            ApplyMovement();
        }

        /**
         * Нажатие пользователем
         */
        void HandleInput()
        {
            movement = Vector3.zero;

            // @todo - проверить как правильно делать
            var keyboard = Keyboard.current;

            bool isPressedLeftKey = keyboard.leftArrowKey.isPressed || keyboard.aKey.isPressed;
            bool isPressedRightKey = keyboard.rightArrowKey.isPressed || keyboard.dKey.isPressed;
            if (isPressedLeftKey)
            {
                movement = Vector3.left;
            }
            else if (isPressedRightKey)
            {
                movement = Vector3.right;
            } 
        }

        /**
         * Применяет силу к игроку, чтобы двигать его
         */
        void ApplyMovement()
        {
            if (movement != Vector3.zero)
            {
                rb.AddForce(movement * moveSpeed, forceMode);
            }
            else
            {
                // Ничего не нажато
                rb.linearVelocity = Vector3.zero;
            }
        }
    }
}

