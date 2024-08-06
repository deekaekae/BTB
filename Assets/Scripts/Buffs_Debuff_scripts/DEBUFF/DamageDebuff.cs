using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu(fileName = "DamageDebuff", menuName = "Debuffs/DamageDebuff")]

public class DamageDebuff : Debuff
{
    public float damageReductionAmount = 5f;

    public override void ApplyDebuff(PlayerBuffs playerBuffs)
    {
        PlayerCombat playerCombat = playerBuffs.GetComponent<PlayerCombat>();
        if (playerCombat != null)
        {
            playerCombat.baseDamage -= damageReductionAmount;
            Debug.Log("Damage Debuff Applied: Reduced Damage");
        }
    }
}
