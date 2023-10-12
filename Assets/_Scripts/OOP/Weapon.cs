using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Weapon : MonoBehaviour
{
    ///////////////////////////
    //SWORD RAYCAST VARIABLES//

    [SerializeField] LayerMask hitLayer;
    [SerializeField] int gridSize = 4;
    [SerializeField] float raySpacing = 0.5f;
    [SerializeField] float rayLength = 1.7f;
    
    ////////////////////
    //OTHER REFERENCES//
    [SerializeField] float fireDelay;
    float fireTimer;
    AudioManager audioManager;
    Animator anim;
    PlayerInventory playerInventory;
    PlayerH playerH;
    Camera mainCamera;
    PlayerMovement playerMovement;


    void Start()
    {
        fireTimer = fireDelay;
        anim = GetComponent<Animator>();
        audioManager = GameObject.FindGameObjectWithTag("AudioManager").GetComponent<AudioManager>();
        playerInventory = GetComponent<PlayerInventory>();
        playerH = GetComponent<PlayerH>();
        mainCamera = Camera.main;
        playerMovement = GetComponent<PlayerMovement>();
    }

    public void SwordHit()
    {
        Vector3 rayDirection = transform.forward;
        Vector3 startPoint = transform.position + transform.up;

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
                        audioManager.AudioSwordHit();
                        EnemyH enemyH = hit.transform.gameObject.GetComponent<EnemyH>();
                        //enemyH.Damage(swordDamageToEnemy);
                    }
                }
                else
                {
                    // No hit occurred.
                    Debug.DrawRay(rayOrigin, rayDirection * rayLength, Color.green);
                }
            }
        }
    }
}
