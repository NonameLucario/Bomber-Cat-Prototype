using DG.Tweening;
using UnityEngine;


public class FlowersSpots : Interactable
{
    private Tween tween;

    [SerializeField]
    private Transform meshParent;
    private void Start()
    {
        prompMassage = "Use - [E]";
    }
    public override void Interact()
    {
        //EventManager.Instance.OnTryCreateBomb?.Invoke();
        EventManager.Instance.OnAddFlowers?.Invoke(5);
        if (tween != null) tween.Kill();
        meshParent.localScale = Vector3.one;
        meshParent.localPosition = Vector3.zero;
        tween = meshParent.DOPunchScale(Vector3.down * 0.025f, 0.2f);
    }
}
