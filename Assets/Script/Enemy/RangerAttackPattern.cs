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
        // Instantiate and shoot the bullet
        EnemyBullet bullet = Instantiate(enemy.bulletPrefab, enemy.transform.position, Quaternion.identity).GetComponent<EnemyBullet>();
        bullet.SetDirection(enemy.transform.up);
    }
}
