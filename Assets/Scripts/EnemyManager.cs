using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class EnemyManager : MonoBehaviour
{
    [SerializeField] GameObject[] aiEnemies;
    Vector3 randomPosition;
    int maxEnemies = 5;
    int currentEnemies = 0;
    PlayerInventory playerInventory;
    bool moveSpawn;
    //[SerializeField] GameObject[] templeGates;
    // Start is called before the first frame update
    private void Start()
    {
        moveSpawn = false;
        /*foreach (GameObject gate in templeGates)
        {
            gate.SetActive(true);
        }*/

    }
    void Update()
    {
        if (GameObject.FindGameObjectWithTag("Player") != null)
        {
            playerInventory = GameObject.FindGameObjectWithTag("Player").GetComponent<PlayerInventory>();
            moveSpawn = playerInventory.GetSilver();
        }

        /*if (moveSpawn)
        {
            foreach (GameObject gate in templeGates)
            {
                gate.SetActive(false);
            }
        }*/
    }


    public void PlaceNewEnemy()
    {
        if(currentEnemies == 0)
        {
            for (int i = currentEnemies; i < maxEnemies; i++)
            {
                randomPosition = new Vector3(Random.Range(-35f, -33f), -16.75f, Random.Range(49f, 52f));
                if (moveSpawn)
                {
                    randomPosition = new Vector3(Random.Range(-51f, -48f), -14f, Random.Range(34f, 43f));
                }
                Instantiate(aiEnemies[Random.Range(0, 2)], randomPosition, Quaternion.identity);
                currentEnemies++;
            }
        }
        
    }


    public void SubtractEnemy()
    {
        currentEnemies--;
    }

}
