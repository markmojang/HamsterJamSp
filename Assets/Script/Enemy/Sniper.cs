using UnityEngine;

public class Sniper : Enemy
{
    public float attackRange = 10f; // Sniper's preferred distance from player
    public float fireCooldown = 2f; // Time between shots
    private float lastShotTime;

    protected override void Start()
    {
        health = 50f;
        damage = 50f;
        speed = 1.5f;
        attackPattern = new SniperAttackPattern();

        base.Start();
    }

    private void Update()
    {
        MaintainDistanceFromPlayer();
        if (Time.time - lastShotTime >= fireCooldown && Vector3.Distance(transform.position, player.position) <= attackRange)
        {
            Attack();
            lastShotTime = Time.time;
        }
    }

    private void MaintainDistanceFromPlayer()
    {
        float distance = Vector3.Distance(transform.position, player.position);

        if (distance < attackRange)
        {
            // Move away from the player
            transform.position = Vector3.MoveTowards(transform.position, player.position, -speed * Time.deltaTime);
        }
        else if (distance > attackRange)
        {
            // Move closer to the player
            transform.position = Vector3.MoveTowards(transform.position, player.position, speed * Time.deltaTime);
        }
    }
}
