using FishingHim.FishAndFisherman.Fish;
using System.Collections;
using UnityEngine;
using FishingHim.Common;

namespace FishingHim.FishAndFisherman.Sections
{
    public class SectionsContoroller : Singleton<SectionsContoroller>
    {
        [SerializeField] private SoundType[] _musics;
        [SerializeField] private int _sectionsCount;
        [SerializeField] private float _victoryDelay;
        [SerializeField] private float _nextSectionDistance;
        [SerializeField] private Transform _mainCamera;
        [SerializeField] private FishController _fishController;
        [SerializeField] private GameObject _fishermanHitEffect;

        private int _sectionIndex;

        public int SectionIndex => _sectionIndex;

        private void Start()
        {
            //SoundsManager.Instance.PlaySound(_musics[_sectionIndex]);
        }

        public void GoToNextSection()
        {
            _sectionIndex++;

            if (_sectionIndex == _sectionsCount)
            {
                StartCoroutine(VictoryRoutine());
            }
            else
            {
                //SoundsManager.Instance.PlaySound(_musics[_sectionIndex]);
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

        private IEnumerator VictoryRoutine()
        {
            _fishController.SectionJump(_nextSectionDistance);
            yield return new WaitForSeconds(_victoryDelay);
            _fishermanHitEffect.SetActive(true);
            ProgressManager.instance.Win(1);
        }
    }
}