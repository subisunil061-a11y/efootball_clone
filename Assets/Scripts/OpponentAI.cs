using UnityEngine;

public class OpponentAI : MonoBehaviour
{
    private Rigidbody rb;
    private Transform ballTransform;

    [Header("Defending Settings")]
    public float chaseSpeed = 5.5f;     // Speed of the enemy defender (slightly slower than your walk speed so it's fair!)

    void Start()
    {
        rb = GetComponent<Rigidbody>();

        // Automatically locate the ball in the stadium
        GameObject ballObj = GameObject.Find("Ball");
        if (ballObj != null)
        {
            ballTransform = ballObj.transform;
        }
    }

    void FixedUpdate()
    {
        if (ballTransform == null) return;

        // Calculate the vector pointing straight from the defender to the soccer ball
        Vector3 directionToBall = (ballTransform.position - transform.position);
        directionToBall.y = 0f; // Keep the defender locked flat to the grass pitch

        // If the defender isn't right on top of the ball, sprint toward it!
        if (directionToBall.magnitude > 0.5f)
        {
            Vector3 moveVelocity = directionToBall.normalized * chaseSpeed;
            // Apply the velocity while preserving gravity down on the Y axis
            rb.linearVelocity = new Vector3(moveVelocity.x, rb.linearVelocity.y, moveVelocity.z);
            
            // Make the capsule smoothly rotate to look at the ball it's chasing
            if (moveVelocity != Vector3.zero)
            {
                transform.forward = moveVelocity.normalized;
            }
        }
        else
        {
            // Stop moving if close enough
            rb.linearVelocity = new Vector3(0f, rb.linearVelocity.y, 0f);
        }
    }
}
