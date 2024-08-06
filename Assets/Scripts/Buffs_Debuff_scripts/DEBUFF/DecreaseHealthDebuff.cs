using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu(fileName = "DecreaseHealthDebuff", menuName = "Debuffs/DecreaseHealthDebuff")]
public class DecreaseHealthDebuff : Debuff
{
    public float healthDecreaseAmount = 20f;

    public override void ApplyDebuff(PlayerBuffs playerBuffs)
    {
        PlayerHealth playerHealth = playerBuffs.GetComponent<PlayerHealth>();
        if (playerHealth != null)
        {
            playerHealth.maxHealth -= healthDecreaseAmount;
            playerHealth.currentHealth -= healthDecreaseAmount;
            Debug.Log("Health Debuff Applied: Decreased Health");
        }
    }
}