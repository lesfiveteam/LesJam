using UnityEngine;
using UnityEngine.UI;

[RequireComponent(typeof(Button))]
public class MainMenuHook : MonoBehaviour
{
    Button hookButton;
    [SerializeField] LevelToLoad levelToLoad = LevelToLoad.MainScene;

    private enum LevelToLoad { MainScene, MiniGame1, MiniGame2, MiniGame3 }

    private void Awake()
    {
        hookButton = GetComponent<Button>();
    }

    private void Start()
    {
        switch (levelToLoad)
        { 
            case LevelToLoad.MainScene:
                hookButton.onClick.AddListener(() => { SceneLoader.Instance.Load_MainScene(); });
                break;
            case LevelToLoad.MiniGame1:
                hookButton.onClick.AddListener(() => { SceneLoader.Instance.Load_MiniGame1(); });
                break;
            case LevelToLoad.MiniGame2:
                hookButton.onClick.AddListener(() => { SceneLoader.Instance.Load_MiniGame2(); });
                break;
            case LevelToLoad.MiniGame3:
                hookButton.onClick.AddListener(() => { SceneLoader.Instance.Load_MiniGame3(); });
                break;
        }
    }
}
