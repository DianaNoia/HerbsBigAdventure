using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class LevelCompletion : MonoBehaviour
{
    [SerializeField]
    private LSLevelEntry lsLevelEntry;

    [SerializeField]
    private float totalNBInLevel,
                    totalGBInLevel;

    private float normalBoltsQuantity,
                    goldenBoltsQuantity,
                    totalBoltsCollected,
                    totalBoltsInLevel, 
                    currentPercent;

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
        totalBoltsInLevel = totalNBInLevel + totalGBInLevel;

        // Current percentage 
        currentPercent = (totalBoltsCollected / totalBoltsInLevel) * 100;

        Debug.Log($"{currentPercent:F0}%");
    }
}
