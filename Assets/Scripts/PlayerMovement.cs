using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerMovement : MonoBehaviour
{
    //public static PlayerMovement Instance { get; private set; }
    [SerializeField] float speed;
    float defaultSpeed;
    float blockingSpeed = 0.5f;
    [SerializeField] float jumpForce;
    float jumpDelay = 0.69f;
    float jumpTimer = 0.69f;
    [SerializeField] float gravity;
    Rigidbody rb;
    float vDir, hDir;
    PlayerWeapon playerWeapon;
    bool momentum;
    private Animator animator;
    private CharacterController controller;
    Vector3 playerVelocity;
    Vector3 moveDirection;
    [SerializeField] Camera fpsCamera;
    

    void Start() {
        rb = GetComponent<Rigidbody>();
        playerWeapon = GetComponent<PlayerWeapon>();
        animator = GetComponent<Animator>();
        controller = GetComponent<CharacterController>();
        controller.enabled = true;
        animator.SetBool("idle", true);
        defaultSpeed = speed;
    }

    void FixedUpdate()
    {
        
    }

    void Update()
    {
        Jumping();
        Movement();

        controller.Move(playerVelocity * Time.deltaTime);
    }
    

    void Movement()
    {
        hDir = Input.GetAxis("Horizontal");
        vDir = Input.GetAxis("Vertical");
        /*hDir = Mathf.Clamp(hDir, -1, 1);
        vDir = Mathf.Clamp(vDir, -1, 1);*/
        if (Input.GetButtonDown("Fire3") && !playerWeapon.blocking)
        {
            RunSpeed();
            animator.SetBool("run", true);
            animator.SetBool("walk", false);
        }
        else if(Input.GetButtonUp("Fire3") && !playerWeapon.blocking)
        {
            DefSpeed();
            animator.SetBool("run", false);
            animator.SetBool("walk", true);
        }
        moveDirection = transform.right * hDir + transform.forward * vDir;
        moveDirection.x = Mathf.Clamp(moveDirection.x, -1, 1);
        moveDirection.z = Mathf.Clamp(moveDirection.z, -1, 1);
        controller.Move(moveDirection * Time.deltaTime * speed);
        
        //momentum = playerWeapon.GetMomentum();

        //animator.SetBool("walk", Input.GetKey(KeyCode.W));
        if(Input.GetKey(KeyCode.W) || Input.GetKey(KeyCode.A) || Input.GetKey(KeyCode.S) || Input.GetKey(KeyCode.D ))
        {
            animator.SetBool("walk", true);
        }
        else
        {
            animator.SetBool("walk", false);
        }
    }

    void Jumping()
    {
        playerVelocity.y += gravity * Time.deltaTime;
        jumpTimer += Time.deltaTime;
        
        if(jumpTimer >= jumpDelay)
        {
            if (controller.isGrounded)
            {
                playerVelocity.y = -2f;
                if (Input.GetButtonDown("Jump"))
                {
                    playerVelocity.y += Mathf.Sqrt(jumpForce * -1.0f * gravity);
                    animator.SetBool("jump", true);
                    jumpTimer = 0f;
                }
                else
                {
                    animator.SetBool("jump", false);
                }
            }
            
        }
        
    }

    bool IsGrounded()
    {
        float distanceToGround = 0.20f;
        return Physics.Raycast(controller.transform.position, Vector3.down, distanceToGround);
    }

    private void OnTriggerExit(Collider other)
    {
        if(other.gameObject.tag == "Boundary")
        {
            controller.Move(-moveDirection);
        }
    }
    public void BlockSpeed()
    {
        speed = blockingSpeed;
    }
    public void DefSpeed()
    {
        speed = defaultSpeed;
    }
    public void RunSpeed()
    {
        speed += 0.5f;
    }
}
