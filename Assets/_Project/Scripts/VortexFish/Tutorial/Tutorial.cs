using FishingHim.Common;
using UnityEngine;
using UnityEngine.InputSystem;
using UnityEngine.EventSystems;
using System.Collections;

namespace FishingHim.VortexFish.Tutorial
{
    public class Tutorial : MonoBehaviour, IPointerClickHandler
    {
        public float time = 2f;
        public void OnPointerClick(PointerEventData eventData)
        {
            Time.timeScale = 1f;
            SoundsManager.Instance.PlaySound(SoundType.VortexFishStart);
            Destroy(gameObject);
        }
    }
}

