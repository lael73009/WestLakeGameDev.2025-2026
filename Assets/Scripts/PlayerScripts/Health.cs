using System;
using UnityEngine;

public class Health : MonoBehaviour
{
    private int health;
    public int maxHealth;
    // Start is called once before the first execution of Update after the MonoBehaviour is created
    void Start()
    {
        //Reset health
        health = maxHealth;
    }

    // Update is called once per frame
    void Update()
    {
        
    }

    public void TakeDamage(int damage)
    {
        print("Damage Taken");
        //Lose health
        health -= damage;

        //Die... temporary for testing
        if (health <= 0)
        {
            Destroy(gameObject);
        }
    }
}