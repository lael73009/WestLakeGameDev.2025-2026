using System.Runtime.CompilerServices;
using UnityEngine;

public class EnemyMove : MonoBehaviour
{
    [SerializeField] private GameObject _player;
    [SerializeField] private Rigidbody2D _rigidbody;
    private Vector2 _playerLocation;

    public float _speed;
    public float _sightRange;

    // TO DO: MOVE DAMAGE SCRIPT SOMEWHRE ELSE
    public int _damage;
    private Health _playerHealth;

    void Start()
    {
        _player = GameObject.Find("Player");
        _rigidbody = GetComponent<Rigidbody2D>();
    }

    // Update is called once per frame
    void Update()
    {
        Move();
    }

    private void Move()
    {
        //Gizmos.DrawSphere(transform.position, sightRange);
        if (_player == null) {
            Debug.LogError($"EnemyMove on {gameObject.name} has a missing or destroyed player reference.", this);
            return;
        }

        if (_rigidbody == null)
        {
            Debug.Log("Grrr...");
        }

        _playerLocation = _player.transform.position;
        Vector2 direction = (_playerLocation - (Vector2)(transform.position)).normalized;

        // Check if player is in range... probably not the best way to approach
        if (Mathf.Sqrt(Mathf.Pow((_playerLocation - (Vector2)(transform.position)).x, 2) + Mathf.Pow((_playerLocation - (Vector2)(transform.position)).y, 2)) <= _sightRange)
        {
            // Find movement vector to approach player
            float angle = Mathf.Atan2(direction.y, direction.x);
            Vector2 move = new Vector2(Mathf.Cos(angle), Mathf.Sin(angle));
 
            // Move
            _rigidbody.linearVelocity = move*_speed;
        }
        else
        {
            // Slow down player is too far away... needs adjustment for gameplay reasons
            _rigidbody.linearVelocity = _rigidbody.linearVelocity * new Vector2((float)(0.6), (float)(0.6));
        }
    }

    // Move this somehwere else later
    private void OnCollisionEnter2D(Collision2D collision)
    {
        // Only damage the player
        if(collision.gameObject.tag == "Player")
        {
            // Find player health only if it hasn't been found
            if (_playerHealth == null) 
            {
                _playerHealth = collision.gameObject.GetComponent<Health>();
            }

            // Damage
            _playerHealth.TakeDamage(_damage);
        }
    } 
}