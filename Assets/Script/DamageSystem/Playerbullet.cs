using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Playerbullet : MonoBehaviour
{
    private float damage;
    public GameObject hitEffect;
    void Start(){
        damage = GameObject.FindWithTag("Player").GetComponent<PlayerController>().Damage;
    }
    void OnTriggerEnter2D(Collider2D col)
    {
        Enemy enemy = col.GetComponent<Enemy>(); // หา component Enemy จากวัตถุที่ชน
        if (enemy != null) 
        {
            // ศัตรูได้รับความเสียหาย
            enemy.TakeDamage(damage); // สมมติว่าคุณมีตัวแปร damage ในสคริปต์นี้เพื่อเก็บค่าความเสียหายที่ต้องการทำ
            // Instantiate the particle effect at the player's position
            if (hitEffect != null)
            {
                GameObject effect = Instantiate(hitEffect, col.transform.position, Quaternion.identity);
                Destroy(effect, 1f); // Adjust the duration as needed
            }
            ObjectPool.Instance.ReturnObjectToPool(gameObject);
        }
    }
}
