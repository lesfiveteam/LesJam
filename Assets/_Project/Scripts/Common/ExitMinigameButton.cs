using FishingHim.Common;
using UnityEngine;

public class ExitMinigameButton : MonoBehaviour
{
    public void OnExitMinigameButton()
    {
        SoundsManager.Instance.PlaySound(SoundType.ExitButton);
        SceneLoader.Instance.Load_MainScene();
    }
}