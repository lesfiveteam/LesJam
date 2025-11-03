using UnityEngine;
using System.Collections;

public class Fish : MonoBehaviour
{
    [SerializeField] private float timeToHook = 1f;

    private float minMoveSpeed = 0.7f;
    private float maxMoveSpeed = 1.4f;
    private float minRotationSpeed = 15f;
    private float MaxRotationSpeed = 25f;

    private void Start()
    {
        transform.Rotate(0, Random.Range(0, 360), 0);
    }

    private void Update()
    {
        CircularMovement();
    }

    private void CircularMovement()
    {
        transform.Translate(Vector3.forward * Random.Range(minMoveSpeed, maxMoveSpeed) * Time.deltaTime);
        transform.Rotate(0, Random.Range(minRotationSpeed, MaxRotationSpeed) * Time.deltaTime, 0);
    }

    public void GoToHook(Transform hook)
    {
        StartCoroutine(MoveToHook(hook));
    }

    private IEnumerator MoveToHook(Transform hook)
    {
        Vector3 startPos = transform.position;
        float timer = 0f;

        while (timer < timeToHook)
        {
            timer += Time.deltaTime;
            float progress = timer / timeToHook;

            transform.LookAt(hook);

            transform.position = Vector3.Lerp(startPos, hook.position, progress);

            yield return null;
        }
    }
}
