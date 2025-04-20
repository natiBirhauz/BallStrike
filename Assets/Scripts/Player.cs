using UnityEngine;

public class Player : MonoBehaviour
{
    public GameManager gm;
    public Rigidbody rb;
    public float forwardSpeed = 100f;
    public float sideSpeed = 450f;
    public float maxSpeed = 100f;
     private float hitCooldown = 1f;  // Cooldown time in seconds
    private float lastHitTime = 0f;  // Time when player was last hit

    void Start()
    {
        // Ensure the Rigidbody is assigned
        if (rb == null)
            rb = GetComponent<Rigidbody>();

        // Ensure the GameManager is assigned
        if (gm == null)
            gm = Object.FindFirstObjectByType<GameManager>();
        
        gm.InitializePlayer(this);
       // rb.isKinematic = true; // Disable physics reactions (e.g., no pushback from enemy balls)

    }

    void FixedUpdate()
    {

        float currentSpeed = rb.linearVelocity.z;
        // Limit the forward speed to maxSpeed
        if (currentSpeed > maxSpeed)
        {
            Vector3 clampedVelocity = rb.linearVelocity;
            clampedVelocity.z = maxSpeed;
            rb.linearVelocity = clampedVelocity;
        }
        else if (currentSpeed < maxSpeed)
        {
        rb.AddForce(Vector3.forward * forwardSpeed * Time.deltaTime, ForceMode.Force);
        }    

        // Side movement
        if (Input.GetKey(KeyCode.A) || Input.GetKey(KeyCode.LeftArrow))
            rb.AddForce(-sideSpeed * Time.deltaTime, 0, 0, ForceMode.Force);
        if (Input.GetKey(KeyCode.D) || Input.GetKey(KeyCode.RightArrow))
            rb.AddForce(sideSpeed * Time.deltaTime, 0, 0, ForceMode.Force);
    }

private void OnCollisionEnter(Collision collision)
{
    if (collision.gameObject.CompareTag("EnemyBall"))
    {
        if (Time.time - lastHitTime > hitCooldown)
        {
            collision.gameObject.GetComponent<EnemyBall>().Hit();
            lastHitTime = Time.time;
        }
    }
    else if (collision.gameObject.CompareTag("HealthBall"))
    {
        collision.gameObject.GetComponent<HealthBall>().Hit();
    }
        else if (collision.gameObject.CompareTag("PointsBall"))
    {
        collision.gameObject.GetComponent<PointsBall>().Hit();
    }
}

}
