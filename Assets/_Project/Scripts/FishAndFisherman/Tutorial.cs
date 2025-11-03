using UnityEngine;
using UnityEngine.EventSystems;

namespace FishingHim.FishAndFisherman.Tutorial
{
    public class Tutorial : MonoBehaviour, IPointerClickHandler
    {
        public void OnPointerClick(PointerEventData eventData)
        {
            Destroy(gameObject);
            Time.timeScale = 1f;
        }
    }
}