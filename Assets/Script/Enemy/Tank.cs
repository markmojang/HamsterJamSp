using UnityEngine;

public class Tank : Enemy
{
    public float minDistance = 1f;
    public float attackRange = 2f;
    protected override void Start()
    {
        health = 200f;
        damage = 10f;
        speed = 1f;

        base.Start();
    }
    private void Update()
    {
        MoveTowardsPlayer();
    }

    private void MoveTowardsPlayer()
    {
        if (distance < minDistance)
        {
            // Move away from the player
            transform.position = Vector3.MoveTowards(transform.position, player.position, -speed * Time.deltaTime);
        }
        else{
            transform.position = Vector3.MoveTowards(transform.position, player.position, speed * Time.deltaTime);
        }
    }

}
