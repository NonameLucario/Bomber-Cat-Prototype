using UnityEngine;
using TMPro;
using System;
using UnityEngine.UI;
using DG.Tweening;

public class UIManager : MonoBehaviour
{
    public static UIManager Instance;
    [Header("Scraps UIs")]
    [SerializeField]
    private TMP_Text scrapsCountText;
    [SerializeField]
    private Image scrapsCountImageFillable;
    [SerializeField]
    private Transform scrapsCountParent;

    private Tween tweenScrapsCount;
    private float scraps;
    private float maxScraps;
    private float scrapsCountSmoothFill;

    [Header("Flowers UIs")]
    [SerializeField]
    private TMP_Text flowersCountText;
    [SerializeField]
    private Image flowersCountImageFillable;
    [SerializeField]
    private Transform flowersCountParent;


    private Tween tweenFlowersCount;
    private float flowers;
    private float maxFlowers;
    private float flowersCountSmoothFill;

    [Header("Timer UIs")]
    [SerializeField]
    private Image timerBombImageFillable;
    [SerializeField]
    private GameObject timerBombParent;

    [SerializeField]
    private TMP_Text devText;
    public bool devGrounded;
    public bool devSlideGrounded;

    private void Awake()
    {
        Instance = this;
    }

    public void SetScraps—ount(int scraps, int maxScraps)
    {  
        this.scraps = scraps;
        this.maxScraps = maxScraps;
        scrapsCountText.text = $"{scraps}";
        scrapsCountParent.DOShakePosition(3f, strength: 2);
    }
    public void SetFlowers—ount(int flowers, int maxFlowers)
    {
        this.flowers = flowers;
        this.maxFlowers = maxFlowers;
        flowersCountText.text = $"{flowers}";
        flowersCountParent.DOShakePosition(3f, strength: 2);
    }

    public void ProcessTimerCreatBomb(float time, float maxTime)
    {
        timerBombImageFillable.fillAmount = time / maxTime;
    }

    public void HideTimerCrearBomb(bool flag)
    {
        timerBombParent.SetActive(flag);
    }

    private void Update()
    {
        UpdateDev();
        Scrap—ountUpdate();
    }
    private void Scrap—ountUpdate()
    {
        scrapsCountSmoothFill = Mathf.Lerp(scrapsCountSmoothFill, (scraps/maxScraps), 0.05f);
        scrapsCountImageFillable.fillAmount = scrapsCountSmoothFill;

        flowersCountSmoothFill = Mathf.Lerp(flowersCountSmoothFill, (flowers / maxFlowers), 0.05f);
        flowersCountImageFillable.fillAmount = flowersCountSmoothFill;
    }
    private void UpdateDev()
    {
        devText.text = $"Grnd:{devGrounded}/{devSlideGrounded}";

    }
}
