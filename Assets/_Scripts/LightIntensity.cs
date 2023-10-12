using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class LightIntensity : MonoBehaviour
{
    Light directLight;
    float minIntensity = 0.65f;
    float frequency = 0.01f;
    float maxIntensity = 0.35f;


    // Start is called before the first frame update
    void Start()
    {
        directLight = GetComponent<Light>();
        
    }

    // Update is called once per frame
    void Update()
    {
        directLight.intensity = minIntensity + Mathf.Sin(Time.timeSinceLevelLoad * frequency) * maxIntensity;
    }
}
