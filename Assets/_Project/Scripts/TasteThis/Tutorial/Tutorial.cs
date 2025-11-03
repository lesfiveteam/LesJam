using System.Collections;
using UnityEngine;

public class Tutorial : MonoBehaviour
{
    [SerializeField]
    private GameObject _pause;
    [SerializeField]
    private GameObject _advice, _startButton;

    private IEnumerator Start()
    {
        yield return new WaitForSeconds(2f);
        _advice.SetActive(true);
        _startButton.SetActive(true);
        Time.timeScale = 0f;
    }

    public void OnExit(GameObject sender)
    {
        Destroy(sender);
        Time.timeScale = 1f;
        _pause.SetActive(true);
        Destroy(gameObject);
    }
}
