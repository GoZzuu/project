using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class IPooledObject : MonoBehaviour {

    Transform parent;
    public float deactivationTime = 0;

    public virtual void CreateObject(float deactivationTime)
    {
        parent = transform.parent;
        gameObject.SetActive(false);

        this.deactivationTime = deactivationTime;
    }

    public virtual void PoolObject()
    {
        gameObject.SetActive(true);
        transform.parent = null;

        if (deactivationTime != 0)
            Invoke("DeactivateObject", deactivationTime);

    }
    public virtual void DeactivateObject()
    {
       
        transform.SetParent(parent);
        transform.localPosition = Vector3.zero;
        transform.localRotation = parent.localRotation;
        gameObject.SetActive(false);
    }
}
