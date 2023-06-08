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

    // Images for the bolts in level select
    [SerializeField]
    public GameObject goldenBolts1,
                        goldenBolts2,
                        goldenBolts3,
                        placeHolder1,
                        placeHolder2,
                        placeHolder3,
                        textIfLevelUnlocked, 
                        textIfLevelLocked;

    private void Awake()
    {
        instance = this;
    }

    private void Start()
    {
        // Start panel disabled
        lNamePanel.SetActive(false);

        // Start golden bolts images disabled
        goldenBolts1.SetActive(false);
        goldenBolts2.SetActive(false);
        goldenBolts3.SetActive(false);

        // Start placeholder images enabled
        placeHolder1.SetActive(true);
        placeHolder2.SetActive(true);
        placeHolder3.SetActive(true);

        // Starts text for if level locked/unlocked
        textIfLevelUnlocked.SetActive(false);
        textIfLevelLocked.SetActive(false);
    }
}