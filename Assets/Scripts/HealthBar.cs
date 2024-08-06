using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class HealthBar : MonoBehaviour
{
    public Image healthBarFill;  // Reference to the health bar Image

    public void SetMaxHealth(float maxHealth)
    {
        healthBarFill.fillAmount = 1f;  // Start with a full health bar
    }

    public void SetHealth(float currentHealth, float maxHealth)
{
    Debug.Log("Updating Health Bar: " + currentHealth + "/" + maxHealth);
    healthBarFill.fillAmount = Mathf.Clamp01(currentHealth / maxHealth);  // Update the fill amount
}
}
