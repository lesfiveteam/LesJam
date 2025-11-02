using System.Collections;
using UnityEngine;


namespace FishingHim.TasteThis
{
    public class SmallFish : MonoBehaviour, ICatchable
    {
        public enum SmallFishType
        {
            Slow,
            Medium,
            Fast
        };

        private enum FishDirection
        {
            TowardsHook = 1,
            AwayFromHook = -1
        }

        [SerializeField] private SmallFishType _type;
        private Hook _hook;

        private float _slowSpeed = 0.6f;
        private float _mediumSpeed = 1f;
        private float _fastSpeed = 1.4f;
        private float _escapeSpeed = 1f;

        private float _sinTime = 0f;
        // Ќастраиваемые параметры движени€ средних рыб
        private float _sinAmplitude = 1.5f;
        private float _sinFrequency = 2f;

        private bool _isEscaping = false;
        private float _escapingTimer = 0f;
        private float _maxEscapingTimer = 2f;

        public SmallFishType FishType 
        {
            get => _type;
            set {  _type = value; }
        }

        private bool _isWobbling = true;
        private float _startWobblingSeconds = 2f;
        private float _wobblingSpeed;

        private void Start()
        {
            _hook = Hook.Instance;
            _hook.FishCaught += Hook_OnFishCaught;
            _wobblingSpeed = _slowSpeed / 2f;
            RotateFish(FishDirection.TowardsHook);
            StartCoroutine(StartWobbling());
        }

        private void RotateFish(FishDirection fishDirection)
        {
            Vector2 direction = (_hook.GetHookPosition() - (Vector2)transform.position).normalized * (int)fishDirection;
            float angle = Mathf.Atan2(direction.y, direction.x) * Mathf.Rad2Deg;
            transform.rotation = Quaternion.Euler(0, 0, angle);
        }

        private void Hook_OnFishCaught(object sender, System.EventArgs e)
        {
            _isEscaping = true;
            RotateFish(FishDirection.AwayFromHook);
            _escapingTimer = 0f;
        }

        private IEnumerator StartWobbling()
        {
            yield return new WaitForSeconds(_startWobblingSeconds);
            _isWobbling = false;
        }

        private void Update()
        {
            if (_isWobbling)
            {
                transform.position += transform.right * _wobblingSpeed * Time.deltaTime;
                return;
            }

            if (_isEscaping)
            {
                RotateFish(FishDirection.AwayFromHook);
                transform.position += transform.right * _escapeSpeed * Time.deltaTime;
                _escapingTimer += Time.deltaTime;
                if (_escapingTimer > _maxEscapingTimer)
                {
                    _isEscaping = false;
                    RotateFish(FishDirection.TowardsHook);
                }
            }
            else
            {
                RotateFish(FishDirection.TowardsHook);
                switch (_type)
                {
                    case SmallFishType.Slow:
                        transform.position += transform.right * _slowSpeed * Time.deltaTime;
                        break;
                    case SmallFishType.Medium:
                        _sinTime += Time.deltaTime;
                        Vector3 baseMovement = transform.right * _mediumSpeed * Time.deltaTime;
                        Vector3 sinOffset = transform.up * Mathf.Sin(_sinTime * _sinFrequency) * _sinAmplitude * Time.deltaTime;
                        transform.position += baseMovement + sinOffset;
                        break;
                    case SmallFishType.Fast:
                        transform.position += transform.right * _fastSpeed * Time.deltaTime;
                        break;
                }
            }
        }

        public void OnDestroy()
        {
            _hook.FishCaught -= Hook_OnFishCaught;
        }

        public void Drag(Transform carrier)
        {
            var rb = GetComponent<Rigidbody2D>();
            Destroy(rb);
            transform.parent = carrier;
            transform.localPosition = Vector3.zero;
            Destroy(this);
        }    
    }
}