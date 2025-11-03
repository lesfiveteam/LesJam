using UnityEngine;

public class Tutorial : MonoBehaviour
{
    [SerializeField]
    private GameObject _pause;

    private void Start() => Time.timeScale = 0f;

    public void OnExit(GameObject sender)
    {
        Destroy(sender);
        Time.timeScale = 1f;
        _pause.SetActive(true);
        Destroy(gameObject);
    }
}
