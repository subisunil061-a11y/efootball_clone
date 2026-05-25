using UnityEngine;
using TMPro; 
using UnityEngine.SceneManagement; // NEW: Required to reload scenes dynamically

public class MatchManager : MonoBehaviour
{
    [Header("Match Timer Settings")]
    public float matchDurationInSeconds = 30f; 
    private float timeRemaining;
    private bool isMatchOver = false;

    [Header("UI Text Components")]
    public TextMeshProUGUI timerText;
    public GameObject fullTimeTextObject;

    void Start()
    {
        timeRemaining = matchDurationInSeconds;
        
        if (fullTimeTextObject != null)
        {
            fullTimeTextObject.SetActive(false);
        }
    }

    void Update()
    {
        // If the match is over, constantly check if the player presses 'R' to restart!
        if (isMatchOver)
        {
            if (Input.GetKeyDown(KeyCode.R))
            {
                RestartCurrentMatch();
            }
            return;
        }

        if (timeRemaining > 0)
        {
            timeRemaining -= Time.deltaTime;
            UpdateTimerDisplay();
        }
        else
        {
            timeRemaining = 0;
            UpdateTimerDisplay();
            EndMatch();
        }
    }

    void UpdateTimerDisplay()
    {
        if (timerText == null) return;

        float matchPercentage = 1f - (timeRemaining / matchDurationInSeconds);
        int virtualMinutes = Mathf.FloorToInt(matchPercentage * 90f);

        timerText.text = virtualMinutes.ToString() + ":00";
    }

    void EndMatch()
    {
        isMatchOver = true;

        if (fullTimeTextObject != null)
        {
            // We update the full time text to remind the player how to play again
            TextMeshProUGUI ftText = fullTimeTextObject.GetComponent<TextMeshProUGUI>();
            if (ftText != null)
            {
                ftText.text = "FULL TIME\n<size=24>Press 'R' to Rematch</size>";
            }
            fullTimeTextObject.SetActive(true);
        }

        Rigidbody[] allBodies = FindObjectsByType<Rigidbody>(FindObjectsSortMode.None);
        foreach (Rigidbody rb in allBodies)
        {
            rb.linearVelocity = Vector3.zero;
            rb.angularVelocity = Vector3.zero;
            rb.isKinematic = true; 
        }

        PlayerController[] players = FindObjectsByType<PlayerController>(FindObjectsSortMode.None);
        foreach (PlayerController p in players)
        {
            p.enabled = false;
        }

        OpponentAI opponent = FindFirstObjectByType<OpponentAI>();
        if (opponent != null) opponent.enabled = false;
    }

    // NEW FUNCTION: Reloads the active arena instantly
    void RestartCurrentMatch()
    {
        // Get the name of your currently open scene and reload it fresh
        string activeSceneName = SceneManager.GetActiveScene().name;
        SceneManager.LoadScene(activeSceneName);
    }
}
