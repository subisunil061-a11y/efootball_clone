using UnityEngine;

public class GoalkeeperAI : MonoBehaviour
{
    [Header("Movement Limits")]
    public float leftLimit = -3.5f;  // How far left the keeper can slide
    public float rightLimit = 3.5f;  // How far right the keeper can slide
    
    [Header("Speed Settings")]
    public float speed = 4f;         // How fast the keeper slides side-to-side
    
    private bool movingRight = true;

    void Update()
    {
        // 1. Move the keeper left or right based on their current direction
        if (movingRight)
        {
            transform.Translate(Vector3.right * speed * Time.deltaTime);
            
            // If they hit the right post limit, turn around
            if (transform.position.x >= rightLimit)
            {
                movingRight = false;
            }
        }
        else
        {
            transform.Translate(Vector3.left * speed * Time.deltaTime);
            
            // If they hit the left post limit, turn around
            if (transform.position.x <= leftLimit)
            {
                movingRight = true;
            }
        }
    }
}
