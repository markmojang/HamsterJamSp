using UnityEngine;

public class Ranger : Enemy
{
    public float rotationSpeed = 5f;
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
        lookatplayer();
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
    private void lookatplayer(){
        Vector2 direction = new Vector2(
            player.position.x - transform.position.x,
            player.position.y - transform.position.y
        );

        float targetAngle = Mathf.Atan2(direction.y, direction.x) * Mathf.Rad2Deg - 90;

        float currentAngle = transform.rotation.eulerAngles.z;
        
        float angle = Mathf.LerpAngle(currentAngle, targetAngle, rotationSpeed * Time.deltaTime);

        transform.rotation = Quaternion.Euler(new Vector3(0, 0, angle));
    }
}

