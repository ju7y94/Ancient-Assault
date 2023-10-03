using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerH : Human
{
    protected bool blocking;
    [SerializeField] protected GameObject[] lastMushroom;
    protected PlayerInventory playerInventory;

    void Awake() 
    {
        playerInventory = GetComponent<PlayerInventory>();
        currentHealthPoints = maxHealth;
        print("PLAYER HP: " + currentHealthPoints);
    
    }

    private void Start() {
        print("PlayerH does something on Start");
    }

    public override void Damage(float damage)
    {
        base.Damage(damage);
        
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

    public void Block()
    {
        blocking = !blocking;
    }
    public bool GetBlocking()
    {
        return blocking;
    }
}
