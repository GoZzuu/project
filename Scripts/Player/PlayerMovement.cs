using UnityEngine;
using System.Collections;
using UnityStandardAssets.CrossPlatformInput;

[SelectionBase]
[RequireComponent(typeof(CharacterController))]
public class PlayerMovement : MonoBehaviour
{
    Animator animator;
    Transform mainCamera;
    CameraOrbit cam;
    CharacterController _charController;

    public float horInput;
    public float vertInput;

    [Space]
    public float rotSpeed = 15f;
    public float moveSpeed = 6f;

    bool IsMoving;
    public bool TargetedWalk = false;

    Vector3 movement = Vector3.zero;
    Quaternion direction;

    public PlayerWeaponAndAttacks weapon;
    public PlayerSearchingTarget targeting;

    void Start()
    {       
        targeting = GetComponent<PlayerSearchingTarget>();
        _charController = GetComponent<CharacterController>();    
        mainCamera = Camera.main.transform;
        cam = mainCamera.GetComponent<CameraOrbit>();
        animator = GetComponent<Animator>();
        weapon = GetComponent<PlayerWeaponAndAttacks>();

    } 

  
    private void FixedUpdate()
    {
        PlayerInput();
        AnimationsInput(horInput, vertInput);
        Moving();

        if ((weapon.IsAttacking /*|| TargetedWalk*/) && targeting.Targeted || targeting.Targeted && weapon.CurrentWeaponRanged() && weapon.IsAttacking)
        {
            Vector3 temp = targeting.Target.position - transform.position;

            direction = Quaternion.LookRotation(temp);
            direction = Quaternion.Euler(0, direction.eulerAngles.y, 0);

            transform.rotation = Quaternion.Lerp(transform.rotation, direction, rotSpeed * Time.fixedDeltaTime);
        }

       // SearchingForTargets();

       
    }

    void PlayerInput()
    {
        //движение
        horInput = Input.GetAxis("Horizontal") + CrossPlatformInputManager.GetAxis("Horizontal");
        vertInput = Input.GetAxis("Vertical") + CrossPlatformInputManager.GetAxis("Vertical");     

        if (Input.GetButtonDown("Jump") || CrossPlatformInputManager.GetButtonDown("Jump"))
            animator.SetTrigger("jump");


        //ПОДНЯТЬ ЩИТ
        /* 
        //           POWER ATTACK X3!
        if (Input.GetButtonDown("Fire2"))
            AttackPower();
        if (Input.GetButton("Blocking"))
        {
           
                if (!IsBlocking && !IsAttacking && currentEndurance > 5)
                {
                    IsBlocking = true;
                    _animator.SetTrigger("switch_block");
                }
                else if (IsAttacking || currentEndurance < 5)
                {
                    IsBlocking = false;
                    _animator.SetTrigger("switch_block");
                }
            

        }
        else
        {
            if (IsBlocking)
            {
                IsBlocking = false;
                _animator.SetTrigger("switch_block");
            }
        }
        */
        //СКИЛЫ
    }
    void Moving()
    {
        movement = Vector3.zero;
        if (horInput != 0 || vertInput != 0)
        {
            IsMoving = true;

            movement.x = horInput;
            movement.z = vertInput;

            if (!TargetedWalk)
            {

                Quaternion tmp = mainCamera.rotation;
                mainCamera.eulerAngles = new Vector3(0, mainCamera.eulerAngles.y, 0);
                movement = mainCamera.TransformDirection(movement);
                //mainCamera.rotation = tmp;
                direction = Quaternion.LookRotation(movement);

                movement += transform.forward;
                movement = movement.normalized * moveSpeed;


                float camYrotate = Vector3.SignedAngle(movement, mainCamera.forward, Vector3.up);
                if (InRange(camYrotate, 170))
                    cam.AddRotation(-camYrotate * 0.008f);

                if (!weapon.IsAttacking)
                {
                    transform.rotation = Quaternion.Lerp(transform.rotation, direction, rotSpeed * 2 * Time.deltaTime);
                    movement *= Time.deltaTime;

                }
                else
                {
                    float coef = 0.1f;

                    transform.rotation = Quaternion.Lerp(transform.rotation, direction, rotSpeed * 0.5f * Time.deltaTime);
                    movement *= Time.deltaTime * coef;
                }
            }
            else
            {
                Quaternion tmp = mainCamera.rotation;
                mainCamera.eulerAngles = new Vector3(0, mainCamera.eulerAngles.y, 0);
                movement = mainCamera.TransformDirection(movement);
         
                direction = Quaternion.Euler(0, mainCamera.eulerAngles.y, 0);
                movement = movement.normalized * moveSpeed;

                if (!targeting.Targeted)
                {
                    transform.rotation = Quaternion.Lerp(transform.rotation, direction, rotSpeed * Time.deltaTime);
                    cam.AddRotation(horInput * 0.5f);
                }
                movement *= Time.deltaTime * 0.3f;

            }
           
            movement.y -= 0.5f;

        }
        else
            IsMoving = false;

        if (movement != Vector3.zero)
            _charController.Move(movement);

        
    } 

    /*
            else
            {
                movement.x = horInput * moveSpeed;
                movement.z = vertInput * moveSpeed;

                movement = Vector3.ClampMagnitude(movement, moveSpeed / 2);

                //

                Quaternion tmp = transform.rotation;

                tmp.eulerAngles = new Vector3(0, tmp.eulerAngles.y, 0);
                movement = transform.TransformDirection(movement);

                //transform.rotation = tmp;


                if (!IsAttacking)
                {
                    movement *= Time.deltaTime;
                }
                else
                {
                    float coef = 0.1f;
                    movement *= Time.deltaTime * coef;
                }


            }
            */ 

    void AnimationsInput(float h, float v)
    {

        float lenght = 0;

        if (h != 0 || v != 0)
        {
            Vector2 direction = new Vector2(h, v);

            lenght = direction.magnitude;

            direction = direction.normalized;

            animator.SetFloat("targetDirX", direction.x);
            animator.SetFloat("targetDirY", direction.y);
        }

        

        animator.SetBool("isAttacking", weapon.IsAttacking);
        animator.SetBool("IsMoving", IsMoving);
        animator.SetBool("Targeted", TargetedWalk);
    }


    //cam.targeted = GetTarget;
    //cam.targetPlayer = Target.gameObject;

    public bool MoveTo(Vector3 point)
    {
        Vector3 vec = point - transform.position;
        transform.Translate((vec) * 0.3f);
        transform.rotation = Quaternion.LookRotation(vec);
        return (transform.position - point).magnitude == 0;
    }




    bool InRange(float value, float maxRange, float minRange)
    {
        return value < maxRange && value > minRange;
    }
    bool InRange(float value, float maxRange)
    {
        return value < maxRange && value > -maxRange;
    } 


    /*private IEnumerator StartColliderAfterTime(int attackID)
    {
        currentAttackID = attackID;
       
        //yield return attacks[attackID].AttackWait;
        attacks[attackID].DetectUnits();       

        if (attackButtonPressed)
        {
            currentAttackID++;

            if (currentAttackID >= attacks.Length)
                currentAttackID = 2;

            Debug.Log(currentAttackID);
            _animator.SetTrigger(attacks[0].triggerName);
            attackButtonPressed = false;

        }
        else
            IsAttacking = false;

        yield break;
    }*/
    /*
    void AttackEnded(int index)
    {       
       //IsAttacking = false;  
        
       Attacks[index] = false;

       bool check = false;
       for (int i = 0; i < Attacks.Length; i++)
       {
           if (Attacks[i])
           {
               check = true;
           }
       }

       if (!check)
       {
           IsAttacking = false;           
       }
       //_animator.SetTrigger("attack_ended");
    }
*/

}
