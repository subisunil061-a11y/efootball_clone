using UnityEngine;
using UnityEngine.UI;

public class PlayerController : MonoBehaviour
{
    [Header("Movement Settings")]
    public float walkSpeed = 6f;
    public float sprintSpeed = 9f;
    private float currentSpeed;
    private Rigidbody rb;
    private Vector3 moveInput;

    [Header("Shooting & Power Bar")]
    public float minShotForce = 10f;
    public float maxShotForce = 35f;
    public float chargeSpeed = 1.5f;
    private float currentShotCharge = 0f;
    private bool isChargingShot = false;
    private Slider powerBarSlider;
    private Rigidbody ballRigidbody;

    [Header("Team Reference")]
    private TeamManager teamManager;

    // ⚽ NEW: Variable to hold our character's 3D animator component
    private Animator anim;

    void Start()
    {
        rb = GetComponent<Rigidbody>();
        currentSpeed = walkSpeed;
        teamManager = FindFirstObjectByType<TeamManager>();

        // ⚽ NEW: Grab the Animator from the child 3D model automatically at kickoff
        anim = GetComponentInChildren<Animator>();

        GameObject ballObj = GameObject.Find("Ball");
        if (ballObj != null)
        {
            ballRigidbody = ballObj.GetComponent<Rigidbody>();
        }

        // Automatically locate and connect to our UI Power Bar slider
        GameObject sliderObj = GameObject.Find("PowerBar");
        if (sliderObj != null)
        {
            powerBarSlider = sliderObj.GetComponent<Slider>();
            powerBarSlider.value = 0f; // Start empty
        }
    }

void Update()
{
    // 1. Handle Directional Inputs (Only define moveX and moveZ ONCE)
    float moveX = Input.GetAxisRaw("Horizontal");
    float moveZ = Input.GetAxisRaw("Vertical");
    
    // Assign directly to the global moveInput variable
    moveInput = new Vector3(moveX, 0f, moveZ).normalized;

    // 2. Sprinting Logic (Shift Key)
    if (Input.GetKey(KeyCode.LeftShift))
    {
        currentSpeed = sprintSpeed;
    }
    else
    {
        currentSpeed = walkSpeed;
    }

    // 3. Tell the Animator Blend Tree how fast we are moving
    if (anim != null)
    {
        // Use the moveInput we already calculated above—do NOT add "Vector3" in front of it here!
        anim.SetFloat("Blend", moveInput.magnitude);
    }

    // 4. Shooting Mechanics (E Key)
    HandleShooting();
}

    void FixedUpdate()
    {
        // Apply physics velocity based on movement direction and current speed
        rb.linearVelocity = new Vector3(moveInput.x * currentSpeed, rb.linearVelocity.y, moveInput.z * currentSpeed);

        // Rotate player model smoothly toward the direction they are traveling
        if (moveInput != Vector3.zero)
        {
            Quaternion targetRotation = Quaternion.LookRotation(moveInput);
            rb.rotation = Quaternion.Slerp(rb.rotation, targetRotation, 15f * Time.fixedDeltaTime);
        }
    }

    private void HandleShooting()
    {
        if (ballRigidbody == null || powerBarSlider == null) return;

        // Start Charging Shot
        if (Input.GetKeyDown(KeyCode.E))
        {
            isChargingShot = true;
            currentShotCharge = minShotForce;
            powerBarSlider.value = 0f;
        }

        // Holding Down Shot Charge
        if (Input.GetKey(KeyCode.E) && isChargingShot)
        {
            currentShotCharge += chargeSpeed * (maxShotForce - minShotForce) * Time.deltaTime;
            currentShotCharge = Mathf.Clamp(currentShotCharge, minShotForce, maxShotForce);

            // Update visual power slider UI (0 to 1 range)
            float chargePercentage = (currentShotCharge - minShotForce) / (maxShotForce - minShotForce);
            powerBarSlider.value = chargePercentage;
        }

        // Release Shot Key -> Release the Rocket Rocket!
        if (Input.GetKeyUp(KeyCode.E) && isChargingShot)
        {
            isChargingShot = false;
            powerBarSlider.value = 0f; // Clear bar instantly

            // Check if ball is close enough to shoot
            float distanceToBall = Vector3.Distance(transform.position, ballRigidbody.transform.position);
            if (distanceToBall <= 2.0f)
            {
                // Calculate shot trajectory vector (forward vector with upward angle)
                Vector3 shotDirection = transform.forward;
                shotDirection.y = 0.25f; // Gives physics lift into top corners
                shotDirection = shotDirection.normalized;

                // Fire ball using linear velocity rocket physics
                ballRigidbody.linearVelocity = shotDirection * currentShotCharge;
            }
        }
    }
}
