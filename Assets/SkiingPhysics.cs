using UnityEngine;

public class SkiingPhysics : MonoBehaviour
{
    public Rigidbody rb;
    public float forwardSpeed = 10f;  // Speed of the skier
    public float turnSpeed = 5f;      // Turning speed of the skier
    public float slopeFactor = 0.5f; // How the slope affects skiing speed
    public float friction = 0.05f;   // Friction applied on the snow surface

    private Vector3 movementInput;

    // Start is called before the first frame update
    void Start()
    {
        rb = GetComponent<Rigidbody>();
    }

    // Update is called once per frame
    void Update()
    {
        HandleInput();
        ApplySkiingForces();
    }

    void HandleInput()
    {
        // Get movement input from player (W/S for forward/backward, A/D for turning)
        float moveForward = Input.GetAxis("Vertical");  // W/S or Arrow keys
        float turn = Input.GetAxis("Horizontal");       // A/D or Arrow keys

        movementInput = new Vector3(0, 0, moveForward); // Apply forward/backward movement

        // Apply turning based on input
        if (moveForward != 0)
        {
            transform.Rotate(0, turn * turnSpeed * Time.deltaTime, 0);
        }
    }

    void ApplySkiingForces()
    {
        // Gravity force (Unity's Rigidbody handles gravity automatically)
        Vector3 gravityForce = new Vector3(0, -9.81f, 0);
        
        // Friction (simulates snow friction)
        Vector3 frictionForce = -rb.linearVelocity.normalized * friction;
        
        // Apply forward movement (influence of slope on speed)
        Vector3 forwardMovement = transform.forward * forwardSpeed * movementInput.z * slopeFactor;

        // Apply forces
        rb.AddForce(gravityForce, ForceMode.Acceleration);
        rb.AddForce(frictionForce, ForceMode.Acceleration);
        rb.AddForce(forwardMovement, ForceMode.VelocityChange);
    }

    // On collision, you might want to simulate snow interactions
    void OnCollisionEnter(Collision collision)
    {
        
    }
}
