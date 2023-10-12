using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerH : Human
{
    protected bool blocking;
    [SerializeField] protected GameObject[] lastMushroom;
    protected PlayerInventory playerInventory;
    protected AudioManager audioManager;
    private int animIDDead;
    private Animator animator;
    void Awake() 
    {
        audioManager = GameObject.FindGameObjectWithTag("AudioManager").GetComponent<AudioManager>();
        playerInventory = GetComponent<PlayerInventory>();
        SetMaxHP();
        animIDDead = Animator.StringToHash("dead");
        animator = GetComponent<Animator>();
    }

    public override void Damage(float damage)
    {
        base.Damage(damage);
        audioManager.AudioPlayerHurt();
        if (currentHealthPoints <= 0)
        {
            if(playerInventory.GetGolden())
            {
                Instantiate(lastMushroom[0], transform.position, transform.rotation);
            }
            else if(playerInventory.GetSilver())
            {
                Instantiate(lastMushroom[1], transform.position, transform.rotation);
            }
            
            animator.SetBool(animIDDead, true);
            TheGameManager.Instance.PlayerDeath();
            
        }
    }

    public float GetCurrentHealth()
    {
        return currentHealthPoints;
    }

    public void SetMaxHP()
    {
        currentHealthPoints = maxHealth;
    }

    public void IncreaseHealth(int value)
    {
        if (value <= maxHealth-currentHealthPoints)
        currentHealthPoints += value;
        else
        currentHealthPoints = maxHealth;
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
