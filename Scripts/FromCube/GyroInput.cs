using UnityEngine;

public class GyroInput : MonoBehaviour {

    public Vector3 startPosition;
    public float rotSpeed = 15f;
    public float sensityvity = 0.2f;

    //public Text textField;


    Quaternion startRotation;


    //PlayerController player;
    CameraRotator camRot;

    void Start()
    {
        //player = PlayerController
        camRot = GetComponent<CameraRotator>();

        startPosition = Input.acceleration;

        //textField = SceneManager.instance.ui.transform.FindChild("accel").GetComponent<Text>();
        //startRotation = Quaternion.Euler(startPosition);
    }



    void Update()
    {
        /*
        Vector3 accel = Input.acceleration - startPosition;
        
        if(accel.magnitude > sensityvity)
        {
            Vector3 direction = accel.normalized;
            //direction = direction.normalized;

            camRot.xRotation -= direction.z * rotSpeed * Time.deltaTime;
            camRot.yRotation += direction.x * rotSpeed * Time.deltaTime;
        }
		*/

        // Quaternion angle = Quaternion.Euler(Input.acceleration);
        Vector3 input = Input.acceleration;
        //textField.text = input.ToString();

        input -= startPosition;
       // textField.text += ", to " + input + ", angle " + Vector3.Angle(input, startPosition);



        if (input.y > sensityvity)
        {
            camRot.Rotate(rotSpeed,0);
            //camRot.xRotation += rotSpeed * Time.deltaTime;
        }
        else if (input.y < -sensityvity)
        {
            camRot.Rotate(-rotSpeed, 0);
        }

        if (input.x > sensityvity)
        {
            camRot.Rotate(0, -rotSpeed);
        }
        else if (input.x < -sensityvity)
        {
            camRot.Rotate(0, rotSpeed);
        }

    }
}
