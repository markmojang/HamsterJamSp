using UnityEngine;

public class TankAttackPattern : IAttackPattern
{
    private int pellets;
    private float spreadAngle;

    public TankAttackPattern(int pellets, float spreadAngle)
    {
        this.pellets = pellets;
        this.spreadAngle = spreadAngle;
    }

    public void ExecuteAttack(Enemy enemy)
    {
        for (int i = 0; i < pellets; i++)
        {
            float angle = i * (spreadAngle / (pellets - 1)) - (spreadAngle / 2);
            Quaternion rotation = Quaternion.Euler(0, 0, angle);
            Vector3 direction = rotation * enemy.transform.up;

            // Instantiate and shoot the bullet
            EnemyBullet bullet = Instantiate(enemy.bulletPrefab, enemy.transform.position, Quaternion.identity).GetComponent<EnemyBullet>();
            bullet.SetDirection(direction);
        }
    }
}
