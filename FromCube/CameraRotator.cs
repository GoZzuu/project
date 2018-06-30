using UnityEngine;
using System.Collections;

public class CameraRotator : MonoBehaviour
{
    public Transform pivot;

    float speed = 2f;
    float maxSpeed = 1.5f;


    public float xRotation;
    public float yRotation;

    float maxX = 80;
    float minX = 10;

    public Vector2 speedvector = Vector2.zero;
    public bool isRotating = false;


    void Start ()
    {
        //pivot = transform.Find("CamRotation");

        xRotation = pivot.localEulerAngles.x;
        yRotation = pivot.localEulerAngles.y;

        
	}
	
    public void Rotate(float deltaX, float deltaY)
    {
        isRotating = true;
        speedvector.x += deltaX/2;
        speedvector.y += deltaY;

        if (speedvector.magnitude >= maxSpeed)
        {
            speedvector = speedvector.normalized * maxSpeed;
        }

    }

    /*
	void Update ()
    {
        if (yRotation != pivot.localEulerAngles.y || xRotation != pivot.localEulerAngles.x)
        {
            yRotation = ClampAngles(yRotation);
            xRotation = Mathf.Clamp(xRotation, minX, maxX);

            pivot.localRotation = Quaternion.Lerp(pivot.localRotation, Quaternion.Euler(xRotation, yRotation, 0), Time.deltaTime * speed); 
        }
	}
    */
    private void Update()
    {
        if(speedvector.sqrMagnitude != 0)
        {
            yRotation += speedvector.y;
            xRotation += speedvector.x;


            yRotation = ClampAngles(yRotation);
            xRotation = Mathf.Clamp(xRotation, minX, maxX);

            pivot.localRotation = Quaternion.Lerp(pivot.localRotation, Quaternion.Euler(xRotation, yRotation, 0), Time.deltaTime * speed);
        }

        if (!isRotating)
        {
            
           speedvector -= speedvector * 0.2f;
            
        }



        isRotating = false;
    } 
    
    float ClampAngles(float angle)
    {
        if (angle > 360)
            angle -= 360;
        if (angle < 0)
            angle += 360;

        return angle;
    }
}
