using UnityEngine;

public class TankAttackPattern : IAttackPattern
{
    public void ExecuteAttack(Enemy enemy)
    {
        // Implement Tank's specific attack logic, e.g., shooting multiple projectiles
        Debug.Log("Tank is attacking!");
    }
}
