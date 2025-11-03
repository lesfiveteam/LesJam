using FishingHim.Common;
using UnityEngine;
using UnityEngine.EventSystems;
using UnityEngine.UI;

[RequireComponent(typeof(Button))]
public class MainMenuHook : MonoBehaviour, IPointerEnterHandler, IPointerExitHandler
{
    Button hookButton;
    [SerializeField] SpriteRenderer spriteRenderer;
    [SerializeField] Material defaultMaterial;
    [SerializeField] Material outlineMaterial;
    [SerializeField] LevelToLoad levelToLoad = LevelToLoad.MainScene;

    private enum LevelToLoad { MainScene, MiniGame1, MiniGame2, MiniGame3 }

    private void Awake()
    {
        hookButton = GetComponent<Button>();
    }

    private void Start()
    {
        if (spriteRenderer != null)
        {
            spriteRenderer.material = defaultMaterial;
        }

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

    public void OnPointerEnter(PointerEventData eventData)
    {
        if (spriteRenderer == null)
            return;

        spriteRenderer.material = outlineMaterial;
        SoundsManager.Instance.PlaySound(SoundType.MainSelectMiniGame);
    }

    public void OnPointerExit(PointerEventData eventData)
    {
        if (spriteRenderer == null)
            return;

        spriteRenderer.material = defaultMaterial;
    }
}