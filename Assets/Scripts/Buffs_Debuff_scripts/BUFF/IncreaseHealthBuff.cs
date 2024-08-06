using UnityEngine;

[CreateAssetMenu(fileName = "IncreaseHealthBuff", menuName = "Buffs/IncreaseHealthBuff")]
public class IncreaseHealthBuff : Buff
{
    public float healthIncreaseAmount = 20f;

    private void OnEnable()
    {
        buffName = "Increased Health";
    }

    public override void ApplyBuff(PlayerBuffs playerBuffs)
    {
        PlayerHealth playerHealth = playerBuffs.GetComponent<PlayerHealth>();
        if (playerHealth != null)
        {
            playerHealth.IncreaseMaxHealth(healthIncreaseAmount);
            Debug.Log("Health Buff Applied: Increased Health");
        }
    }
}
