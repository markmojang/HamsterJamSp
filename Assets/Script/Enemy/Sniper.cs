using UnityEngine;

public class Sniper : Enemy
{
    public float attackRange = 10f;
    protected override void Start()
    {
        health = 50f;
        damage = 50f;
        speed = 1.5f;

        base.Start();
    }
    private void Update()
    {
        MaintainDistanceFromPlayer();
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
