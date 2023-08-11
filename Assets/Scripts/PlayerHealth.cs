using System.Collections;
using System.Collections.Generic;
using UnityEngine;

enum Poison { Poisoned, Cured };

public class PlayerHealth : MonoBehaviour
{
    [SerializeField] float currentHealthPoints;
    [SerializeField] float maxHealth = 100f;
    bool poisoned = false;
    private float poisonTimer = 0f;
    [SerializeField] float poisonDuration = 10f;
    Poison poison;
    AudioManager audioManager;
    bool blocking;
    [SerializeField] GameObject[] lastMushroom;
    PlayerInventory playerInventory;
    // Start is called before the first frame update
    void Start()
    {
        audioManager = GameObject.FindGameObjectWithTag("AudioManager").GetComponent<AudioManager>();
        currentHealthPoints = maxHealth;
        poison = Poison.Cured;
        playerInventory = GetComponent<PlayerInventory>();
    }

    private void OnCollisionEnter(Collision other)
    {
        if (other.gameObject.tag == "Cure")
        {
            if (poisoned)
            {
                poison = Poison.Cured;
                currentHealthPoints += 5f;
                poisonTimer = 0f;
                audioManager.AudioWeaponPickUp();
                Destroy(other.gameObject);
            }
            else if (!poisoned)
            {
                if(currentHealthPoints <= 80f)
                {
                    currentHealthPoints += 20f;
                }
                else
                {
                    currentHealthPoints += maxHealth - currentHealthPoints;
                }
                
                audioManager.AudioWeaponPickUp();
                Destroy(other.gameObject);
            }
        }

        if (other.gameObject.tag == "PoisonedEnemy")
        {
            poison = Poison.Poisoned;
            poisonTimer = 0f;
        }
    }
    
    // Update is called once per frame
    void Update()
    {
        if (poison == Poison.Poisoned)
        {
            currentHealthPoints -= 1.5f * Time.deltaTime;
            if (currentHealthPoints <= 0)
            {
                //Destroy(gameObject);
                TheGameManager.PlayerDeath();
            }
            poisoned = true;
            poisonTimer += Time.deltaTime;
            if (poisonTimer >= poisonDuration)
            {
                poison = Poison.Cured;
                poisonTimer = 0f;
            }
        }

        else if (poison == Poison.Cured)
        {
            poisoned = false;
        }
    }

    public void Damage(float damage)
    {
        audioManager.AudioPlayerHurt();
        currentHealthPoints -= damage;
        if (currentHealthPoints <= 0)
        {
            gameObject.tag = "Default";
            if(playerInventory.GetGolden())
            {
                Instantiate(lastMushroom[0], transform.position, transform.rotation);
            }
            else if(playerInventory.GetSilver())
            {
                Instantiate(lastMushroom[1], transform.position, transform.rotation);
            }
            
            TheGameManager.PlayerDeath();
        }
    }
    public float GetCurrentHealth()
    {
        return currentHealthPoints;
    }

    public bool GetPoisoned()
    {
        return poisoned;
    }

    public void SetPoisoned()
    {
        poison = Poison.Poisoned;
    }
    public void Block()
    {
        blocking = !blocking;
    }
    public bool GetBlocking()
    {
        return blocking;
    }
}
