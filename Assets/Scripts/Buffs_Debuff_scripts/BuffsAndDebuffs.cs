using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public abstract class Buff : ScriptableObject
{
    public string buffName;
    public abstract void ApplyBuff(PlayerBuffs playerBuffs);
}

public abstract class Debuff : ScriptableObject
{
    public string debuffName;
    public abstract void ApplyDebuff(PlayerBuffs playerBuffs);
}
