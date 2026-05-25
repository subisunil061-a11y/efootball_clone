using UnityEngine;

public class TeamManager : MonoBehaviour
{
    [Header("Team Squad")]
    public PlayerController[] teammates; 
    
    [Header("Target Ball")]
    public Transform ball;              
    
    private int activeIndex = 0;        

    void Start()
    {
        SelectPlayer(0);
    }

    void Update()
    {
        if (Input.GetKeyDown(KeyCode.Space))
        {
            SwitchToClosestPlayer();
        }
    }

    void SwitchToClosestPlayer()
    {
        int closestIndex = activeIndex;
        float shortestDistance = Mathf.Infinity;

        for (int i = 0; i < teammates.Length; i++)
        {
            float distanceToBall = Vector3.Distance(teammates[i].transform.position, ball.position);
            if (distanceToBall < shortestDistance)
            {
                shortestDistance = distanceToBall;
                closestIndex = i;
            }
        }

        SelectPlayer(closestIndex);
    }

    void SelectPlayer(int index)
    {
        activeIndex = index;

        for (int i = 0; i < teammates.Length; i++)
        {
            if (i == index)
            {
                teammates[i].enabled = true; 
                teammates[i].GetComponent<Renderer>().material.color = Color.white; 
            }
            else
            {
                teammates[i].enabled = false; 
                teammates[i].GetComponent<Renderer>().material.color = Color.yellow; 
            }
        }
    }

    // NEW FUNCTION: Returns the player who is NOT currently active so we can pass to them
    public Transform GetTeammateFor(PlayerController activePlayer)
    {
        for (int i = 0; i < teammates.Length; i++)
        {
            if (teammates[i] != activePlayer)
            {
                return teammates[i].transform;
            }
        }
        return null;
    }
}
