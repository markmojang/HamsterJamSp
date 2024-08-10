using UnityEngine;

public class ObjectFactory : MonoBehaviour
{
    public static GameObject InstantiatePrefab(GameObject prefab, Vector3 position, Quaternion rotation)
    {
        return Instantiate(prefab, position, rotation);
    }
}
