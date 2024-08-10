using UnityEngine;
using System.Collections.Generic;

public class ObjectPool : MonoBehaviour
{
    public static ObjectPool Instance;   
 // Singleton instance for easy access

    public GameObject objectPrefab; 

    private Queue<GameObject> objectQueue = new Queue<GameObject>();
    [SerializeField]private int initialPoolSize = 10; // Initial number of objects in the pool

    void Awake()
    {
        // Singleton pattern setup
        if (Instance == null)
        {
            Instance = this;
        }
        else
        {
            Destroy(gameObject);
        }
    }

    void Start()
    {
        // Pre-instantiate objects and add them to the pool
        for (int i = 0; i < initialPoolSize; i++)
        {
            GameObject obj = Instantiate(objectPrefab);
            obj.SetActive(false);
            objectQueue.Enqueue(obj);
        }
    }

    public GameObject GetObjectFromPool()
    {
        if (objectQueue.Count > 0)
        {
            GameObject obj = objectQueue.Dequeue();
            obj.SetActive(true);
            return obj;
        }
        else   

        {
            // If the pool is empty, create a new object 
            GameObject obj = Instantiate(objectPrefab);
            return obj;
        }
    }

    public void ReturnObjectToPool(GameObject obj)
    {
        obj.SetActive(false);
        objectQueue.Enqueue(obj);
    }
}