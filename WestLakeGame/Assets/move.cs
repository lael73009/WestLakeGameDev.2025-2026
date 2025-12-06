using UnityEngine;
using System;
public class move : MonoBehaviour
{     
    public int moveSpeed = 5;
    public Rigidbody2D rigidbody;
    private Vector2 input;
 
    // Start is called once before the first execution of Update after the MonoBehaviour is created
    void Start()
    {
    }

    // Update is called once per frame
    void Update()
    {
        // Key inputs
        input.x = Input.GetAxisRaw("Horizontal");
        input.y = Input.GetAxisRaw("Vertical");

        //input.Normalize();
    }

    void FixedUpdate() {
        // Apply movement
        if (rigidbody != null)
        {
            rigidbody.linearVelocity = input * moveSpeed;
        }
    }
}
 