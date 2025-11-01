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
        public float leftBound = -2f;
        public float rightBound = 2f;
        public float decelerationSpeed = 1f;

        [Header("Физика")]
        public ForceMode forceMode = ForceMode.VelocityChange;

        [Header("Ярость")]
        [SerializeField]
        private float rageTime = 3f;

        private Rigidbody rb;
        private Vector3 movement;

        public bool InRage { get; private set; }

        void Start()
        {
            rb = GetComponent<Rigidbody>();
            if (rb == null)
            {
                Debug.LogError("Нет Rigidbody на Fish");
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

        /**
         * Войти в ярость
         */
        public void EnterInRage()
        {
            Debug.Log("Вошел в ярость - доделайте меня");
            InRage = true;
        }

        /**
         * Выход из ярости
         */
        private void ExitFromRage()
        {
            Debug.Log("Вышел в ярость - доделайте меня");
            InRage = false;
        }

        /**
         * Подождать и выйти из ярости
         */
        private IEnumerator WaitAndExitFromRage()
        {
            yield return new WaitForSeconds(rageTime);
            ExitFromRage();
        }
    }
}

