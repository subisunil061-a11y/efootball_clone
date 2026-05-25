using UnityEngine;
using TMPro;
using System.Collections;

public class GoalTrigger : MonoBehaviour
{
    public TextMeshProUGUI scoreText; // Links our UI display text
    private int score = 0;            // Keeps track of your total points
    private Vector3 ballStartPos;     // Remembers where the ball starts
    private GameObject ballObject;

    void Start()
    {
        // Find the ball in our scene automatically and save its starting position
        ballObject = GameObject.Find("Ball");
        if (ballObject != null)
        {
            ballStartPos = ballObject.transform.position;
        }
    }

    // This built-in Unity function activates the exact frame an object passes inside our wall
    void OnTriggerEnter(Collider other)
    {
        // Check if the object entering the goal is named "Ball"
        if (other.gameObject.name == "Ball")
        {
            score++; // Add a point!
            StartCoroutine(FlashGoalRoutine());
        }
    }

    IEnumerator FlashGoalRoutine()
    {
        // 1. Flash "GOAL!" full screen
        scoreText.text = "GOAL!!!";
        scoreText.color = Color.yellow;

        // 2. Freeze the ball momentum briefly so it stops rolling away
        Rigidbody ballRb = ballObject.GetComponent<Rigidbody>();
        if (ballRb != null)
        {
            ballRb.linearVelocity = Vector3.zero;
            ballRb.angularVelocity = Vector3.zero;
        }

        // 3. Wait 2 seconds for celebration animation effect
        yield return new WaitForSeconds(2f);

        // 4. Teleport the ball back to the center field spot
        ballObject.transform.position = ballStartPos;

        // 5. Restore normal score counting screen text layout
        scoreText.text = "Score: " + score;
        scoreText.color = Color.white;
    }
}
