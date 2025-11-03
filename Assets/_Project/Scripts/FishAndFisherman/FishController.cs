using FishingHim.Common;
using System.Collections;
using UnityEngine;
using UnityEngine.InputSystem;

namespace FishingHim.FishAndFisherman.Fish
{
    public class FishController : MonoBehaviour
    {
        [Header("References")]
        [SerializeField] private Transform _fish;

        [Header("Input")]
        [SerializeField] private InputActionReference _movementAction;
        [SerializeField] private InputActionReference _jumpAction;
        [SerializeField] private InputActionReference _rollAction;

        [Header("Movement Settings")]
        [SerializeField] private float _moveSpeed = 5f;
        [SerializeField] private float _leftBoundary = -6f;
        [SerializeField] private float _rightBoundary = 6f;

        [Header("Rotation Settings")]
        [SerializeField] private float _rotationTime = 10f;
        [SerializeField] private float _leftRotationAngle = -30f;
        [SerializeField] private float _rightRotationAngle = 30f;

        [Header("Jump Settings")]
        [SerializeField] private float _jumpHeight = 2f;
        [SerializeField] private float _jumpDuration = 0.8f;
        [SerializeField] private float _jumpRotationSpeed = 360f;
        [SerializeField]
        private AnimationCurve _jumpCurve = new AnimationCurve(
            new Keyframe(0f, 0f, 0f, 3f),
            new Keyframe(0.3f, 0.8f, 1f, 1f),
            new Keyframe(0.5f, 1f, 0f, 0f),
            new Keyframe(0.7f, 0.8f, -1f, -1f),
            new Keyframe(1f, 0f, -3f, 0f)
        );

        [Header("Roll Settings")]
        [SerializeField] private float _rollDuration = 0.5f;
        [SerializeField] private float _rollSpeedMultiplier = 1.5f;

        // Переменные состояния
        private Vector3 _startPosition;
        private bool _isJumping = false;
        private bool _isRolling = false;
        private float _jumpTimer = 0f;
        private float _rollTimer = 0f;
        private Quaternion _preJumpRotation;
        private float _currentJumpRotation = 0f;
        private float _currentRollRotation = 0f;
        private float _lastMoveDirection = 1f;
        private float _jumpStartDirection = 1f;
        private float _rollStartDirection = 1f;
        private float _originalMoveSpeed;
        private Vector2 _movementInput;

        public bool IsRolling => _isRolling;
        public float JumpDuration => _jumpDuration;


        private void Awake()
        {
            _startPosition = _fish.position;
            _preJumpRotation = _fish.rotation;
            _originalMoveSpeed = _moveSpeed;
        }

        private void OnEnable()
        {
            _movementAction.action.Enable();
            _jumpAction.action.Enable();
            _rollAction.action.Enable();
            _jumpAction.action.performed += OnJumpPerformed;
            _rollAction.action.performed += OnRollPerformed;
        }

        private void OnDisable()
        {
            _jumpAction.action.performed -= OnJumpPerformed;
            _rollAction.action.performed -= OnRollPerformed;
            _movementAction.action.Disable();
            _jumpAction.action.Disable();
            _rollAction.action.Disable();
        }

        private void Update()
        {
            _movementInput = _movementAction.action.ReadValue<Vector2>();
            HandleMovement();
            HandleJump(); 
            HandleRoll(); 
        }

        private void OnJumpPerformed(InputAction.CallbackContext context)
        {
            if (!_isJumping && !_isRolling)
            {
                SoundsManager.Instance.PlaySound(SoundType.FishAndFishermanJump);
                _isJumping = true;
                _jumpTimer = 0f;
                _currentJumpRotation = 0f;
                _preJumpRotation = _fish.rotation;
                _jumpStartDirection = _lastMoveDirection;
            }
        }

        private void OnRollPerformed(InputAction.CallbackContext context)
        {
            if (!_isRolling && !_isJumping)
            {
                _isRolling = true;
                _rollTimer = 0f;
                _currentRollRotation = 0f;
                _rollStartDirection = _lastMoveDirection;
                _moveSpeed = _originalMoveSpeed * _rollSpeedMultiplier;
            }
        }

        private void HandleRoll()
        {
            if (_isRolling)
            {
                _rollTimer += Time.deltaTime;
                float rollProgress = _rollTimer / _rollDuration;

                if (rollProgress < 1f)
                {
                    _currentRollRotation = 360f * rollProgress;
                    float rotationDirection = _rollStartDirection > 0 ? -1f : 1f;
                    float zRotation = _currentRollRotation * rotationDirection;

                    Quaternion rollRotation = Quaternion.Euler(
                        _preJumpRotation.eulerAngles.x,
                        _preJumpRotation.eulerAngles.y,
                        zRotation
                    );
                    _fish.rotation = rollRotation;
                }
                else
                {
                    SoundsManager.Instance.PlaySound(SoundType.FishAndFishermanRoll);
                    _isRolling = false;
                    _currentRollRotation = 0f;
                    _moveSpeed = _originalMoveSpeed;
                    _fish.rotation = _preJumpRotation;
                }
            }
        }

        private void HandleMovement()
        {
            float moveInput = _movementInput.x;

            if (moveInput != 0f && !_isJumping)
            {
                _lastMoveDirection = moveInput;
            }

            if (moveInput != 0f)
            {
                Vector3 newPosition = _fish.position + Vector3.right * moveInput * _moveSpeed * Time.deltaTime;
                newPosition.x = Mathf.Clamp(newPosition.x, _leftBoundary, _rightBoundary);

                if (_isJumping)
                {
                    newPosition.y = _fish.position.y;
                }
                else
                {
                    newPosition.y = _startPosition.y;
                }

                _fish.position = newPosition;

                if (!_isJumping && !_isRolling)
                {
                    RotateFishTowardsMovement(moveInput);
                    _preJumpRotation = _fish.rotation;
                }
            }
            else if (!_isJumping && !_isRolling)
            {
                LookStraight();
            }
        }

        private void HandleJump()
        {
            if (_isJumping)
            {
                _jumpTimer += Time.deltaTime;
                float jumpProgress = _jumpTimer / _jumpDuration;

                if (jumpProgress < 1f)
                {
                    float jumpHeight = _jumpCurve.Evaluate(jumpProgress) * _jumpHeight;
                    Vector3 newPosition = _fish.position;
                    newPosition.y = _startPosition.y + jumpHeight;
                    _fish.position = newPosition;

                    _currentJumpRotation += _jumpRotationSpeed * Time.deltaTime;
                    float rotationDirection = _jumpStartDirection > 0 ? -1f : 1f;
                    float zRotation = _currentJumpRotation * rotationDirection;

                    Quaternion jumpRotation = Quaternion.Euler(
                        _preJumpRotation.eulerAngles.x,
                        _preJumpRotation.eulerAngles.y,
                        zRotation
                    );
                    _fish.rotation = jumpRotation;
                }
                else
                {
                    SoundsManager.Instance.PlaySound(SoundType.FishAndFishermanFall);
                    _isJumping = false;
                    Vector3 newPosition = _fish.position;
                    newPosition.y = _startPosition.y;
                    _fish.position = newPosition;
                    _fish.rotation = _preJumpRotation;
                    _currentJumpRotation = 0f;
                }
            }
        }

        public void SectionJump(float jumpDistance)
        {
            StartCoroutine(SectionJumpCoroutine(jumpDistance));
        }

        private IEnumerator SectionJumpCoroutine(float jumpDistance)
        {
            _movementAction.action.Disable();
            _jumpAction.action.Disable();
            _rollAction.action.Disable();

            while (_isJumping || _isRolling)
            {
                yield return null;
            }

            SoundsManager.Instance.PlaySound(SoundType.FishAndFishermanJump);
            _isJumping = true;
            Vector3 startPos = _fish.position;
            Vector3 targetPos = startPos + Vector3.forward * jumpDistance;
            Quaternion startRotation = _fish.rotation;
            float jumpTimer = 0f;
            float currentRotation = 0f;

            while (jumpTimer < _jumpDuration)
            {
                jumpTimer += Time.deltaTime;
                float progress = jumpTimer / _jumpDuration;
                float jumpHeight = _jumpCurve.Evaluate(progress) * _jumpHeight;
                Vector3 newPosition = Vector3.Lerp(startPos, targetPos, progress);
                newPosition.y = _startPosition.y + jumpHeight;
                _fish.position = newPosition;
                currentRotation += _jumpRotationSpeed * Time.deltaTime;
                Quaternion jumpRotation = Quaternion.Euler(
                    currentRotation,
                    startRotation.eulerAngles.y,
                    startRotation.eulerAngles.z
                );
                _fish.rotation = jumpRotation;

                yield return null;
            }

            _fish.position = targetPos;
            _fish.rotation = startRotation;
            _isJumping = false;
            SoundsManager.Instance.PlaySound(SoundType.FishAndFishermanFall);
            _movementAction.action.Enable();
            _jumpAction.action.Enable();
            _rollAction.action.Enable();
        }

        private void RotateFishTowardsMovement(float moveInput)
        {
            float targetRotationY = moveInput > 0 ? _rightRotationAngle : _leftRotationAngle;
            Quaternion targetRotation = Quaternion.Euler(0f, targetRotationY, 0f);
            _fish.rotation = Quaternion.Lerp(_fish.rotation, targetRotation, _rotationTime * Time.deltaTime);
        }

        private void LookStraight()
        {
            Quaternion targetRotation = Quaternion.Euler(0f, 0f, 0f);
            _fish.rotation = Quaternion.Lerp(_fish.rotation, targetRotation, _rotationTime * Time.deltaTime);
            _preJumpRotation = _fish.rotation;
        }
    }
}