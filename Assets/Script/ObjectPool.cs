using System.Collections.Generic;
using UnityEngine;

public class ObjectPool : MonoBehaviour
{
    public static ObjectPool Instance;

    private Dictionary<string, Queue<GameObject>> pools = new Dictionary<string, Queue<GameObject>>();
    private Dictionary<string, GameObject> prefabs = new Dictionary<string, GameObject>(); // Store prefabs for each pool
    private Dictionary<string, int> poolSizes = new Dictionary<string, int>(); // Track pool sizes

    private const int poolIncreaseAmount = 10; // Amount to increase pool size by

    void Awake()
    {
        Instance = this;
    }

    // Create or initialize a pool with a specified size
    public void CreatePool(string poolKey, GameObject prefab, int initialSize)
    {
        if (!pools.ContainsKey(poolKey))
        {
            Queue<GameObject> newPool = new Queue<GameObject>();

            for (int i = 0; i < initialSize; i++)
            {
                GameObject obj = Instantiate(prefab);
                obj.SetActive(false);
                newPool.Enqueue(obj);
            }

            pools.Add(poolKey, newPool);
            prefabs.Add(poolKey, prefab);
            poolSizes.Add(poolKey, initialSize);
        }
    }

    // Retrieve an object from the pool; create a new one if necessary
    public GameObject GetObjectFromPool(string poolKey)
    {
        if (pools.ContainsKey(poolKey))
        {
            if (pools[poolKey].Count > 0)
            {
                GameObject obj = pools[poolKey].Dequeue();
                obj.SetActive(true);
                return obj;
            }
            else
            {
                // Optional: Increase the pool size if needed
                IncreasePoolSize(poolKey, poolIncreaseAmount);
                return GetObjectFromPool(poolKey); // Try again after increasing the pool size
            }
        }
        return null;
    }

    // Return an object to the pool
    public void ReturnObjectToPool(string poolKey, GameObject obj)
    {
        if (pools.ContainsKey(poolKey))
        {
            obj.SetActive(false);
            pools[poolKey].Enqueue(obj);
        }

        // Proactive pool size increase if needed
        if (pools[poolKey].Count < poolIncreaseAmount)
        {
            IncreasePoolSize(poolKey, poolIncreaseAmount);
        }
    }

    // Increase the pool size by creating additional objects
    private void IncreasePoolSize(string poolKey, int increaseAmount)
    {
        if (pools.ContainsKey(poolKey) && prefabs.ContainsKey(poolKey))
        {
            for (int i = 0; i < increaseAmount; i++)
            {
                GameObject obj = Instantiate(prefabs[poolKey]);
                obj.SetActive(false);
                pools[poolKey].Enqueue(obj);
            }

            poolSizes[poolKey] += increaseAmount;
        }
    }
}
