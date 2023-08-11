using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerInventory : MonoBehaviour
{
    bool goldenMushroom, silverMushroom, hasBoth;
    AudioManager audioManager;
    // Start is called before the first frame update
    void Start()
    {
        goldenMushroom = false;
        silverMushroom = false;
        audioManager = GameObject.FindGameObjectWithTag("AudioManager").GetComponent<AudioManager>();
    }

    // Update is called once per frame
    void Update()
    {
        if(goldenMushroom && silverMushroom)
        {
            hasBoth = true;
        }
        else
        {
            hasBoth = false;
        }
    }
    public void SetGoldenMushroom(bool mushroomBool)
    {
        goldenMushroom = mushroomBool;
        audioManager.AudioHealthBonus();
    }

    public void SetSilverMushroom(bool mushroomBool)
    {
        silverMushroom = mushroomBool;
        audioManager.AudioHealthBonus();
    }
    public bool GetHasBoth()
    {
        return hasBoth;
    }
    public bool GetGolden()
    {
        return goldenMushroom;
    }
    public bool GetSilver()
    {
        return silverMushroom;
    }
}
