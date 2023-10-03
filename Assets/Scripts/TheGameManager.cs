using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class TheGameManager : MonoBehaviour
{
    [SerializeField] GameObject pausePanel, gameOverPanel, winPanel, controlPanel;
    public static TheGameManager tgm;
    public static GameObject playerInstance;
    static Transform playerInstanceTransform;
    [SerializeField] Image healthBar;
    [SerializeField] Text lifeText;
    [SerializeField] Image bleedingImage, goldenM, silverM;
    [SerializeField] Text ammoAmount, bleedingText;
    float playerCurrentHealth;
    public static bool paused;
    public static bool playerAlive;
    public static bool gameOver;
    public static int lives;
    bool gameControlPanel;
    [SerializeField] Transform playerSpawnLocation;
    [SerializeField] GameObject playerPrefab;
    [SerializeField] Camera deathCamera;
    AudioManager audioManager;
    AudioListener audioListener;
    EnemyManager enemyManager;
    bool won, goldMush, silverMush;
    PlayerWeapon playerWeapon;
    [SerializeField] Image crosshair;

    // Start is called before the first frame update
    void Start()
    {
        gameControlPanel = false;
        paused = false;
        gameOver = false;
        won = false;
        tgm = this;
        tgm.deathCamera.enabled = true;
        lives = 3;
        Time.timeScale = 1f;
        tgm.gameOverPanel.gameObject.SetActive(false);
        audioManager = GameObject.FindGameObjectWithTag("AudioManager").GetComponent<AudioManager>();
        enemyManager = GameObject.FindGameObjectWithTag("EnemyManager").GetComponent<EnemyManager>();
        enemyManager.Invoke("PlaceNewEnemy", 2f);
        tgm.Invoke("PlacePlayer", 2.0f);
        //playerInstance = Instantiate(playerPrefab, playerSpawnLocation.position, Quaternion.identity);
        //playerInstance.SetActive(false);
        lifeText.text = "x " + lives.ToString();
    }

    // Update is called once per frame
    void Update()
    {
        //print(playerInstance.transform.position);
        if (Input.GetKeyDown(KeyCode.Escape) && !gameOver)
        {
            if(!gameControlPanel)
            {
                paused = !paused;
            }
            //paused = !paused;
            if (paused)
            {
                if(gameControlPanel)
                {
                    controlPanel.SetActive(false);
                    SwitchControlPanel();
                }
                else
                {
                    tgm.pausePanel.SetActive(true);
                    Time.timeScale = 0f;
                }
            }
            else if(!paused)
            {
                tgm.pausePanel.SetActive(false);
                Time.timeScale = 1f;
            }
        }

        if(paused || gameOver || won)
        {
            Cursor.lockState = CursorLockMode.None;
        }
        else
        {
            Cursor.lockState = CursorLockMode.Locked;
        }
        if (playerInstance != null)
        {
            UpdateUI();
            audioListener = GetComponent<AudioListener>();
            audioListener.enabled = false;
        }
        

        if(playerInstance == null)
        {
            audioListener = GetComponent<AudioListener>();
            audioListener.enabled = true;
            playerAlive = false;
            tgm.bleedingText.gameObject.SetActive(false);
            
            //tgm.momentumIcon.color = new Color(momentumIcon.color.r, momentumIcon.color.g, momentumIcon.color.b, 0.0f);
            tgm.bleedingImage.color = new Color(bleedingImage.color.r, bleedingImage.color.g, bleedingImage.color.b, 0.0f);
            tgm.goldenM.color = new Color(goldenM.color.r, goldenM.color.g, goldenM.color.b, 1.0f);
            tgm.silverM.color = new Color(silverM.color.r, silverM.color.g, silverM.color.b, 1.0f);
        }
        
        if(playerAlive)
        {
            
            won = playerInstance.GetComponent<PlayerInventory>().GetHasBoth();
            if(won)
            {
                Time.timeScale = 0f;
                tgm.winPanel.gameObject.SetActive(true);
            }
            tgm.deathCamera.enabled = false;
        }
        else if(!playerAlive)
        {
            tgm.deathCamera.enabled = true;
            lifeText.text = "x " + lives.ToString();
        }
    }

    public void PlacePlayer()
    {
        
        audioManager.AudioRespawnPlayer();
        playerAlive = true;
        playerInstance = Instantiate(playerPrefab, playerSpawnLocation.position, Quaternion.identity);
        playerInstanceTransform = playerInstance.transform;
        //playerInstance.SetActive(true);
        //playerInstance.transform.position = playerSpawnLocation.position;
        playerWeapon = playerInstance.GetComponent<PlayerWeapon>();
    }

    public static GameObject GetPlayerInstance()
    {
        return playerInstance;
    }

    public void Unpause()
    {
        paused = false;
        tgm.pausePanel.SetActive(false);
        Time.timeScale = 1f;
    }

    public void UpdateUI()
    {
        if (playerInstance != null) { 

            if (playerAlive)
            {
                if(playerWeapon.GetGun())
                {
                    crosshair.gameObject.SetActive(true);
                }
                else if(!playerWeapon.GetGun())
                {
                    crosshair.gameObject.SetActive(false);
                }
                goldMush = playerInstance.GetComponent<PlayerInventory>().GetGolden();
                if (goldMush)
                {
                    tgm.goldenM.color = new Color(goldenM.color.r, goldenM.color.g, goldenM.color.b, 1f);
                }
                else
                {
                    tgm.goldenM.color = new Color(goldenM.color.r, goldenM.color.g, goldenM.color.b, 0.35f);
                }
                silverMush = playerInstance.GetComponent<PlayerInventory>().GetSilver();
                if (silverMush)
                {
                    tgm.silverM.color = new Color(silverM.color.r, silverM.color.g, silverM.color.b, 1f);
                }
                else
                {
                    tgm.silverM.color = new Color(silverM.color.r, silverM.color.g, silverM.color.b, 0.35f);
                }
                playerCurrentHealth = playerInstance.GetComponent<PlayerH>().GetCurrentHealth();
                tgm.healthBar.fillAmount = playerCurrentHealth / 100f;
                //bool momentum = playerInstance.GetComponent<PlayerWeapon>().GetMomentum();
                tgm.ammoAmount.text = playerInstance.GetComponent<PlayerWeapon>().GetAmmunition().ToString();

                // bool bleeding = playerInstance.GetComponent<PlayerH>().GetPoisoned();
                // if (bleeding)
                // {
                //     tgm.bleedingImage.color = new Color(bleedingImage.color.r, bleedingImage.color.g, bleedingImage.color.b, 1f);
                //     tgm.bleedingText.gameObject.SetActive(true);
                // }
                // else if (!bleeding)
                // {
                //     tgm.bleedingImage.color = new Color(bleedingImage.color.r, bleedingImage.color.g, bleedingImage.color.b, 0.3f);
                //     tgm.bleedingText.gameObject.SetActive(false);
                // }

                /*if (momentum)
                {
                    tgm.momentumIcon.color = new Color(momentumIcon.color.r, momentumIcon.color.g, momentumIcon.color.b, 1f);
                }
                else if (!momentum)
                {
                    tgm.momentumIcon.color = new Color(momentumIcon.color.r, momentumIcon.color.g, momentumIcon.color.b, 0.3f);
                }*/
            }
    }
        else
        {
            tgm.healthBar.fillAmount = 0f;
        }
    }
    
    public void SwitchControlPanel()
    {
        gameControlPanel = !gameControlPanel;
    }

    static public void PlayerDeath()
    {
        playerAlive = false;
        lives--;
        if(lives > 0)
        {
            Destroy(playerInstance);
            //playerInstance.SetActive(false);
            gameOver = false;
            tgm.Invoke("PlacePlayer", 3.0f);
        }
        else if(lives <=0)
        {
            print("GameOver");
            Time.timeScale = 0f;
            tgm.gameOverPanel.gameObject.SetActive(true);
            gameOver = true;
            tgm.deathCamera.enabled = true;
            playerInstance.SetActive(false);
        }      
    }

    public static void GetPlayerTransform(Transform input)
    {
        input = playerInstanceTransform;
    }
}
