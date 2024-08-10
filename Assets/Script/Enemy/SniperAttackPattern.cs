using UnityEngine;

public class SniperAttackPattern : IAttackPattern
{
    public void ExecuteAttack(Enemy enemy)
    {
        // Implement Tank's specific attack logic, e.g., shooting multiple projectiles
        Debug.Log("Sniper is attacking!");
    }
}
