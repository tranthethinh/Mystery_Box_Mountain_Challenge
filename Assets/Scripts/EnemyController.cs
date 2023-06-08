using UnityEngine;

public class EnemyController : MonoBehaviour
{
    public Transform player; // Reference to the player's transform
    private float moveSpeed = 3f; // Speed at which the enemy moves towards the player
    private float timer = 0f;
    private float randomDirectionInterval = 3f;
    private Vector3 randomDirection;
    private Vector3 currentDirection;
    private bool isGrounded = false;
    private float jumpForce = 2f;
    private Rigidbody rb;

    private void Start()
    {
        // Find the player GameObject using a specific tag
        player = GameObject.FindGameObjectWithTag("Player").transform;
        rb = GetComponent<Rigidbody>();
    }
    void Update()
    {
        if (player != null)
        {
            Vector3 direction = player.position - transform.position;
            float distance = direction.magnitude;

            if (distance < 22f)
            {
                direction.Normalize();
                currentDirection = direction;
                Quaternion lookRotation = Quaternion.LookRotation(currentDirection);
                transform.rotation = lookRotation;
                transform.position += currentDirection * moveSpeed * Time.deltaTime;
            }
            else
            {
                timer += Time.deltaTime;

                // Check if the timer exceeds the interval
                if (timer >= randomDirectionInterval)
                {
                    // Generate a new random direction
                    randomDirection = new Vector3(Random.Range(-1f, 1f), 0f, Random.Range(-1f, 1f));
                    randomDirection.Normalize();

                    // Reset the timer
                    timer = 0f;
                }
                if (randomDirection != Vector3.zero)
                {
                    // Calculate the rotation needed to face the random direction
                    Quaternion lookRotation = Quaternion.LookRotation(randomDirection);

                    // Apply the rotation to the enemy object
                    transform.rotation = lookRotation;
                }

                // Smoothly transition between the current direction and the new random direction
                currentDirection = Vector3.Lerp(currentDirection, randomDirection, Time.deltaTime);

                // Move the enemy in the current direction
                transform.position += currentDirection * moveSpeed * Time.deltaTime;
            }
        }
        // Check if the enemy is grounded
        if (isGrounded)
        {
            // Apply an upward force to simulate the jump
            rb.AddForce(Vector3.up * jumpForce, ForceMode.Impulse);
            isGrounded = false;
        }
        // Check if the enemy is below the threshold position or away from the player
        float distanceToPlayer = Vector3.Distance(transform.position, player.transform.position);
        if (transform.position.y < -1f||distanceToPlayer>200)
        {
            // Destroy the enemy object
            //Destroy(gameObject);
            Vector3 spawnPosition = Random.insideUnitSphere * 225;
            spawnPosition.y = 0f; 

            // Offset the spawn position relative to the player
            spawnPosition += player.position;
            transform.position = spawnPosition;
        }
    }
    void OnCollisionEnter(Collision collision)
    {
        // Check if the enemy collided with the ground
        if (collision.gameObject.CompareTag("Ground"))
        {
            isGrounded = true;
        }
    }
}

