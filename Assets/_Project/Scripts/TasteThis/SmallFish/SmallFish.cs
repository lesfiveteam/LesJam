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

       // private bool _isWobbling = true;
        private float _startWobblingSeconds = 3f, _minWobblingSeconds = 0.5f, _maxWobblingSeconds = 2f;
        private float _wobblingFactor = 1f;
        private float _noWobblingPeriodMin = 2f, _noWobblingPeriodMax = 10f;

        private void Start()
        {
            _hook = Hook.Instance;
            _hook.FishCaught += Hook_OnFishCaught;
            RotateFish(FishDirection.TowardsHook);
            StartCoroutine(WobblingProceed());
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

        private IEnumerator StartWobbling(float time, float wobblingFactor)
        {
            _wobblingFactor = wobblingFactor;
            yield return new WaitForSeconds(time);
            _wobblingFactor = 1f;
        }
        
        private IEnumerator WobblingProceed()
        {
            yield return StartWobbling(_startWobblingSeconds, 0.4f);

            while (true)
            {
                yield return new WaitForSeconds(Random.Range(_noWobblingPeriodMin, _noWobblingPeriodMax));
                yield return StartWobbling(Random.Range(_minWobblingSeconds, _maxWobblingSeconds), Random.Range(0.2f, 0.8f));
            }
        }

        private void Update()
        {
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
                        transform.position += transform.right * _slowSpeed * Time.deltaTime * _wobblingFactor;
                        break;
                    case SmallFishType.Medium:
                        _sinTime += Time.deltaTime;
                        Vector3 baseMovement = transform.right * _mediumSpeed * Time.deltaTime;
                        Vector3 sinOffset = transform.up * Mathf.Sin(_sinTime * _sinFrequency) * _sinAmplitude * Time.deltaTime * _wobblingFactor;
                        transform.position += baseMovement + sinOffset;
                        break;
                    case SmallFishType.Fast:
                        transform.position += transform.right * _fastSpeed * Time.deltaTime * _wobblingFactor;
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