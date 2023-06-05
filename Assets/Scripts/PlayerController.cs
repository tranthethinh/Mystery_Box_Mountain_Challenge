using UnityEngine;

public class PlayerController : MonoBehaviour
{
    private Animator animator;


    public float moveSpeed = 48f; // speed of the player's movement
    public float maxSpeed = 48f; // maximum speed of the player
    private float jumpForce = 500f; // force of the player's jump
    private Rigidbody rb;
    [SerializeField]private bool isGrounded;
    public Transform cameraTransform;

    private float turnSmoothTime = 0.1f;
    private float turnSmoothVelocity;
    void Start()
    {
        rb = GetComponent<Rigidbody>();
        animator = GetComponentInChildren<Animator>();

    }

    void FixedUpdate()
    {
        // Get the player's input
        float horizontalInput = Input.GetAxis("Horizontal");
        float verticalInput = Input.GetAxis("Vertical");
        Vector3 inputVector = new Vector3(horizontalInput, 0, verticalInput);
        if (inputVector.magnitude > 0)
        {
            // Move the player

            float targetAngle = Mathf.Atan2(inputVector.x, inputVector.z) * Mathf.Rad2Deg + cameraTransform.eulerAngles.y;
            float angle = Mathf.SmoothDampAngle(transform.eulerAngles.y, targetAngle, ref turnSmoothVelocity, turnSmoothTime);
            transform.rotation = Quaternion.Euler(0f, angle, 0f);
            

            Vector3 moveDir = Quaternion.Euler(0f, targetAngle, 0f) * Vector3.forward;
            rb.AddForce(moveDir * moveSpeed);
            // Limit the player's maximum speed
            if (rb.velocity.magnitude > maxSpeed)
            {
                rb.velocity = rb.velocity.normalized * maxSpeed;
            }

            
        }
        animator.SetBool("IsRunning", rb.velocity.magnitude >0);

        //find nearest S_Enemy or Enemy to look at it if it inside searchDistance
        float searchDistance = 18f;
        GameObject nearestEnemy = FindNearestEnemyWithTag("Enemy", searchDistance);
        GameObject nearestS_Enemy = FindNearestEnemyWithTag("S_Enemy", searchDistance);
        GameObject targetEnemy = (nearestS_Enemy != null) ? nearestS_Enemy : nearestEnemy;

        if (targetEnemy != null)
        {
            Vector3 direction = targetEnemy.transform.position - transform.position;
            direction.Normalize();
            if (Mathf.Abs(targetEnemy.transform.position.y - transform.position.y) < 2)
            {
                transform.rotation = Quaternion.LookRotation(direction);
            }
        }
        // Check if the player is grounded
        //Vector3 rayDirection = (Vector3.down + Vector3.right).normalized;

        //isGrounded = Physics.Raycast(transform.position, rayDirection, out RaycastHit hit, 2f);

        // Jump if the player is grounded and the jump button is pressed
        if (Input.GetKeyDown(KeyCode.Space) && isGrounded)
        {
            rb.AddForce(Vector3.up * jumpForce);
            isGrounded = false;
        }
        if (transform.position.y < -1)
        {
            transform.position = new Vector3(15, 10, 10);
        }
    }
    private GameObject FindNearestEnemyWithTag(string tag, float maxDistance)
    {
        GameObject[] enemies = GameObject.FindGameObjectsWithTag(tag);

        GameObject nearestEnemy = null;
        float shortestDistance = Mathf.Infinity;

        foreach (GameObject enemy in enemies)
        {
            float distance = Vector3.Distance(transform.position, enemy.transform.position);
            if (distance < maxDistance && distance < shortestDistance)
            {
                shortestDistance = distance;
                nearestEnemy = enemy;
            }
        }

        return nearestEnemy;
    }
    private void OnCollisionEnter(Collision collision)
    {
        if (collision.gameObject.CompareTag("Ground")){
            isGrounded = true;
        }
    }
}