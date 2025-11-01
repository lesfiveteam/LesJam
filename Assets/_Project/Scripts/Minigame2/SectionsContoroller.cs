using UnityEngine;

namespace FishingHim.FishAndFisherman.Sections
{
    public class SectionsContoroller : MonoBehaviour
    {
        [SerializeField] private float _jumpDistance;
        [SerializeField] private int _sectionsCount;
        [SerializeField] private Transform _mainCamera;

        private int _sectionIndex;

        public int SectionIndex;

        public void GoToNextSection()
        {
            _sectionIndex++;

            if (_sectionIndex == _sectionsCount)
            {
                Victory();
            }
            else
            {

            }
        }

        public void ReturnToPreviousSection()
        {
            _sectionIndex--;

            if (_sectionIndex < 0)
            {
                _sectionIndex = 0;
            }
            else
            {

            }
        }

        private void Victory()
        {
            Debug.Log("Victory");
        }
    }
}