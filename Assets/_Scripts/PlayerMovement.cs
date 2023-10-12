using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.InputSystem;
using StarterAssets;

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
    

    void Start() {
        rb = GetComponent<Rigidbody>();
        playerWeapon = GetComponent<PlayerWeapon>();
        animator = GetComponent<Animator>();
        controller = GetComponent<CharacterController>();
        controller.enabled = true;
        animator.SetBool("idle", true);
        defaultSpeed = speed;
        _playerInput = GetComponent<PlayerInput>();
        _input = GetComponent<StarterAssetsInputs>();
        _cinemachineTargetYaw = CinemachineCameraTarget.transform.rotation.eulerAngles.y;
    }

    void LateUpdate()
    {
        CameraRotation();
    }

    void Update()
    {   
        Jumping();
        Movement();
        controller.Move(playerVelocity * Time.deltaTime);
    }

    private PlayerInput _playerInput;
    private StarterAssetsInputs _input;
    private float _cinemachineTargetYaw;
    private float _cinemachineTargetPitch;
    private const float _threshold = 0.01f;
    public bool LockCameraPosition = false;
    public float TopClamp = 70.0f;
    public float BottomClamp = -30.0f;
    public GameObject CinemachineCameraTarget;
    public float CameraAngleOverride = 0.0f;
    public float sensitivity = 1.0f;

    private bool IsCurrentDeviceMouse
        {
            get
            {
#if ENABLE_INPUT_SYSTEM && STARTER_ASSETS_PACKAGES_CHECKED
                return _playerInput.currentControlScheme == "KeyboardMouse";
#else
				return false;
#endif
            }
        }

    private static float ClampAngle(float lfAngle, float lfMin, float lfMax)
    {
        if (lfAngle < -360f) lfAngle += 360f;
        if (lfAngle > 360f) lfAngle -= 360f;
        return Mathf.Clamp(lfAngle, lfMin, lfMax);
    }

    private void CameraRotation()
    {
            // if there is an input and camera position is not fixed
            if (_input.look.sqrMagnitude >= _threshold && !LockCameraPosition)
            {
                //Don't multiply mouse input by Time.deltaTime;
                float deltaTimeMultiplier = IsCurrentDeviceMouse ? 1.0f : Time.deltaTime;

                _cinemachineTargetYaw += _input.look.x * deltaTimeMultiplier * sensitivity;
                _cinemachineTargetPitch += _input.look.y * deltaTimeMultiplier * sensitivity;
            }

            // clamp our rotations so our values are limited 360 degrees
            _cinemachineTargetYaw = ClampAngle(_cinemachineTargetYaw, float.MinValue, float.MaxValue);
            _cinemachineTargetPitch = ClampAngle(_cinemachineTargetPitch, BottomClamp, TopClamp);

            // Cinemachine will follow this target
            CinemachineCameraTarget.transform.rotation = Quaternion.Euler(_cinemachineTargetPitch + CameraAngleOverride,
                _cinemachineTargetYaw, 0.0f);
    }

    public void SetSensitivity(float newSensitivity)
    {
        sensitivity = newSensitivity;
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
        speed *= 1.5f;
    }
}
