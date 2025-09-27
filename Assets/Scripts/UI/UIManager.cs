using UnityEngine;
using TMPro;
using System;

public class UIManager : MonoBehaviour
{
    public static UIManager Instance;

    [SerializeField]
    private TMP_Text scrapCountText;

    [SerializeField]
    private TMP_Text devText;
    public bool devGrounded;
    public bool devSlideGrounded;

    private void Awake()
    {
        Instance = this;
    }

    public void ScrapCountUpdate(int scrap, int maxScrap)
    {
        scrapCountText.text = $"{scrap} / {maxScrap}";
    }

    private void Update()
    {
        UpdateDev();
    }
    private void UpdateDev()
    {
        devText.text = $"Grnd:{devGrounded}/{devSlideGrounded}";
    }
}
