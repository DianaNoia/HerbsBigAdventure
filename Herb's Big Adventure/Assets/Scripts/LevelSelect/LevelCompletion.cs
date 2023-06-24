using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

// Class for the level % completion calculations
public class LevelCompletion : MonoBehaviour
{
    [SerializeField]
    private LSLevelEntry lsLevelEntry;

    private UIManager uim;

    private float normalBoltsQuantity,
                    goldenBoltsQuantity,
                    totalBoltsCollected,
                    totalBoltsInLevel, 
                    currentPercent;

    private void Start()
    {
        uim = FindObjectOfType<UIManager>();    
    }

    // Calculates level % completion
    public void CalculatePercentComplete()
    {
        // Gets the number of normal bolts collected
        if (PlayerPrefs.HasKey(lsLevelEntry.levelName + "_normalBolts"))
        {
            normalBoltsQuantity = PlayerPrefs.GetInt(lsLevelEntry.levelName + "_normalBolts");
        }
        else
        {
            normalBoltsQuantity = 0; // Default value if the key is not found
        }
        // Gets the number of golden bolts collected
        if (PlayerPrefs.HasKey(lsLevelEntry.levelName + "_goldenBolts"))
        {
            goldenBoltsQuantity = PlayerPrefs.GetInt(lsLevelEntry.levelName + "_goldenBolts");
        }
        else
        {
            goldenBoltsQuantity = 0; // Default value if the key is not found
        }

        // Total bolts player has
        totalBoltsCollected = normalBoltsQuantity + goldenBoltsQuantity;

        // Total bolts to be collected inlevel
        totalBoltsInLevel = lsLevelEntry.totalNBInLevel + lsLevelEntry.totalGBInLevel;

        // Current percentage 
        currentPercent = (totalBoltsCollected / totalBoltsInLevel) * 100;

        SetLevelPercentInScreen();
    }

    // Updates the level % in the pause screens when player enters a level collider
    public void SetLevelPercentInScreen()
    {
        if (lsLevelEntry.levelName == "Level1")
        {
            uim.level1PercentageText.text = $"{currentPercent:F0}%";
        }
        if (lsLevelEntry.levelName == "Level2")
        {
            uim.level2PercentageText.text = $"{currentPercent:F0}%";
        }
        if (lsLevelEntry.levelName == "Level3")
        {
            uim.level3PercentageText.text = $"{currentPercent:F0}%";
        }
        if (lsLevelEntry.levelName == "Boss")
        {
            uim.level4PercentageText.text = $"{currentPercent:F0}%";
        }
    }
}
