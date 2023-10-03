using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Human : MonoBehaviour
{
    protected float currentHealthPoints;
    [SerializeField] protected float maxHealth;
    

    public virtual void Damage(float damage)
    {
        currentHealthPoints -= damage;
    }
}

