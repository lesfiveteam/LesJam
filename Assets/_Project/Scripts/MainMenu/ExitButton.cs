using FishingHim.Common;
using UnityEngine;

public class ExitButton : MonoBehaviour
{
    public void QuitGame()
    {
        SoundsManager.Instance.PlaySound(SoundType.MainClick);
        Application.Quit();
    }
}
