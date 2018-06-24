using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class Billboard : MonoBehaviour
{
    Transform mainCamTransform;
    Quaternion rotation;

    void Start()
    {
        mainCamTransform = Camera.main.transform;
    }
    
    void FixedUpdate()
    {
        if (!mainCamTransform)
            return;

        rotation = Quaternion.LookRotation(mainCamTransform.position - transform.position, Vector3.up);
        //rotation = Quaternion.Euler(rotation.eulerAngles.x, rotation.eulerAngles.y, 0);
        rotation.eulerAngles.Set(rotation.eulerAngles.x, rotation.eulerAngles.y, 0);
        transform.rotation = Quaternion.Lerp(transform.rotation, rotation, Time.fixedDeltaTime * 5);
    }
    
}
