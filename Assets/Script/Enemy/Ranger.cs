using UnityEngine;

public class Ranger : Enemy
{
    public float minDistance = 3f;  // Minimum distance from player
    public float maxDistance = 6f;  // Maximum distance from player
    public float fireRate = 0.2f;   // Time between shots

    private float lastShotTime;

    protected override void Start()
    {
        health = 100f;
        damage = 20f;
        speed = 2f;
        attackPattern = new RangerAttackPattern(fireRate);

        base.Start();
    }

    private void Update()
    {
        MaintainDistanceFromPlayer();
        if (Time.time - lastShotTime >= fireRate)
        {
            Attack();
            lastShotTime = Time.time;
        }
    }

    private void MaintainDistanceFromPlayer()
    {
        float distance = Vector3.Distance(transform.position, player.position);

        if (distance < minDistance)
        {
            // Move away from the player
            transform.position = Vector3.MoveTowards(transform.position, player.position, -speed * Time.deltaTime);
        }
        else if (distance > maxDistance)
        {
            // Move closer to the player
            transform.position = Vector3.MoveTowards(transform.position, player.position, speed * Time.deltaTime);
        }
    }
}
