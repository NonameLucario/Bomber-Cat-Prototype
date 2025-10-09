using UnityEngine;
using DG.Tweening;

public class ScrapsSpots : Interactable
{
    private Tween tween;

    [SerializeField]
    private Transform meshParent;
    private void Awake()
    {
        prompMassage = "Scrap - [E]";
        
    }

    public override void Interact()
    {
        EventManager.Instance.OnAddScarps?.Invoke(3);
        //MessageManager.instance.SendMsg("", "<i>You Pick Up 10 Scrap</i>");
        if (tween != null) tween.Kill();
        transform.localScale = Vector3.one;
        meshParent.localPosition = Vector3.zero;
        //tween = transform.DOPunchScale(Vector3.down * 0.1f, 0.1f, vibrato: 2);
        tween = meshParent.DOShakePosition(0.25f, strength:0.01f);
    }
}
