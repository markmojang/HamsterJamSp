using UnityEngine;

public class Tank : Enemy
{
    public float rotationSpeed = 5f;
    public float minDistance = 1f; 
    public float attackRange = 2f;
    protected override void Start()
    {
        health = 200f;
        damage = 10f;
        speed = 1.0f;

        base.Start();
    }
    private void Update()
    {
        MoveTowardsPlayer();
        lookatplayer();
    }

    private void MoveTowardsPlayer()
    {
        float distance = Vector3.Distance(transform.position, player.position);
        if (distance < minDistance)
        {
            // Move away from the player
            transform.position = Vector3.MoveTowards(transform.position, player.position, -speed * Time.deltaTime);
        }
        else{
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
