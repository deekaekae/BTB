using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu(fileName = "RemoveDebuff", menuName = "Buffs/RemoveDebuff")]

public class RemoveDebuff : Buff
{
    private void OnEnable()
    {
        buffName = "Removed 1 debuff";
    }
    public override void ApplyBuff(PlayerBuffs playerBuffs)
    {
        playerBuffs.RemoveRandomDebuff();
        Debug.Log("Debuff Removed Buff Applied");
    }
}
