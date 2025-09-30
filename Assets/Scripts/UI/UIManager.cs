using UnityEngine;
using TMPro;
using System;
using UnityEngine.UI;
using DG.Tweening;

public class UIManager : MonoBehaviour
{
    public static UIManager Instance;

    [SerializeField]
    private TMP_Text scrapCountText;
    [SerializeField]
    private Image scrapCountImageFillable;

    private Tween tweenScrapCount;
    private float scrap;
    private float maxScrap;
    private float scrapCountSmoothFill;

    [SerializeField]
    private TMP_Text devText;
    public bool devGrounded;
    public bool devSlideGrounded;

    private void Awake()
    {
        Instance = this;
    }

    public void SetScrap—ount(int scrap, int maxScrap)
    {  
        this.scrap = scrap;
        this.maxScrap = maxScrap;
        scrapCountText.text = $"{scrap}";
        tweenScrapCount = scrapCountText.transform.DOShakeScale(1f);
    }

    private void Update()
    {
        UpdateDev();
        Scrap—ountUpdate();
    }
    private void Scrap—ountUpdate()
    {
        scrapCountSmoothFill = Mathf.Lerp(scrapCountSmoothFill, (scrap/maxScrap), 0.05f);
        scrapCountImageFillable.fillAmount = scrapCountSmoothFill;


    }
    private void UpdateDev()
    {
        devText.text = $"Grnd:{devGrounded}/{devSlideGrounded}";

    }
}
