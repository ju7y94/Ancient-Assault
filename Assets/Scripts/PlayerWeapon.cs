using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class PlayerWeapon : MonoBehaviour
{
    
    [SerializeField] GameObject bullet;
    [SerializeField] float bulletSpeed;

    [SerializeField] GameObject bow;
    [SerializeField] GameObject shield;
    [SerializeField] GameObject sword;

    [SerializeField] float swordDamageToEnemy;
    [SerializeField] float arrowDamageToEnemy;

    AudioManager audioManager;

    public bool hasBow, paused, blocking;

    PlayerInventory playerInventory;
    PlayerHealth playerHealth;
    PlayerMovement playerMovement;


    [SerializeField] Image crosshair;
    [SerializeField] Camera fpsCam;

    

    Animator anim;

    [Range(0.01f, 0.35f)]
    [SerializeField] float autoFireDelay;
    float autoFireTimer = 0f;
    [Range(0.5f, 2.0f)]
    [SerializeField] float fireDelay;
    float fireTimer;
    int ammunition = 0;


    // Start is called before the first frame update

    void Start()
    {
        fireTimer = fireDelay;
        anim = GetComponent<Animator>();
        audioManager = GameObject.FindGameObjectWithTag("AudioManager").GetComponent<AudioManager>();
        playerInventory = GetComponent<PlayerInventory>();
        playerHealth = GetComponent<PlayerHealth>();
        playerMovement = GetComponent<PlayerMovement>();
    }

    // Update is called once per frame
    void Update()
    {
        paused = TheGameManager.paused;

        if (!playerInventory.GetHasBoth())
        {
            Fire();
        }

        if (hasBow)
        {
            bow.SetActive(true);
            sword.SetActive(false);
            shield.SetActive(false);
        }
        else if (!hasBow)
        {
            bow.SetActive(false);
            sword.SetActive(true);
            shield.SetActive(true);
        }

        if (Input.GetKeyDown(KeyCode.Q))
        {
            if (ammunition > 0)
            {
                SwitchGun();
            }
        }
        if (ammunition <= 0)
        {
            hasBow = false;
            
        }

        BlockShield();
    }

    private void OnCollisionEnter(Collision collision)
    {
        if (collision.gameObject.tag == "Gun")
        {
            ammunition += 30;
            audioManager.AudioWeaponPickUp();
            Destroy(collision.gameObject);
        }
    }

    private void SwitchGun()
    {
        hasBow = !hasBow;
    }

    public bool GetGun()
    {
        return hasBow;
    }

    public int GetAmmunition()
    {
        return ammunition;
    }

    void Fire()
    {
        blocking = playerHealth.GetBlocking();
        if (!blocking)
        {
            fireTimer += Time.deltaTime;
            if (Input.GetButton("Fire1") && !paused)
            {
                if (fireTimer >= fireDelay)
                {

                    
                    if (hasBow)
                    {
                        ArrowShot();
                        ammunition--;
                        fireTimer = 0f;
                    }
                    else
                    {
                        //anim.SetBool("attack", true);
                        anim.SetInteger("atkIndex", Random.Range(0,2));
                        anim.SetTrigger("attacks");
                        //SwordHit();
                        fireTimer = 0f;
                    }
                }
            }
            else
            {
                //anim.SetBool("attack", false);
            }
        }

    }


    void ArrowShot()
    {
        GameObject bulletInst = Instantiate(bullet, gameObject.transform.position + Vector3.up / 3, Quaternion.LookRotation(fpsCam.ScreenPointToRay(new Vector2(Screen.width/2, Screen.height/2)).direction));
        Rigidbody bulletInstRB = bulletInst.GetComponent<Rigidbody>();
        bulletInstRB.AddForce(fpsCam.ScreenPointToRay(new Vector2(Screen.width / 2, Screen.height / 2)).direction * bulletSpeed, ForceMode.Impulse);
        Destroy(bulletInst, 5f);
        audioManager.AudioPlayerArrowLaunch();
        print(gameObject.transform.position);
    }
    public void SwordHit()
    {
        RaycastHit hit;
        if (Physics.SphereCast(transform.position + transform.up / 2, 0.3f, transform.forward, out hit, 1.5f))
        {
            if (hit.transform != null)
            {
                if (hit.transform.gameObject.tag == "Enemy" || hit.transform.gameObject.tag == "PoisonedEnemy")
                {
                    audioManager.AudioSwordHit();
                    EnemyScript enemyObject = hit.transform.gameObject.GetComponent<EnemyScript>();
                    enemyObject.DealDamage(swordDamageToEnemy);
                }
            }
        }
    }

    void BlockShield()
    {
        if (!hasBow)
        {
            PlayerHealth playerHealth = GetComponent<PlayerHealth>();
            if (Input.GetButtonDown("Fire2") && !paused)
            {
                playerHealth.Block();
                playerMovement.BlockSpeed();
                anim.SetBool("block", true);
            }
            else if (Input.GetButtonUp("Fire2") && !paused)
            {
                playerHealth.Block();
                playerMovement.DefSpeed();
                anim.SetBool("block", false);
            }
        }

    }

}
