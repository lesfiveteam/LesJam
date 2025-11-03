using System.Collections;
using UnityEngine;
using UnityEngine.SceneManagement;

public class SceneLoader : MonoBehaviour
{
    private const string Scene_Main = "MainScene";
    private const string Scene_MiniGame1 = "VortexFishScene";
    private const string Scene_MiniGame2 = "FishAndFishermanScene";
    private const string Scene_MiniGame3 = "TasteThis";
    private const string Scene_EndGame = "EndGameScene";


    private bool _isLoading;

    public static SceneLoader Instance;

    private void Awake()
    {
        if (Instance != null)
        {
            Destroy(gameObject);
            return;
        }

        Instance = this;

        DontDestroyOnLoad(gameObject);
    }

    public void Load_MainScene()
    {
        LoadScene(Scene_Main);
    }

    public void Load_MiniGame1()
    {
        LoadScene(Scene_MiniGame1);
    }

    public void Load_MiniGame2()
    {
        LoadScene(Scene_MiniGame2);
    }

    public void Load_MiniGame3()
    {
        LoadScene(Scene_MiniGame3);
    }

    public void Load_EndGame()
    {
        LoadScene(Scene_EndGame);
    }

    private void LoadScene(string sceneName)
    {
        Debug.Log(sceneName);
        if(_isLoading)
            return;

        var currentScene = SceneManager.GetActiveScene().name;

        if (currentScene == sceneName)
            throw new System.Exception("You are trying to load already loaded scene.");

        StartCoroutine(LoadSceneRoutine(sceneName));
    }

    private IEnumerator LoadSceneRoutine(string sceneName)
    {
        _isLoading = true;

        var waitFading = true;
        Fader.Instance.FadeIn(() => waitFading = false);

        while (waitFading)
            yield return null;

        var async = SceneManager.LoadSceneAsync(sceneName);
        async.allowSceneActivation = false;

        while (async.progress < 0.9f)
            yield return null;

        async.allowSceneActivation = true;

        waitFading = true;
        Fader.Instance.FadeOut(() => waitFading = false);

        while (waitFading)
            yield return null;

        _isLoading = false;
    }
}
