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
    private GameObject effectShineFull, effectShineDepleted, effectSmoke;

    // Swaps the effects
    public void EffectSwapper()
    {
        // Health full
        if (enemyHealthManager.currentHealth == enemyHealthManager.maxHealth)
        {
            SetActiveEffect(effectShineFull, true);
            SetActiveEffect(effectShineDepleted, false);
            SetActiveEffect(effectSmoke, false);
        }
        // Health full - 1
        else if (enemyHealthManager.currentHealth == (enemyHealthManager.maxHealth - 1))
        {
            SetActiveEffect(effectShineFull, false);
            SetActiveEffect(effectShineDepleted, true);
            SetActiveEffect(effectSmoke, false);
        }
        // Health 1 below death almost dead
        else if (enemyHealthManager.currentHealth == (enemyHealthManager.minHealth +1))
        {
            SetActiveEffect(effectShineFull, false);
            SetActiveEffect(effectShineDepleted, false);
            SetActiveEffect(effectSmoke, true);
        }
        // Health other
        else
        {
            effectShineFull.SetActive(false);
            effectShineDepleted.SetActive(false);
            effectSmoke.SetActive(false);
        }
    }
    
    // Sets active the effect if it exist in unity enemy object
    private void SetActiveEffect(GameObject effect, bool isActive)
    {
        if (effect != null)
            effect.SetActive(isActive);
    }
}
