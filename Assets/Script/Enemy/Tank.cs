using UnityEngine;

public class Tank : Enemy
{
    protected override void Start()
    {
        health = 200f;
        damage = 10f;
        speed = 1f;
        attackPattern = new TankAttackPattern();

        base.Start();
    }
}
