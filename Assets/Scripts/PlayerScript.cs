using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerScript : MonoBehaviour
{
    [Header("Player Movement")]
    public float playerSpeed = 2.0f;
    public float playerSprint = 3.0f;


    [Header("Player Health Things")]
    private float playerHealth = 120;
    private float presentHealth;
    public HealthBar healthBar;

    [Header("Player Script Cameras")]
    public Transform playerCamera;


    [Header("Player Animator and Gravity")]
    public CharacterController cC;
    public float gravity = -9.81f;
    public Animator animator;


    [Header("Player Jumping and Velocity")]
    public float jumpRange = 1f;
    Vector3 velocity;
    public float turnCalmTime = 0.1f;
    float turnCalmVelocity;
    public Transform surfaceCheck;
    bool onSurface;
    public float surfaceDistance = 0.4f;
    public LayerMask surfaceMask;


    // Start is called before the first frame update
    void Start()
    {
        Cursor.lockState = CursorLockMode.Locked;
        presentHealth = playerHealth;
        healthBar.GivefullHealth(playerHealth);
    }

    // Update is called once per frame
    void Update()
    {
        onSurface = Physics.CheckSphere(surfaceCheck.position, surfaceDistance, surfaceMask);

        if(onSurface && velocity.y < 0)
        {
            velocity.y = -2f;
        }

        //gravity
        velocity.y += gravity * Time.deltaTime;
        cC.Move(velocity * Time.deltaTime);
        

        playerMove();

        Jump();

        Sprint();
    }

    void playerMove()
    {
        float horizontal_axis = Input.GetAxisRaw("Horizontal");
        float vertical_axis = Input.GetAxisRaw("Vertical");

        Vector3 direction = new Vector3(horizontal_axis,0f ,vertical_axis).normalized;

        if(direction.magnitude >= 0.1f)
        {

            animator.SetBool("Walk",true);
            animator.SetBool("Running",false);
            animator.SetBool("Idle",false);
            animator.SetTrigger("Jump");
            animator.SetBool("AimWalk",false);
            animator.SetBool("IdleAim",false);

            float targetAngle = Mathf.Atan2(direction.x, direction.z) * Mathf.Rad2Deg + playerCamera.eulerAngles.y;
            float angle =  Mathf.SmoothDampAngle(transform.eulerAngles.y, targetAngle, ref turnCalmVelocity , 0f);
            transform.rotation = Quaternion.Euler(0f, angle, 0f);
            
            Vector3 moveDirection = Quaternion.Euler(0f, targetAngle, 0f) * Vector3.forward;
            cC.Move(moveDirection.normalized * playerSpeed * Time.deltaTime);
        }
        else
        {
            animator.SetBool("Idle",true);
            animator.SetTrigger("Jump");
            animator.SetBool("Walk",false);
            animator.SetBool("Running",false);
            animator.SetBool("AimWalk",false);

            if (Input.GetButton("Fire2"))
            {
                float targetAngle = Mathf.Atan2(direction.x, direction.z) * Mathf.Rad2Deg + playerCamera.eulerAngles.y;
                float angle = Mathf.SmoothDampAngle(transform.eulerAngles.y, targetAngle, ref turnCalmVelocity, 0f);
                transform.rotation = Quaternion.Euler(0f, angle, 0f);

                //float targetAngle1 = Mathf.Atan2(direction.y, direction.z) * Mathf.Rad2Deg + playerCamera.eulerAngles.x;
                //float angle1 = Mathf.SmoothDampAngle(transform.eulerAngles.x, targetAngle1, ref turnCalmVelocity, 0f);
               //0 transform.rotation = Quaternion.Euler(angle1, 0f, 0f);
            }
        }
    }

    void Jump()
    {
        if(Input.GetButtonDown("Jump") && onSurface)
        {
            animator.SetBool("Walk",false);
            animator.SetTrigger("Jump");
            velocity.y = Mathf.Sqrt(jumpRange * -2 * gravity);
        }
        else
        {
            animator.ResetTrigger("Jump");
        }
    }

    void Sprint()
    {
        if(Input.GetButton("Sprint") && Input.GetKey(KeyCode.W) || Input.GetKey(KeyCode.UpArrow) && onSurface)
        {  
            float horizontal_axis = Input.GetAxisRaw("Horizontal");
            float vertical_axis = Input.GetAxisRaw("Vertical");

            Vector3 direction = new Vector3(horizontal_axis,0f ,vertical_axis).normalized;

            if(direction.magnitude >= 0.1f)
            {
                animator.SetBool("Idle",false);
                animator.SetBool("Running",true);
                
                animator.SetBool("Walk",false);
                animator.SetBool("IdleAim",false);
                float targetAngle = Mathf.Atan2(direction.x, direction.z) * Mathf.Rad2Deg + playerCamera.eulerAngles.y;
                float angle =  Mathf.SmoothDampAngle(transform.eulerAngles.y, targetAngle, ref turnCalmVelocity , turnCalmTime);
                transform.rotation = Quaternion.Euler(0f, angle, 0f);
                
                Vector3 moveDirection = Quaternion.Euler(0f, targetAngle, 0f) * Vector3.forward;
                cC.Move(moveDirection.normalized * playerSprint * Time.deltaTime);
            }
            else
            {
                animator.SetBool("Idle",false);
                animator.SetBool("Walk",false);
            }
        }
    }

    public void playerHitDamage(float takeDamage)
    {
        presentHealth -= takeDamage;
        healthBar.SetHealth(presentHealth);

        if(presentHealth <= 0)
        {
            PlayerDie();
        }
    }

    void PlayerDie()
    {
        Cursor.lockState = CursorLockMode.None;
        Object.Destroy(gameObject,1.0f);
    }
}
