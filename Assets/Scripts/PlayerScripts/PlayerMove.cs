using UnityEngine;

[RequireComponent(typeof(Rigidbody2D))]
public class Move : MonoBehaviour
{     
    [SerializeField] private float moveSpeed = 5f;
    private Rigidbody2D rigidBody;
    private Vector2 input;

    void Awake()
    {
        rigidBody = GetComponent<Rigidbody2D>();
    }

    // Update is called once per frame
    void Update()
    {
        HandleInputs();
    }

    void FixedUpdate() {
        rigidBody.linearVelocity = input * moveSpeed;
    }

    private void HandleInputs() {
        //Move based off arrow and WASD keys
        input.x = Input.GetAxisRaw("Horizontal");
        input.y = Input.GetAxisRaw("Vertical");

        input.Normalize();
    }
}
 