using UnityEngine;

public class FlowerSpots : Interactable
{
    private void Start()
    {
        prompMassage = "Use - [E]";
    }
    public override void Interact()
    {
        EventManager.Instance.OnTryCreateBomb?.Invoke();
    }
}
