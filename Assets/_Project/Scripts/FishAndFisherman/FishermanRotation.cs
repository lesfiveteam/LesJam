using UnityEngine;

namespace FishingHim.FishAndFisherman.Fisherman
{
    public class FishermanRotation : MonoBehaviour
    {
        [Header("Rotation Settings")]
        [SerializeField] private float _leftRotationAngle = -50f;
        [SerializeField] private float _rightRotationAngle = 50f;

        private float _targetRotation;
        private bool _isRotatingRight = true;

        private void Awake()
        {
            _targetRotation = _rightRotationAngle;
        }

        public void UpdateRotation(float rotationStep)
        {
            float currentRotation = transform.eulerAngles.y;
            currentRotation = NormalizeAngle(currentRotation);
            float newRotation = Mathf.LerpAngle(currentRotation, _targetRotation, rotationStep);
            transform.rotation = Quaternion.Euler(0f, newRotation, 0f);
        }

        public void SwitchDirection()
        {
            _isRotatingRight = !_isRotatingRight;
            _targetRotation = _isRotatingRight ? _rightRotationAngle : _leftRotationAngle;
        }

        public bool HasReachedTarget()
        {
            float currentRotation = NormalizeAngle(transform.eulerAngles.y);
            return Mathf.Abs(currentRotation - _targetRotation) < 0.5f;
        }

        private float NormalizeAngle(float angle)
        {
            angle = angle % 360f;
            if (angle > 180f)
                angle -= 360f;
            return angle;
        }
    }
}