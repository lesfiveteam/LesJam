using FishingHim.FishAndFisherman.Fish;
using System.Collections;
using UnityEngine;

namespace FishingHim.FishAndFisherman.Sections
{
    public class SectionsContoroller : MonoBehaviour
    {
        [SerializeField] private int _sectionsCount;
        [SerializeField] private float _nextSectionDistance;
        [SerializeField] private Transform _mainCamera;
        [SerializeField] private FishController _fishController;

        private int _sectionIndex;

        public int SectionIndex => _sectionIndex;

        public void GoToNextSection()
        {
            _sectionIndex++;

            if (_sectionIndex == _sectionsCount)
            {
                Victory();
            }
            else
            {
                _fishController.SectionJump(_nextSectionDistance);
                StartCoroutine(MoveCameraSmoothly(_nextSectionDistance));
            }
        }

        private IEnumerator MoveCameraSmoothly(float distance)
        {
            Vector3 startPos = _mainCamera.position;
            Vector3 targetPos = startPos + Vector3.forward * distance;
            float duration = _fishController.JumpDuration;
            float timer = 0f;

            while (timer < duration)
            {
                timer += Time.deltaTime;
                float progress = timer / duration;
                _mainCamera.position = Vector3.Lerp(startPos, targetPos, progress);
                yield return null;
            }

            _mainCamera.position = targetPos;
        }

        private void Victory()
        {
            Debug.Log("Victory");
        }
    }
}