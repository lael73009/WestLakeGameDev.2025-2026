using UnityEngine;

[RequireComponent(typeof(Rigidbody2D))]
public class Move : MonoBehaviour
{     
    [SerializeField] private float _moveSpeed = 5f;
    private Rigidbody2D _rigidBody;
    private Vector2 _input;

    void Awake()
    {
        _rigidBody = GetComponent<Rigidbody2D>();
    }

    // Update is called once per frame
    void Update()
    {
        HandleInputs();
    }

    void FixedUpdate() {
        _rigidBody.linearVelocity = _input * _moveSpeed;
    }

    private void HandleInputs() {
        //Move based off arrow and WASD keys
        _input.x = Input.GetAxisRaw("Horizontal");
        _input.y = Input.GetAxisRaw("Vertical");

        _input.Normalize();
    }
}
 