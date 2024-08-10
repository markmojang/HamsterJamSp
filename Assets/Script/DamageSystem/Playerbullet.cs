using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Playerbullet : MonoBehaviour
{
    [SerializeField] private float damage;
    void OnTriggerEnter2D(Collider2D col)
    {
        Enemy enemy = col.GetComponent<Enemy>(); // หา component Enemy จากวัตถุที่ชน
        if (enemy != null) 
        {
            // ศัตรูได้รับความเสียหาย
            enemy.TakeDamage(damage); // สมมติว่าคุณมีตัวแปร damage ในสคริปต์นี้เพื่อเก็บค่าความเสียหายที่ต้องการทำ
            Destroy(gameObject);
            
        }
    }
}
