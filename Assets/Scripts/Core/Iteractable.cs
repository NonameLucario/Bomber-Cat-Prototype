using UnityEngine;

public class Interactable : MonoBehaviour
{
    public string prompMassage = "prompMassage = [null]!";

    public virtual void Interact()
    {
        Debug.Log("BaseIteractable.Iteract()!");
    }
}
