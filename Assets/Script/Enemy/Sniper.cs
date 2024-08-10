using UnityEngine;

public class Sniper : Enemy
{
    protected override void Start()
    {
        health = 50f;
        damage = 50f;
        speed = 1.5f;
        attackPattern = new SniperAttackPattern();

        base.Start();
    }
}
