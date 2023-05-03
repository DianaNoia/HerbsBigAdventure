using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class LSUIManager : MonoBehaviour
{
    public static LSUIManager instance;

    public Text lNameText;
    public GameObject lNamePanel;

    public Text coinsText;
    public Text normalBoltsText;
    public Text goldenBoltsText;

    private void Awake()
    {
        instance = this;
    }

    private void Start()
    {
        lNamePanel.SetActive(false);
    }

    private void Update()
    {
        coinsText.text= GameManager.instance.currentGems.ToString();
        normalBoltsText.text = GameManager.instance.currentNormalBolts.ToString();
        goldenBoltsText.text = GameManager.instance.currentGoldenBolts.ToString();
    }
}