using UnityEngine;

public class RangerAttackPattern : IAttackPattern
{
    private float fireRate;

    public RangerAttackPattern(float fireRate)
    {
        this.fireRate = fireRate;
    }

    public void ExecuteAttack(Enemy enemy)
    {
        // Use the ObjectFactory to instantiate the bullet
        GameObject bulletObject = ObjectFactory.InstantiatePrefab(enemy.bulletPrefab, enemy.transform.position, Quaternion.identity);
        EnemyBullet bullet = bulletObject.GetComponent<EnemyBullet>();
        bullet.SetDirection(enemy.transform.up);
    }
}
