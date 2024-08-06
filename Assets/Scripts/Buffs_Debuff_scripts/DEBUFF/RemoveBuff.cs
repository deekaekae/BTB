using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu(fileName = "RemoveBuff", menuName = "Debuffs/RemoveBuff")]

public class RemoveBuff : Debuff
{
    public override void ApplyDebuff(PlayerBuffs playerBuffs)
    {
        playerBuffs.RemoveRandomBuff();
        Debug.Log("Buff Removed Debuff Applied");
    }
}
