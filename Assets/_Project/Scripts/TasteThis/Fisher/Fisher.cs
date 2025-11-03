using UnityEngine;

namespace FishingHim.TasteThis
{
    public class Fisher : MonoBehaviour
    {
        private Animator _animator;

        private void Start() => _animator = GetComponent<Animator>();

        public void Reset() => _animator.SetTrigger("Reset");
    }
}