using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu(fileName = "IncreaseCriticalChanceBuff", menuName = "Buffs/IncreaseCriticalChanceBuff")]
public class IncreaseCriticalChanceBuff : Buff
{
    public float criticalChanceIncrease = 0.2f;

    private void OnEnable()
    {
        buffName = "Increase Critical Chance";
    }
    public override void ApplyBuff(PlayerBuffs playerBuffs)
    {
        PlayerCombat playerCombat = playerBuffs.GetComponent<PlayerCombat>();
        if (playerCombat != null)
        {
            playerCombat.criticalChance += criticalChanceIncrease;
            Debug.Log("Critical Chance Buff Applied: Increased Critical Chance");
        }
    }
}

