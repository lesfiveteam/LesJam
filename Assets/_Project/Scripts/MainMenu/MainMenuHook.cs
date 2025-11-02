using UnityEditor;
using UnityEngine;
using UnityEngine.UI;

public class MainMenuHook : MonoBehaviour
{
    [SerializeField] Button hookButton;
    [SerializeField] LevelToLoad levelToLoad = LevelToLoad.MainScene;

    private enum LevelToLoad { MainScene, MiniGame1, MiniGame2, MiniGame3 }

    private void Start()
    {
        switch (levelToLoad)
        { 
            case LevelToLoad.MainScene:
                hookButton.onClick.AddListener(() => { FaderManager._instance.Load_MainScene(); });
                break;
            case LevelToLoad.MiniGame1:
                hookButton.onClick.AddListener(() => { FaderManager._instance.Load_MiniGame1(); });
                break;
            case LevelToLoad.MiniGame2:
                hookButton.onClick.AddListener(() => { FaderManager._instance.Load_MiniGame2(); });
                break;
            case LevelToLoad.MiniGame3:
                hookButton.onClick.AddListener(() => { FaderManager._instance.Load_MiniGame3(); });
                break;
        }
    }
}
