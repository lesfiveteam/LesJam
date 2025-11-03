using UnityEngine;

public class ExitMinigameButton : MonoBehaviour
{
    public void OnExitMinigameButton()
    {
        SceneLoader.Instance.Load_MainScene();
    }
}