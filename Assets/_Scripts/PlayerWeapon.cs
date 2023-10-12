using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using Cinemachine;
using StarterAssets;

enum WeaponStates {
    Sword,
    Bow
}

public class PlayerWeapon : MonoBehaviour
{
    
    [SerializeField] GameObject bullet;
    [SerializeField] float bulletSpeed;
    [SerializeField] GameObject shield;
    [SerializeField] GameObject sword;
    [SerializeField] float swordDamageToEnemy;
    AudioManager audioManager;
    public bool hasBow, paused, blocking;
    PlayerInventory playerInventory;
    PlayerH playerH;
    PlayerMovement playerMovement;
    Animator anim;
    WeaponStates weaponStates;

    [Range(0.01f, 0.35f)]
    [SerializeField] float autoFireDelay;
    float autoFireTimer = 0f;
    [Range(0.5f, 2.0f)]
    [SerializeField] float fireDelay;
    float fireTimer;
    int ammunition = 0;
    Camera mainCamera;
    CinemachineImpulseSource source;
    private StarterAssetsInputs starterAssetsInputs;
    private int animIDAttack;
    private int animIDAim;
    private int animIDShoot;
    private int animIDBlock;
    private int animIDAtkIndex;
    [SerializeField] LayerMask aimColliderLayerMask;

    [SerializeField] private CinemachineVirtualCamera aimVirtualCamera;
    [SerializeField] float normalSensitivity;
    [SerializeField] float aimSensitivity;
    ThirdPersonController thirdPersonController;
    Vector3 mouseWorldPosition;
    [SerializeField] Transform spawnBulletPosition;
    bool aiming;


    // Start is called before the first frame update

    void Start()
    {
        fireTimer = fireDelay;
        anim = GetComponent<Animator>();
        audioManager = GameObject.FindGameObjectWithTag("AudioManager").GetComponent<AudioManager>();
        playerInventory = GetComponent<PlayerInventory>();
        playerH = GetComponent<PlayerH>();
        mainCamera = Camera.main;
        playerMovement = GetComponent<PlayerMovement>();
        weaponStates = WeaponStates.Sword;
        starterAssetsInputs = GetComponent<StarterAssetsInputs>();
        thirdPersonController = GetComponent<ThirdPersonController>();

        AssignAnimationIDs();
    }

    void Awake() {
        mainCamera = Camera.main;
    }

    void AssignAnimationIDs()
    {
        animIDAttack = Animator.StringToHash("attack");
        animIDAim = Animator.StringToHash("aimArrow");
        animIDShoot = Animator.StringToHash("shootArrow");
        animIDBlock = Animator.StringToHash("block");
        animIDAtkIndex = Animator.StringToHash("atkIndex");
    }

    void Aim()
    {
        mouseWorldPosition = Vector3.zero;

        Ray ray = mainCamera.ScreenPointToRay(new Vector2(Screen.width/2f, Screen.height/2f));
        if(Physics.Raycast(ray, out RaycastHit raycastHit, 999f, aimColliderLayerMask))
        {
            mouseWorldPosition = raycastHit.point;
        }

        if(starterAssetsInputs.aim && GetGun())
        {
            thirdPersonController.SetSensitivity(aimSensitivity);
            thirdPersonController.SetRotateOnMove(false);
            aimVirtualCamera.gameObject.SetActive(true);
            aiming = true;
            

            Vector3 worldAimTarget = mouseWorldPosition;
            worldAimTarget.y = transform.position.y;
            Vector3 aimDirection = (worldAimTarget - transform.position).normalized;

            transform.forward = Vector3.Lerp(transform.forward, aimDirection, Time.deltaTime * 20f);
        }
        else
        {
            thirdPersonController.SetSensitivity(normalSensitivity);
            aimVirtualCamera.gameObject.SetActive(false);
            thirdPersonController.SetRotateOnMove(true);
            aiming = false;
        }
    }

    public bool GetAiming()
    {
        return aiming;
    }

    // Update is called once per frame
    void Update()
    {
        paused = TheGameManager.paused;
        if(!paused)
        {
            Aim();
            Fire();
        }


        if (hasBow)
        {
            sword.SetActive(false);
            shield.SetActive(false);
        }
        else if (!hasBow)
        {
            sword.SetActive(true);
            shield.SetActive(true);
        }

        if (Input.GetKeyDown(KeyCode.Q))
        {
            if (ammunition > 0)
                SwitchGun();
        }
        if (ammunition <= 0)
            hasBow = false;
        
        BlockShield();
    }

    // private void OnTriggerEnter(Collider collision)
    // {
    //     if (collision.gameObject.tag == "Gun")
    //     {
    //         ammunition += 30;
    //         audioManager.AudioWeaponPickUp();
    //         Destroy(collision.gameObject);
    //     }
    // }

    private void SwitchGun()
    {
        hasBow = !hasBow;
    }

    void SwitchToBow()
    {
        weaponStates = WeaponStates.Bow;
    }
    void SwitchToSword()
    {
        weaponStates = WeaponStates.Sword;
    }

    public bool GetGun()
    {
        return hasBow;
    }

    public int GetAmmunition()
    {
        return ammunition;
    }
    public void IncreaseAmmunition(int value)
    {
        //audioManager.AudioWeaponPickUp();
        ammunition += value;
    }

    void Fire()
    {
        blocking = playerH.GetBlocking();
        if (!blocking)
        {
            fireTimer += Time.deltaTime;
            if (starterAssetsInputs.attack && !paused)
            {
                if (fireTimer >= fireDelay)
                {
                    if(!hasBow)
                    {
                        anim.SetInteger(animIDAtkIndex, Random.Range(0,2));
                        anim.SetTrigger(animIDAttack);
                        fireTimer = 0f;
                    }
                }
            }
        }

        if(starterAssetsInputs.aim && !paused)
        {
            if(hasBow)
            {
                anim.SetBool(animIDAim, true);
                fireTimer += Time.deltaTime;
                if(starterAssetsInputs.attack && !paused)
                {
                    if (fireTimer >= fireDelay)
                    {
                        anim.SetTrigger(animIDShoot);
                        ArrowShot();
                        ammunition--;
                        fireTimer = -2f;
                    }
                }
            }
        }

        else if(Input.GetKeyUp(KeyCode.Mouse1))
        {
            anim.SetBool(animIDAim, false);
        }
    }


    void ArrowShot()
    {
        Vector3 aimDir = (mouseWorldPosition - spawnBulletPosition.position).normalized;
        
        //Vector3 aimDirection = Camera.main.ScreenPointToRay(new Vector2(Screen.width/2f, Screen.height/2f)).direction;
        
        GameObject bulletInst = ObjectPool.objectPoolInstance.SpawnFromPool("PlayerArrow", spawnBulletPosition.position, Quaternion.LookRotation(aimDir, Vector3.up));

        audioManager.AudioPlayerArrowLaunch();

        source = GetComponent<CinemachineImpulseSource>();

        source.GenerateImpulse(mainCamera.transform.forward);
        
        
    }

    void BlockShield()
    {
        if (!hasBow)
        {
            PlayerH playerH = GetComponent<PlayerH>();
            if (starterAssetsInputs.aim && !paused)
            {
                playerH.Block();
                //playerMovement.BlockSpeed();
                anim.SetBool(animIDBlock, true);
            }
            else if (!starterAssetsInputs.aim && !paused)
            {
                playerH.Block();
                //playerMovement.DefSpeed();
                anim.SetBool(animIDBlock, false);
            }
        }
    }

    [SerializeField] LayerMask hitLayer;
    [SerializeField] int gridSize = 4;
    [SerializeField] float raySpacing = 0.5f;
    [SerializeField] float rayLength = 1.7f;

    public void SwordHit()
    {
        // Get the direction of the ray (e.g., forward).
        Vector3 rayDirection = transform.forward;

        // Calculate the starting point for the ray grid.
        Vector3 startPoint = transform.position + transform.up;
        bool playAttackAudio = true;

        // Cast a grid of rays.
        for (int i = 0; i < gridSize; i++)
        {
            for (int j = 0; j < gridSize; j++)
            {
                // Calculate the position of the current ray in the grid.
                Vector3 rayOrigin = startPoint +
                    transform.right * (i * raySpacing - (raySpacing * (gridSize - 1) / 2f)) +
                    transform.up * (j * raySpacing - (raySpacing * (gridSize - 1) / 2f));

                // Cast the ray.
                RaycastHit hit;
                if (Physics.Raycast(rayOrigin, rayDirection, out hit, rayLength))
                {
                    // Handle the hit here.
                    Debug.DrawLine(rayOrigin, hit.point, Color.red);
                    if (hit.transform.gameObject.tag == "Warrior" || hit.transform.gameObject.tag == "Archer")
                    {
                        
                        EnemyH enemyH = hit.transform.gameObject.GetComponent<EnemyH>();
                        enemyH.Damage(swordDamageToEnemy);


                        //audioAttackTimer += Time.deltaTime;
                        if(playAttackAudio)
                        {
                            audioManager.AudioSwordHit();
                            playAttackAudio = false;
                        }
                    }
                }
                else
                {
                    // No hit occurred.
                    Debug.DrawRay(rayOrigin, rayDirection * rayLength, Color.green);
                }
            }
            playAttackAudio = true;
        }

    }
}

