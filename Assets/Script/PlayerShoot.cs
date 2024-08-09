using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerShoot : MonoBehaviour
{
    public GameObject bulletPrefab;
    public float bulletSpeed = 10f;
    public float fireRate = 0.5f;
    private float nextFireTime = 0f; 
    private Camera mainCamera;

    void Start() 
    {
    mainCamera = Camera.main; 
    }
    
    void Update()
    {
        if (Time.time > nextFireTime) 
        {
            Shoot();
            nextFireTime = Time.time + fireRate; 
        }
    }

    void Shoot()
    {
        // หาตำแหน่งของเมาส์ในโลก 3 มิติ
        Vector3 mousePosition = mainCamera.ScreenToWorldPoint(Input.mousePosition);
        mousePosition.z = 0f; // ปรับให้ z เป็น 0 เพื่อให้กระสุนอยู่ในระนาบเดียวกับตัวละคร

        // หาเวกเตอร์ทิศทางจากตัวละครไปยังเมาส์
        Vector3 direction = (mousePosition - transform.position).normalized;

        // สร้างกระสุน
        GameObject bullet = Instantiate(bulletPrefab, transform.position, Quaternion.identity);
        Destroy(bullet, 3f); 

        // กำหนดทิศทางและความเร็วให้กับกระสุน
        Rigidbody2D rb = bullet.GetComponent<Rigidbody2D>();
        rb.velocity = direction * bulletSpeed;
    }
}
