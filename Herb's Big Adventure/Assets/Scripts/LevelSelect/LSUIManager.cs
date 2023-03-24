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
        coinsText.text= GameManager.instance.currentCoins.ToString();
        normalBoltsText.text = UIManager.instance.boltsText.ToString();
        goldenBoltsText.text = UIManager.instance.goldenBoltsText.ToString();
    }
}