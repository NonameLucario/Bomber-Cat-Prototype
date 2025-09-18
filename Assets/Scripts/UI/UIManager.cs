using UnityEngine;
using TMPro;
using System;

public class UIManager : MonoBehaviour
{
    public static UIManager Instance;

    [SerializeField]
    private TMP_Text scrapCountText;


    private void Awake()
    {
        Instance = this;
    }

    public void ScrapCountUpdate(int scrap, int maxScrap)
    {
        scrapCountText.text = $"{scrap} / {maxScrap}";
    }
}
