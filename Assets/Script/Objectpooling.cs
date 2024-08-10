using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ObjectPool : MonoBehaviour
{
    public static List<ObjectPool> instance;
	public List<GameObject> pooledObjects;
	public GameObject objectToPool;
	public bool canExpand = true;
	public int countToPool;
	[SerializeField]
	private string tag;
	
	void Awake() {
		if(instance == null){
			instance = new List<ObjectPool>();
		}
		tag = objectToPool.tag;
		bool tagExists =  instance.Exists((ob) => {
				if(ob.objectToPool.tag == tag){
					return true;
				}
				return false;
		});
		if(tagExists){
			Debug.Log("Object pool for " + tag + " already exists!");
			Destroy(this);
			return;
		}
		pooledObjects = new List<GameObject>();
		instance.Add(this);
		for(int i = 0; i < countToPool; i++){
			GameObject obj = Instantiate(objectToPool);
			obj.SetActive(false);
			pooledObjects.Add(obj);
		}
	}
	public static GameObject GetPooledObject(string tagToFind){
		bool tagExists =  instance.Exists((ob) => {
				if(ob.objectToPool.tag == tagToFind){
					return true;
				}
				return false;
		});
		Debug.Log("Trying to shoot " + tagToFind + " but " + tagExists); 
		if(tagExists){
			ObjectPool pool =  instance.Find((list) => {
				if(list.tag == tagToFind){
					return true;
				}
				return false;
			});
			for(int i = 0; i < pool.pooledObjects.Count; i++){
				if(!pool.pooledObjects[i].activeInHierarchy){
					return pool.pooledObjects[i];
				}
			}
			if(pool.canExpand){
				GameObject obj = Instantiate(pool.objectToPool);
				obj.SetActive(false);
				pool.pooledObjects.Add(obj);
				return obj;
			}
		}
		return null;
	}
}
