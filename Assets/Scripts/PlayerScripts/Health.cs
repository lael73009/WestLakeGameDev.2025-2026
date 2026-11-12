using System;
using UnityEngine;

public class Health : MonoBehaviour
{
    private int _health;
    public int _maxHealth;
    // Start is called once before the first execution of Update after the MonoBehaviour is created
    void Start()
    {
        // Reset health
        _health = _maxHealth;
    }

    public void TakeDamage(int damage)
    {
        // Lose health
        _health -= damage;

        // TO DO: MAKE DEATH RESET
        // Die
        if (_health <= 0)
        {
            Destroy(gameObject);
        }
    }
}