using UnityEngine;

public class Ranger : Enemy
{
    protected override void Start()
    {
        health = 100f;
        damage = 20f;
        speed = 2f;
        attackPattern = new RangerAttackPattern();

        base.Start();
    }
}
