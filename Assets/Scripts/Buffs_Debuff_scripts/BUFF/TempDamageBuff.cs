using System.Collections;
using System.Collections.Generic;
using UnityEngine;
[CreateAssetMenu(fileName = "TempDamageBuff", menuName = "Buffs/ TempDamageBuff")]

public class TempDamageBuff : Buff
{
    public float damageBoostAmount = 20f;
    public float boostDuration = 30f;

    private void OnEnable()
    {
        buffName = " Temporary Damage Boost";
    }
    public override void ApplyBuff(PlayerBuffs playerBuffs)
    {
        PlayerCombat playerCombat = playerBuffs.GetComponent<PlayerCombat>();
        if (playerCombat != null)
        {
            playerCombat.AddTemporaryDamageBoost(damageBoostAmount, boostDuration);
            Debug.Log("Damage Boost Per Kill Buff Applied");
        }
    }
}
