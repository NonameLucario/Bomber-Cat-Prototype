using UnityEngine;
using UnityEngine.UI;
using TMPro;
using DG.Tweening;

public class MessageUI : MonoBehaviour
{
    public Image imageBG;
    public TMP_Text text;
    public SpriteRenderer textRenderer;

    public void Init(string _msg)
    {
        text.text = _msg;
        Invoke("Hide", 10f);
    }
    public void Hide()
    {
        imageBG.DOFade(0, 5f);
        text.DOFade(0, 5f);
        Destroy(gameObject, 5.1f);
    }
}
