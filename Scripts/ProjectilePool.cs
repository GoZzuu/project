using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ProjectilePool : ObjectPool {

    Bullet[] objectsPool;
    WeaponBase projectileWeapon;

    public static ProjectilePool CreatePool(Transform parent, GameObject poolingObject, WeaponBase weapon)
    {
        //var pool = Instantiate(new GameObject("Helper_projectilePool"), parent).AddComponent<ProjectilePool>();
        var pool = new GameObject("ProjectilePool").AddComponent<ProjectilePool>();
        pool.transform.SetParent(parent);

        pool.transform.localPosition = arrowSpawnerOffset;
        pool.transform.localRotation = Quaternion.identity;
       // var pool = new GameObject("ProjectilePool", typeof(ProjectilePool)).GetComponent<ProjectilePool>();
       //pool.transform.SetParent(parent);
       // pool.transform.position = arrowSpawnerOffset;

        pool.poolingObject = poolingObject;
        pool.projectileWeapon = weapon;
        return pool;
    }

    void Start()
    {
        objectsPool = new Bullet[poolCount];

        for (int i = 0; i < poolCount; i++)
        {
            objectsPool[i] = Instantiate(poolingObject, transform).GetComponent<Bullet>();
            objectsPool[i].weapon = projectileWeapon;
            objectsPool[i].CreateObject(deactivationTime);
        }
    }

    public override void PoolObject()
    {
        objectsPool[currentNo].PoolObject();

        currentNo++;
        if (currentNo >= poolCount)
            currentNo = 0;
        //StartCoroutine(ActivatePool());
    }
}
