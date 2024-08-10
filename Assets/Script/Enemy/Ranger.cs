using UnityEngine;

public class Ranger : Enemy
{
    public float minDistance = 3f;  // Minimum distance from player
    public float maxDistance = 6f;  // Maximum distance from player
    public float attackRange = 10f;
    protected override void Start()
    {
        health = 100f;
        damage = 20f;
        speed = 2f;

        base.Start();
    }

    private void Update()
    {
        MaintainDistanceFromPlayer();
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

