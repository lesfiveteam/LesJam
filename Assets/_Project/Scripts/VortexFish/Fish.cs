using System.Collections;
using UnityEngine;
using UnityEngine.InputSystem;

namespace FishingHim.VortexFish
{
    /**
     * Класс для управление рыбой в миниигре про VortexFish
     */
    public class Fish : MonoBehaviour
    {
        [Header("Движение")]
        public float moveSpeed = 1f;
        public float moveSmoothness = 0.8f;
        public float decelerationSpeed = 1f;

        [Header("Физика")]
        public ForceMode forceMode = ForceMode.VelocityChange;

        private Rigidbody rb;
        private Vector3 movement;

        public bool InRage { get; private set; }

        private Animator animator;

        void Start()
        {
            rb = GetComponent<Rigidbody>();
            if (rb == null)
            {
                Debug.LogError("Нет Rigidbody на Fish");
            }
            animator = GetComponent<Animator>();
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
                float horizontalMovement = movement.x;

                rb.linearVelocity = new Vector2(horizontalMovement * moveSpeed, rb.linearVelocity.y);
            }
            else
            {
                // Ничего не нажато
                rb.linearVelocity *= moveSmoothness;
            }
        }
        /**
         * Ускоряет или замедляет анимацию
         */
        public void SetAnimationSpeed(float speed)
        {
            if (animator != null)
            {
                animator.speed = speed;
            }
        }
    }
}

