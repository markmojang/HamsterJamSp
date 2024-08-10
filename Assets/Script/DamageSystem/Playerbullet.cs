using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Playerbullet : MonoBehaviour
{
    private float damage;
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
            ObjectPool.Instance.ReturnObjectToPool(gameObject);
        }
    }
}
