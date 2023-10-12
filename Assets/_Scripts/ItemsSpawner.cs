using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ItemsSpawner : MonoBehaviour
{
    [SerializeField] GameObject[] mushrooms;
    [SerializeField] GameObject[] collectables;
    [SerializeField] Transform goldenTransform;
    [SerializeField] Transform silverTransform;
    Vector3 randomPos;
    float maxItems = 5;
    // Start is called before the first frame update
    void Start()
    {
        for(int i = 0; i<maxItems; i++)
        {
            randomPos = new Vector3(Random.Range(-50f, -28f), 0f, Random.Range(34.5f, 58f));
            Instantiate(collectables[0], randomPos, Quaternion.identity);
        }
        for(int i = 0; i<maxItems; i++)
        {
            randomPos = new Vector3(Random.Range(-50f, -28f), 0f, Random.Range(34.5f, 58f));
            Instantiate(collectables[1], randomPos, Quaternion.identity);
        }
        for(int i = 0; i<maxItems; i++)
        {
            randomPos = new Vector3(Random.Range(-50f, -28f), 0f, Random.Range(34.5f, 58f));
            Instantiate(collectables[2], randomPos, Quaternion.identity);
        }
        Instantiate(mushrooms[0], goldenTransform.position , Quaternion.identity);
        Instantiate(mushrooms[1], silverTransform.position, Quaternion.identity);
    }
}
