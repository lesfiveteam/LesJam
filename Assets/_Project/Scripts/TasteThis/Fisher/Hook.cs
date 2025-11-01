using System;
using UnityEngine;
using static FishingHim.TasteThis.Item;

public class Hook : MonoBehaviour
{
    public static Hook Instance { get; private set; }

    private void Awake()
    {
        Instance = this;
    }

    public event EventHandler OnFishCaught;

    public Vector2 GetHookPosition()
    {
        return (Vector2)transform.position;
    }

    private void OnTriggerEnter2D(Collider2D collision)
    {
        if (collision.TryGetComponent<SmallFish>(out var fish))
        {
            Destroy(collision.gameObject);
            OnFishCaught?.Invoke(this, EventArgs.Empty);
        }
    }
}
