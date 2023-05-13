using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class LSUIManager : MonoBehaviour
{
    public static LSUIManager instance;
    GameManager gm;

    public Text lNameText;
    public GameObject lNamePanel;

    public Text normalBoltsText;
    public Image goldenBolts1, goldenBolts2, goldenBolts3;

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
        normalBoltsText.text = gm.currentNormalBolts.ToString();

        // Activates images if golden bolts are collected
        if (gm.currentNormalBolts == 1)
        {
            goldenBolts1.enabled = true;
        }
        if (gm.currentNormalBolts == 2)
        {
            goldenBolts2.enabled = true;
        }
        if (gm.currentNormalBolts == 3)
        {
            goldenBolts3.enabled = true;
        }
    }
}