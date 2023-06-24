using System.Collections;
using System.Collections.Generic;
using UnityEngine;

// Controls the effects on the enemies
public class EnemyEffects : MonoBehaviour
{
    [SerializeField]
    private EnemyHealthManager enemyHealthManager;

    // The effects on unity
    [SerializeField]
    private GameObject effectShineFull, effectSmokeWeak, effectSmokeFull;

    // Swaps the effects
    public void EffectSwapper()
    {
        // Health full
        if (enemyHealthManager.currentHealth == enemyHealthManager.maxHealth)
        {
            SetActiveEffect(effectShineFull, true);
            SetActiveEffect(effectSmokeWeak, false);
            SetActiveEffect(effectSmokeFull, false);
        }
        // Health 2 below death
        else if (enemyHealthManager.currentHealth == (enemyHealthManager.minHealth + 2))
        {
            SetActiveEffect(effectShineFull, false);
            SetActiveEffect(effectSmokeWeak, true);
            SetActiveEffect(effectSmokeFull, false);
        }
        // Health 1 below death almost dead
        else if (enemyHealthManager.currentHealth == (enemyHealthManager.minHealth +1))
        {
            SetActiveEffect(effectShineFull, false);
            SetActiveEffect(effectSmokeWeak, false);
            SetActiveEffect(effectSmokeFull, true);
        }
        // Health other
        else
        {
            effectShineFull.SetActive(false);
            effectSmokeWeak.SetActive(false);
            effectSmokeFull.SetActive(false);
        }
    }
    
    // Sets active the effect if it exist in unity enemy object
    private void SetActiveEffect(GameObject effect, bool isActive)
    {
        if (effect != null)
            effect.SetActive(isActive);
    }
}
