using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu(fileName = "DecreaseCriticalChanceDebuff", menuName = "Debuffs/DecreaseCriticalChanceDebuff")]
public class DecreaseCriticalChanceDebuff : Debuff
{
    public float criticalChanceDecrease = 0.2f;

    public override void ApplyDebuff(PlayerBuffs playerBuffs)
    {
        PlayerCombat playerCombat = playerBuffs.GetComponent<PlayerCombat>();
        if (playerCombat != null)
        {
            playerCombat.criticalChance -= criticalChanceDecrease;
            Debug.Log("Critical Chance Debuff Applied: Decreased Critical Chance");
        }
    }
}
