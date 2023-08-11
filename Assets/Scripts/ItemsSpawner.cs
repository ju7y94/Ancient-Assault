using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ItemsSpawner : MonoBehaviour
{
    [SerializeField] GameObject[] mushrooms;
    [SerializeField] GameObject[] collectables;
    Vector3 goldenPosition;
    Vector3 silverPosition;
    Vector3 randomPos;
    float maxItems = 5;
    // Start is called before the first frame update
    void Start()
    {
        for(int i = 0; i<maxItems; i++)
        {
            randomPos = new Vector3(Random.Range(-50f, -28f), -5f, Random.Range(34.5f, 58f));
            Instantiate(collectables[0], randomPos, Quaternion.identity);
        }
        for(int i = 0; i<maxItems; i++)
        {
            randomPos = new Vector3(Random.Range(-50f, -28f), -5f, Random.Range(34.5f, 58f));
            Instantiate(collectables[1], randomPos, Quaternion.identity);
        }
        for(int i = 0; i<maxItems; i++)
        {
            randomPos = new Vector3(Random.Range(-50f, -28f), -5f, Random.Range(34.5f, 58f));
            Instantiate(collectables[2], randomPos, Quaternion.identity);
        }
        silverPosition = new Vector3(-28.0f, -16.6f, 54.8f);
        goldenPosition = new Vector3(-38.3f, -12.4f, 40.7f);
        Instantiate(mushrooms[0], goldenPosition, Quaternion.identity);
        Instantiate(mushrooms[1], silverPosition, Quaternion.identity);
    }

    // Update is called once per frame
    void Update()
    {
        
    }
}
