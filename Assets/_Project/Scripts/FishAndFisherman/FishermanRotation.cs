using UnityEngine;
using FishingHim.Common;

namespace FishingHim.FishAndFisherman.Fisherman
{
    public class FishermanRotation : Singleton<FishermanRotation>
    {
        [Header("Rotation Settings")]
        [SerializeField] private float _leftRotationAngle = -50f;
        [SerializeField] private float _rightRotationAngle = 50f;
        [SerializeField] private float _rotationSpeed = 90f;

        private float _targetRotation;
        private bool _isRotatingRight = true;

        protected override void Awake()
        {
            base.Awake();
            _targetRotation = _rightRotationAngle;
        }

        public void UpdateRotation(float deltaTime)
        {
            float currentRotation = transform.eulerAngles.y;
            float newRotation = Mathf.MoveTowardsAngle(currentRotation, _targetRotation, _rotationSpeed * deltaTime);
            transform.rotation = Quaternion.Euler(0f, newRotation, 0f);
        }

        public void SwitchDirection()
        {
            _isRotatingRight = !_isRotatingRight;
            _targetRotation = _isRotatingRight ? _rightRotationAngle : _leftRotationAngle;
        }

        public bool HasReachedTarget()
        {
            float currentRotation = transform.eulerAngles.y;
            return Mathf.Abs(Mathf.DeltaAngle(currentRotation, _targetRotation)) < 0.5f;
        }
    }
}