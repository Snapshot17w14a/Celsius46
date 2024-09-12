using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerHealth : MonoBehaviour
{
    [SerializeField]
    private int currentHealth, maxHealth;

    // Start is called before the first frame update
    void Start()
    {
        GameEvents.onPlayerPickUpItem += HealPlayer;
    }

    private void HealPlayer(PickUpItem item)
    {
        if (item is Healthpack) {
            Healthpack healthpack = (Healthpack)item;
            currentHealth += healthpack.restoredHealthAmount;
            if (currentHealth > maxHealth)
                currentHealth = maxHealth;
        }
    }

    private void TakeDamage(int damageAmount)
    {
        currentHealth -= damageAmount;
        if (currentHealth <= 0)
            GameEvents.RaiseOnPlayerDies();
    }
}
