using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class EnemyManager : MonoBehaviour
{
    [SerializeField] GameObject[] aiEnemies;
    Vector3 randomPosition;
    int maxEnemies = 3;
    int currentEnemies = 0;
    int maxSpawns = 4;
    PlayerInventory playerInventory;
    bool moveSpawn;
    Vector3 initialSpawnPosition;
    Vector3 finalSpawnPosition;
    public static EnemyManager enemyManager;
    //[SerializeField] GameObject[] templeGates;
    // Start is called before the first frame update
    private void Awake() {
        enemyManager = this;
    }
    private void Start()
    {
        moveSpawn = false;
    }
    void Update()
    {
        if (GameObject.FindGameObjectWithTag("Player") != null)
        {
            playerInventory = GameObject.FindGameObjectWithTag("Player").GetComponent<PlayerInventory>();
            moveSpawn = playerInventory.GetSilver();
        }
    }


    public void PlaceNewEnemy()
    {
        if(maxSpawns > 0)
        {
            if(currentEnemies < maxEnemies)
            {
                for (int i = currentEnemies; i < maxEnemies; i++)
                {
                    initialSpawnPosition = new Vector3(Random.Range(-27f, -21f), -20f, Random.Range(48f, 58f));
                    finalSpawnPosition = new Vector3(Random.Range(-51f, -48f), -14f, Random.Range(34f, 43f));
                    
                    randomPosition = moveSpawn ? finalSpawnPosition : initialSpawnPosition;
                    Instantiate(aiEnemies[Random.Range(0, 2)], randomPosition, Quaternion.identity);
                    

                    currentEnemies++;
                }
                maxSpawns--;
            }
        }

    }

    public void SubtractEnemy()
    {
        currentEnemies--;
    }
}
