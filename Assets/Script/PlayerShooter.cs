using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerShooter : MonoBehaviour
{
    public GameObject bulletPrefab;
    
    public float bulletSpeed = 10f;
    public float fireRate = 0.5f;
    private float firect = 0f;
    private Camera mainCamera;
    [SerializeField] private float bulletlifespan;
    void Start() 
    {
        mainCamera = Camera.main; 
    }
    
    void Update()
    {
        if ( firect < fireRate) 
        {
            firect += Time.deltaTime; 
        }
        else{
            Shoot();
            firect = 0f;
        }
    }

    void Shoot()
    {
        Vector3 mousePosition = mainCamera.ScreenToWorldPoint(Input.mousePosition);
        mousePosition.z = 0f;

        Vector3 direction = (mousePosition - transform.position).normalized;

        GameObject bullet = ObjectPool.Instance.GetObjectFromPool();

        bullet.transform.position = transform.position; 
        Rigidbody2D rb = bullet.GetComponent<Rigidbody2D>();
        rb.velocity = direction * bulletSpeed;

        StartCoroutine(vanishbullet(bullet));
    }

    IEnumerator vanishbullet(GameObject objects){
        yield return new WaitForSeconds(bulletlifespan);
        ObjectPool.Instance.ReturnObjectToPool(objects);
    }
}
