using UnityEngine;
using UnityEngine.UI;
using StarterAssets;

public class TheGameManager : MonoBehaviour
{
    [SerializeField] GameObject pausePanel, gameOverPanel, winPanel, controlPanel, inventoryPanel;
    public static TheGameManager Instance;
    public static GameObject playerInstance;
    static Transform playerInstanceTransform;
    [SerializeField] Image healthBar;
    [SerializeField] Text lifeText;
    [SerializeField] Image goldenM, silverM;
    [SerializeField] Text ammoAmount;
    float playerCurrentHealth;
    public static bool paused;
    public static bool playerAlive;
    public static bool gameOver;
    public static int lives;
    bool gameControlPanel;
    [SerializeField] Transform playerSpawnLocation;
    [SerializeField] GameObject playerPrefab;
    AudioManager audioManager;
    //AudioListener audioListener;
    EnemyManager enemyManager;
    bool won, goldMush, silverMush, inventoryState;
    PlayerWeapon playerWeapon;
    [SerializeField] Image crosshair;
    StarterAssetsInputs starterAssetsInputs;

    private void Awake() 
    {
        Instance = this;
    }
    void Start()
    {
        inventoryState = false;
        starterAssetsInputs = GetComponent<StarterAssetsInputs>();
        gameControlPanel = false;
        paused = false;
        gameOver = false;
        won = false;
        lives = 3;
        Time.timeScale = 1f;
        Instance.gameOverPanel.gameObject.SetActive(false);
        audioManager = GameObject.FindGameObjectWithTag("AudioManager").GetComponent<AudioManager>();
        enemyManager = GameObject.FindGameObjectWithTag("EnemyManager").GetComponent<EnemyManager>();
        enemyManager.Invoke("PlaceNewEnemy", 2f);
        Instance.Invoke("InstantiatePlayer", 2.0f);
        lifeText.text = "x " + lives.ToString();
    }

    void SetInventoryState()
    {
        inventoryState = !inventoryState;
    }

    // Update is called once per frame
    void Update()
    {
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
                    pausePanel.SetActive(true);
                    Time.timeScale = 0f;
                }
            }
            else if(!paused)
            {
                pausePanel.SetActive(false);
                Time.timeScale = 1f;
            }
        }
        if (Input.GetKeyDown(KeyCode.Tab))
        {
            SetInventoryState();
            inventoryPanel.SetActive(inventoryState);
        }

        if(paused || gameOver || won || inventoryState)
        {
            Cursor.lockState = CursorLockMode.None;
        }
        else
        {
            Cursor.lockState = CursorLockMode.Locked;
        }
        if (playerInstance != null)
        {
            playerWeapon = playerInstance.GetComponent<PlayerWeapon>();
            UpdateUI();
            //audioListener = GetComponent<AudioListener>();
            //audioListener.enabled = false;
            playerAlive = true;
        }
        

        if(playerInstance == null)
        {
            //audioListener = GetComponent<AudioListener>();
            //audioListener.enabled = true;
            playerAlive = false;
            Instance.goldenM.color = new Color(goldenM.color.r, goldenM.color.g, goldenM.color.b, 1.0f);
            Instance.silverM.color = new Color(silverM.color.r, silverM.color.g, silverM.color.b, 1.0f);
        }
        
        if(playerAlive)
        {
            
            won = playerInstance.GetComponent<PlayerInventory>().GetHasBoth();
            if(won)
            {
                Time.timeScale = 0f;
                Instance.winPanel.gameObject.SetActive(true);
            }
            //Instance.deathCamera.enabled = false;
        }
        else if(!playerAlive)
        {
            //Instance.deathCamera.enabled = true;
            lifeText.text = "x " + lives.ToString();
        }
    }

    public void PlacePlayer()
    {
        audioManager.AudioRespawnPlayer();
        playerInstance.GetComponent<PlayerH>().SetMaxHP();
        playerInstance.transform.position = playerSpawnLocation.position;
        playerInstance.transform.rotation = playerSpawnLocation.rotation;
        playerInstance.SetActive(true);
        playerInstance.GetComponent<Animator>().SetBool(Animator.StringToHash("dead"),false);
        //playerInstance.GetComponent<CapsuleCollider>().enabled = true;
        playerAlive = true;
    }

    void InstantiatePlayer()
    {
        playerInstance = Instantiate(playerPrefab, playerSpawnLocation.position, Quaternion.identity);
        playerInstanceTransform = playerInstance.transform;
        playerWeapon = playerInstance.GetComponent<PlayerWeapon>();
        playerAlive = true;
    }

    public static GameObject GetPlayerInstance()
    {
        return playerInstance;
    }

    public void Unpause()
    {
        paused = false;
        Instance.pausePanel.SetActive(false);
        Time.timeScale = 1f;
    }

    public void UpdateUI()
    {
        if (playerInstance != null) { 

            if (playerAlive)
            {
                if(playerWeapon.GetAiming())
                {
                    crosshair.gameObject.SetActive(true);
                }
                else if(!playerWeapon.GetAiming())
                {
                    crosshair.gameObject.SetActive(false);
                }
                goldMush = playerInstance.GetComponent<PlayerInventory>().GetGolden();
                if (goldMush)
                {
                    Instance.goldenM.color = new Color(goldenM.color.r, goldenM.color.g, goldenM.color.b, 1f);
                }
                else
                {
                    Instance.goldenM.color = new Color(goldenM.color.r, goldenM.color.g, goldenM.color.b, 0.35f);
                }
                silverMush = playerInstance.GetComponent<PlayerInventory>().GetSilver();
                if (silverMush)
                {
                    Instance.silverM.color = new Color(silverM.color.r, silverM.color.g, silverM.color.b, 1f);
                }
                else
                {
                    Instance.silverM.color = new Color(silverM.color.r, silverM.color.g, silverM.color.b, 0.35f);
                }
                playerCurrentHealth = playerInstance.GetComponent<PlayerH>().GetCurrentHealth();
                Instance.healthBar.fillAmount = playerCurrentHealth / 100f;
                Instance.ammoAmount.text = playerInstance.GetComponent<PlayerWeapon>().GetAmmunition().ToString();
            }
        }

        else
        {
            Instance.healthBar.fillAmount = 0f;
        }
        lifeText.text = "x " + lives.ToString();
    }
    
    public void SwitchControlPanel()
    {
        gameControlPanel = !gameControlPanel;
    }

    public void PlayerDeath()
    {
        playerAlive = false;
        lives--;
        if(lives > 0)
        {
            playerInstance.SetActive(false);
            //playerInstance.GetComponent<CapsuleCollider>().enabled = true;

            Invoke("PlacePlayer", 3.0f);
        }
        else if(lives <=0)
        {
            gameOver = true;
            Time.timeScale = 0f;
            gameOverPanel.gameObject.SetActive(true);
            playerInstance.SetActive(false);
        }      
    }
}
// private GameState gameState = GameState.Playing;

//     private enum GameState { Playing, Paused, GameOver, Inventory, Won }

//     private void Awake()
//     {
//         InitializeGame();
//     }

//     private void InitializeGame()
//     {
//         //Cursor.lockState = CursorLockMode.Locked;
//         Time.timeScale = 1f;

//         pausePanel.SetActive(false);
//         gameOverPanel.SetActive(false);
//         winPanel.SetActive(false);
//         controlPanel.SetActive(false);
//         inventoryPanel.SetActive(false);

//         SpawnPlayer();
//         UpdateUI();
//     }

//     private void Update()
//     {
//         HandleInput();
//         HandleGameStates();
//     }

//     private void HandleInput()
//     {
//         if (Input.GetKeyDown(KeyCode.Escape) && gameState != GameState.GameOver)
//         {
//             if (gameState != GameState.Inventory)
//             {
//                 gameState = gameState == GameState.Playing ? GameState.Paused : GameState.Playing;
//             }
//         }

//         if (Input.GetKeyDown(KeyCode.Tab))
//         {
//             ToggleInventory();
//         }
//     }

//     private void HandleGameStates()
//     {
//         switch (gameState)
//         {
//             case GameState.Playing:
//                 // Handle logic for playing state.
//                 break;
//             case GameState.Paused:
//                 // Handle logic for paused state.
//                 break;
//             case GameState.GameOver:
//                 // Handle logic for game over state.
//                 break;
//             case GameState.Inventory:
//                 // Handle logic for inventory state.
//                 break;
//             case GameState.Won:
//                 // Handle logic for won state.
//                 break;
//         }
//     }

//     private void ToggleInventory()
//     {
//         inventoryState = !inventoryState;
//         inventoryPanel.SetActive(inventoryState);
//     }

//     private void UpdateUI()
//     {
//         if (playerInstance != null && gameState == GameState.Playing)
//         {
//             playerWeapon = playerInstance.GetComponent<PlayerWeapon>();
//             crosshair.gameObject.SetActive(playerWeapon.GetAiming());

//             bool goldMush = playerInstance.GetComponent<PlayerInventory>().GetGolden();
//             goldenM.color = new Color(goldenM.color.r, goldenM.color.g, goldenM.color.b, goldMush ? 1f : 0.35f);

//             bool silverMush = playerInstance.GetComponent<PlayerInventory>().GetSilver();
//             silverM.color = new Color(silverM.color.r, silverM.color.g, silverM.color.b, silverMush ? 1f : 0.35f);

//             float playerCurrentHealth = playerInstance.GetComponent<PlayerH>().GetCurrentHealth();
//             healthBar.fillAmount = playerCurrentHealth / 100f;

//             ammoAmount.text = playerWeapon.GetAmmunition().ToString();
//         }
//         else
//         {
//             healthBar.fillAmount = 0f;
//         }

//         lifeText.text = "x " + lives.ToString();
//     }

//     private void SpawnPlayer()
//     {
//         playerInstance = Instantiate(playerPrefab, playerSpawnLocation.position, Quaternion.identity);
//         playerWeapon = playerInstance.GetComponent<PlayerWeapon>();
//     }

//     public void PlayerDeath()
//     {
//         lives--;

//         if (lives > 0)
//         {
//             playerInstance.SetActive(false);
//             Invoke("RespawnPlayer", 3.0f);
//         }
//         else
//         {
//             gameState = GameState.GameOver;
//             Time.timeScale = 0f;
//             gameOverPanel.SetActive(true);
//             playerInstance.SetActive(false);
//         }
//     }

//     private void RespawnPlayer()
//     {
//         playerInstance.transform.position = playerSpawnLocation.position;
//         playerInstance.transform.rotation = playerSpawnLocation.rotation;
//         playerInstance.SetActive(true);
//         playerInstance.GetComponent<Animator>().SetBool(Animator.StringToHash("dead"), false);
//         gameState = GameState.Playing;
//     }
//}
