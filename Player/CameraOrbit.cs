using UnityEngine;
using UnityStandardAssets.CrossPlatformInput;
using System.Collections;
using System;

public class CameraOrbit : MonoBehaviour
{
    [SerializeField]
    private Transform camTarget;

    public enum InputFrom
    {
        Mouse,
        Joystick
    }
    public InputFrom _input;

    public bool targeted;
    public GameObject targetPlayer;

    public float VerticalSensitivity = 1.5f;
    public float HorizontalSensitivity = 1.5f;


    private float _rotY;
    private float _rotY_clamp;
    private float _rotX;

    private Quaternion rotation;

    private Vector3 _offset;
    private Vector3 destination;

    private bool IsMoving;

    void Awake ()
    {
        _rotY = transform.eulerAngles.y;
        _rotY_clamp = 0;
        _rotX = transform.eulerAngles.x;

        _offset = camTarget.localPosition - transform.localPosition;
    }

    void LateUpdate()
    {
        InputPlayer();
                
        if (IsMoving && Vector3.Distance(transform.position, destination) < 0.05f)
            IsMoving = false;

        MoveAndRotate();

    }

    void InputPlayer()
    {
        switch (_input)
        {
            case (InputFrom.Mouse):

                _rotX -= Input.GetAxis("Mouse Y") * VerticalSensitivity / 3;

                if (targeted)
                {
                    _rotY_clamp += Input.GetAxis("Mouse X") * HorizontalSensitivity / 6;
                    _rotY = transform.eulerAngles.y;
                }
                else
                    _rotY += Input.GetAxis("Mouse X") * HorizontalSensitivity;

                if (Input.GetKeyUp(KeyCode.LeftShift))
                    IsMoving = true;

                break;
            case (InputFrom.Joystick):
#if UNITY_EDITOR
                if (Input.GetMouseButton(1))
                {
                    _rotX -= Input.GetAxis("Mouse Y") * VerticalSensitivity / 3;
                    _rotY += Input.GetAxis("Mouse X") * HorizontalSensitivity;
                }
#endif

                _rotX += CrossPlatformInputManager.GetAxis("VerticalTilt") * VerticalSensitivity;

                if (targeted)
                {
                    _rotY_clamp += CrossPlatformInputManager.GetAxis("HorizontalTilt") * HorizontalSensitivity / 6;
                    _rotY = transform.eulerAngles.y;
                }
                else
                    _rotY += CrossPlatformInputManager.GetAxis("HorizontalTilt") * HorizontalSensitivity;

                break;
        }

        if (_rotY > 180)
            _rotY -= 360;

        if (_rotY < -180)
            _rotY += 360;

        _rotX = Mathf.Clamp(_rotX, 0, 35);
        _rotY_clamp = Mathf.Clamp(_rotY_clamp, -10, 10);
    }

    public void AddRotation(float aboveY, float aboveX = 0)
    {
        _rotY += aboveY;
        _rotX += aboveX;
    }

    void MoveAndRotate()
    {
        if (!targeted)
        {
            rotation = Quaternion.Euler(_rotX, _rotY, 0);

            destination = camTarget.position - (rotation * _offset);

            if (IsMoving)
            {
                transform.rotation = Quaternion.Slerp(transform.rotation, rotation, Time.deltaTime * 6);
                transform.position = Vector3.Slerp(transform.position, destination, Time.deltaTime * 8);
            }
            else
            {
                transform.rotation = rotation;
                transform.position = destination;
            }
        }
        else
        {
           

            Vector3 rotate = targetPlayer.transform.position - transform.position;
            rotation = Quaternion.LookRotation(rotate, Vector3.up);

            rotation = Quaternion.Euler(0, rotation.eulerAngles.y, 0);

            if (transform.position != camTarget.position - (rotation * _offset))
                transform.position = Vector3.Lerp(transform.position, camTarget.position - (rotation * _offset), Time.deltaTime * 5);

            rotation = Quaternion.Euler(_rotX, rotation.eulerAngles.y + _rotY_clamp, 0);

            transform.rotation = Quaternion.Lerp(transform.rotation, rotation, Time.deltaTime * 5);
        }
    }

    void OnCollisionEnter(Collision collision)
    {
        Debug.Log("Collision");

        foreach (var point in collision.contacts)
            transform.position -= point.normal * 3;
    }
}
