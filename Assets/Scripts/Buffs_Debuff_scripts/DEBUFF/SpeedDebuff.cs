using UnityEngine;

[CreateAssetMenu(fileName = "SpeedDebuff", menuName = "Debuffs/SpeedDebuff")]
public class SpeedDebuff : Debuff
{
    public float speedMultiplier = 0.5f;

    public override void ApplyDebuff(PlayerBuffs playerBuffs)
    {
        PlayerMovement playerMovement = playerBuffs.GetComponent<PlayerMovement>();
        if (playerMovement != null)
        {
            playerMovement.moveSpeed *= speedMultiplier;
            Debug.Log("Speed Debuff Applied: Decreased Speed");
        }
    }
}