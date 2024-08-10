using UnityEngine;

public class SniperAttackPattern : IAttackPattern
{
    
    public void ExecuteAttack(Enemy enemy)
    {
        // Instantiate and shoot a high-speed, high-damage bullet
        GameObject bulletObject = ObjectFactory.InstantiatePrefab(enemy.bulletPrefab, enemy.transform.position, Quaternion.identity);
        EnemyBullet bullet = bulletObject.GetComponent<EnemyBullet>();
        bullet.SetDirection(enemy.transform.up);
        bullet.speed *= 2f; // Increase bullet speed
        bullet.damage *= 2f; // Increase bullet damage
    }
}
