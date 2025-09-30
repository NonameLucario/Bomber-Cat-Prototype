using UnityEngine;

public class ScrapSpots : Interactable
{
    private void Awake()
    {
        prompMassage = "Scrap - [E]";
        
    }

    public override void Interact()
    {
        EventManager.Instance.OnAddScarp?.Invoke(3);
        //MessageManager.instance.SendMsg("", "<i>You Pick Up 10 Scrap</i>");
    }
}
