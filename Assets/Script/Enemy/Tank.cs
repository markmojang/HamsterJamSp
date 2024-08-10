using UnityEngine;

public class Tank : Enemy
{
    public float attackRange = 2f;  // Range at which Tank will start shooting
    public int pellets = 5;         // Number of bullets per shotgun blast
    public float spreadAngle = 30f; // Angle spread of the shotgun

    protected override void Start()
    {
        health = 200f;
        damage = 10f;
        speed = 1f;
        attackPattern = new TankAttackPattern(pellets, spreadAngle);

        base.Start();
    }

    private void Update()
    {
        MoveTowardsPlayer();
        if (Vector3.Distance(transform.position, player.position) <= attackRange)
        {
            Attack();
        }
    }

    private void MoveTowardsPlayer()
    {
        transform.position = Vector3.MoveTowards(transform.position, player.position, speed * Time.deltaTime);
    }
}
