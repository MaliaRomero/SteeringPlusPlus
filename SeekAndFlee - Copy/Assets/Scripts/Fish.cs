//Put this on all of the fish objects

using UnityEngine;
using TMPro;

public class Fish : MonoBehaviour
{
    public Transform player;
    public Transform camera1; // this is here so that when the player dies
    // there will still be a camera since I set player active to false
    public float sizeThreshold = 1.2f; // If player is this much bigger, fish will flee
    private KinematicSeek seek;
    private KinematicFlee flee;

    public Rigidbody rb;

    // Obstacle Avoidance Parameters
    public float detectionDistance = 5.0f;
    public float avoidanceStrength = 1.5f;
    public float safeDistance = 1.0f;
    public float sidewaysStrength = 2.0f;  // Strength of sideways movement when close to an obstacle
    public GameObject[] obstacles;

    public GameObject winLosePanel;  // Win Screen
    public TextMeshProUGUI loseText;


    void Start()
    {
        seek = GetComponent<KinematicSeek>();
        flee = GetComponent<KinematicFlee>();

        // Ensure both scripts are disabled initially
        seek.enabled = false;
        flee.enabled = false;

        rb = GetComponent<Rigidbody>();
    }

    void Update()
    {

        float playerSize = player.GetComponent<PlayerController>().GetCurrentSize();  // Get current size of player
        float mySize = transform.localScale.x;

        // toggle between seek and flee based on size, 1.2
        if (playerSize >= mySize * sizeThreshold)
        {
            ActivateFlee();
        }
        else
        {
            ActivateSeek();
        }

        //ApplyObstacleAvoidance();
    }

    void ActivateSeek()
    {
        seek.enabled = true;
        flee.enabled = false;
        seek.target = player;
    }

    void ActivateFlee()
    {
        flee.enabled = true;
        seek.enabled = false;
        flee.target = player;
    }

    void OnTriggerEnter(Collider other)
    {
        if (other.CompareTag("Player"))
        {
            float playerSize = player.localScale.x;
            float mySize = transform.localScale.x;

            if (playerSize >= mySize * sizeThreshold)
            {
                other.GetComponent<PlayerController>().IncreaseSize(mySize); //Player eats fish, gets bigger
                Destroy(gameObject); //fish destroyed
            }

            else if (playerSize <= mySize * sizeThreshold)
            {
                camera1.SetParent(null); // so there is still a camera in the scene
                player.gameObject.SetActive(false); // player destroyed
                Debug.Log("Dead");
                winLosePanel.SetActive(true);
                loseText.gameObject.SetActive(true);

            }
        }
    }
}

/*
// Obstacle Avoidance Logic
    void ApplyObstacleAvoidance()
    {
        // We will combine the fish's seek/flee movement with obstacle avoidance here
        Vector3 avoidanceForce = Vector3.zero;

        foreach (var obstacle in obstacles)
        {
            RaycastHit hit;

            // Cast a ray from the fish's position forward
            if (Physics.Raycast(transform.position, transform.forward, out hit, detectionDistance))
            {
                // If the ray hits an obstacle
                if (hit.collider.gameObject == obstacle)
                {
                    // Calculate the avoidance direction
                    Vector3 hitPoint = hit.point;    // The point of collision
                    Vector3 hitNormal = hit.normal;  // The surface normal at the collision point

                    // Calculate the avoidance target
                    Vector3 avoidanceTarget = hitPoint + hitNormal * safeDistance;

                    // Calculate the steering force to avoid the obstacle
                    Vector3 steering = avoidanceTarget - transform.position;
                    steering.Normalize();
                    steering *= avoidanceStrength;

                    // If the fish is too close to the obstacle, apply sideways movement
                    if (hit.distance < safeDistance)
                    {
                        // Calculate sideways direction (perpendicular to the hit normal)
                        Vector3 sideways = Vector3.Cross(hitNormal, Vector3.up);
                        sideways.Normalize();
                        steering += sideways * sidewaysStrength;  // Apply sideways velocity to the steering
                    }

                    avoidanceForce = steering;  // Store the avoidance force
                }
            }
        }

        // If there's an avoidance force, apply it along with seek or flee
        if (avoidanceForce != Vector3.zero)
        {
            // Add the avoidance force to the movement
            rb.velocity = avoidanceForce;  // Directly apply the avoidance force to stop the fish from moving through the wall
        }
        else
        {
            // If no obstacle is detected, apply the seek/flee velocity
            Vector3 desiredVelocity = Vector3.zero;
            if (seek.enabled)
            {
                // Seek behavior: move towards the player
                desiredVelocity = (player.position - transform.position).normalized * 10f; // Adjust the speed factor (10f)
            }
            else if (flee.enabled)
            {
                // Flee behavior: move away from the player
                desiredVelocity = (transform.position - player.position).normalized * 10f; // Adjust the speed factor (10f)
            }

            // Apply the combined steering to the Rigidbody's velocity
            rb.velocity = desiredVelocity + avoidanceForce; // Add avoidance force to desired velocity
        }
    }
*/