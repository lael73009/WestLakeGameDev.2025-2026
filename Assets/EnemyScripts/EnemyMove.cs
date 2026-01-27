using System.Runtime.CompilerServices;
using UnityEngine;

public class EnemyMove : MonoBehaviour
{
    public GameObject player;
    public Rigidbody2D rigidbody;
    private Vector2 playerLocation;

    public float speed;
    public float sightRange;

    //Move this somewhere else later
    public int damage;
    private Health playerHealth;

    // Start is called once before the first execution of Update after the MonoBehaviour is created
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        Move();
    }

    private void Move()
    {
            //Gizmos.DrawSphere(transform.position, sightRange);
        playerLocation = player.transform.position;
        Vector2 direction = (playerLocation - (Vector2)(transform.position)).normalized;

        //Check if player is in range... probably not the best way to approach
        if (Mathf.Sqrt(Mathf.Pow((playerLocation - (Vector2)(transform.position)).x, 2) + Mathf.Pow((playerLocation - (Vector2)(transform.position)).y, 2)) <= sightRange)
        {
            //Find movement vector to approach player
            float angle = Mathf.Atan2(direction.y, direction.x);
            Vector2 move = new Vector2(Mathf.Cos(angle), Mathf.Sin(angle));
 
            //Move
            rigidbody.linearVelocity = move*speed;
        }
        else
        {
            //Slow down player is too far away... needs adjustment for gameplay reasons
            rigidbody.linearVelocity = rigidbody.linearVelocity * new Vector2((float)(0.6), (float)(0.6));
        }
    }

    //Move this somehwere else later
    private void OnCollisionEnter2D(Collision2D collision)
    {
        //Only damage the player
        if(collision.gameObject.tag == "Player")
        {
            //Find player health only if it hasn't been found
            if (playerHealth == null) 
            {
                playerHealth = collision.gameObject.GetComponent<Health>();
            }

            //Damage
            playerHealth.TakeDamage(damage);
        }
    } 
}