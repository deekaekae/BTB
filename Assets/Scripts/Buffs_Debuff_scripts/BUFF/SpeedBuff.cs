using UnityEngine;

[CreateAssetMenu(fileName = "SpeedBuff", menuName = "Buffs/SpeedBuff")]
public class SpeedBuff : Buff
{
    public float speedMultiplier = 1.5f;
    private void OnEnable()
    {
        buffName = "Increased Speed";
    }

    public override void ApplyBuff(PlayerBuffs playerBuffs)
    {
        PlayerMovement playerMovement = playerBuffs.GetComponent<PlayerMovement>();
        if (playerMovement != null)
        {
            playerMovement.moveSpeed *= speedMultiplier;
            Debug.Log("Speed Buff Applied: Increased Speed");
        }
    }
}

