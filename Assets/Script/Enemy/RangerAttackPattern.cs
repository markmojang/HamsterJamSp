using UnityEngine;

public class RangerAttackPattern : IAttackPattern
{
    public void ExecuteAttack(Enemy enemy)
    {
        // Implement Tank's specific attack logic, e.g., shooting multiple projectiles
        Debug.Log("Ranger is attacking!");
    }
}
