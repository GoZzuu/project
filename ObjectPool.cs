using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ObjectPool : MonoBehaviour {

    public GameObject poolingObject;

    public int poolCount = 3;
    public int currentNo = 0;
    public float deactivationTime = 2;

    protected IPooledObject[] objectsPool;
    protected WaitForSeconds deactivateTime;

    public readonly static Vector3 arrowSpawnerOffset = new Vector3(0.1f, 1.4f, 0.75f);

	void Start () {
        objectsPool = new IPooledObject[poolCount];

        for (int i = 0; i < poolCount; i++)
        {
            objectsPool[i] = Instantiate(poolingObject, transform).GetComponent<IPooledObject>();
            objectsPool[i].CreateObject(deactivationTime);
        }
	}
	
	public virtual void PoolObject()
    {
        objectsPool[currentNo].PoolObject();

        currentNo++;
        if (currentNo >= poolCount)
            currentNo = 0;
        //StartCoroutine(ActivatePool());
    }
    
    public static ObjectPool CreatePool(Transform parent, GameObject poolingObject)
    {
        var pool = Instantiate(new GameObject(), arrowSpawnerOffset, Quaternion.identity, parent).AddComponent<ObjectPool>();
        pool.poolingObject = poolingObject;
        return pool;
    }
    
    /*
    IEnumerator ActivatePool()
    {
       

        objectsPool[currentNo].SetActive(true);
        yield return deactivateTime;
        objectsPool[currentNo].SetActive(false);
        yield return null;
    }*/
}
