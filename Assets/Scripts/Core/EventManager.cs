using System;
using UnityEngine;

public class EventManager : MonoBehaviour
{
    public static EventManager Instance;

    public delegate bool TryCreateBombAction();
    public TryCreateBombAction OnTryCreateBomb;
    public Action<int> OnAddScarps;
    public Action<int> OnAddFlowers;

    private void Awake()
    {
        Instance = this;
    }
}
