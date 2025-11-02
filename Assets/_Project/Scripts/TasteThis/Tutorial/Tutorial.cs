using UnityEngine;

public class Tutorial : MonoBehaviour
{
    [SerializeField]
    private GameObject _pause;

    private void Start() => Time.timeScale = 0f;

    private void OnExit()
    {
        Destroy(gameObject);
        Time.timeScale = 1f;
        _pause.SetActive(true);
    }
}
