using UnityEngine;
using TMPro;

public class PlayerController : MonoBehaviour
{
    public float speed = 15f;
    private Rigidbody rb;

    public Transform cameraTransform;  // Reference to the camera's transform (if you want to access it directly)

    public float grow = 1.2f; // how much player grows when eating a fish

    private float currentSize = 1f;

    public TextMeshProUGUI scoreText;

    public int score = 0;

    public GameObject winLosePanel;  // Win Screen
    public TextMeshProUGUI loseText;
    public TextMeshProUGUI winText;
    void Start()
    {
        rb = GetComponent<Rigidbody>();
    }

    void Update()
    {
        float horizontal = Input.GetAxis("Horizontal");
        float vertical = Input.GetAxis("Vertical");

        Vector3 movement = new Vector3(horizontal, 0, vertical).normalized;

        rb.velocity = movement * speed;

        // Rotate the fish (player mesh) towards the movement direction
        if (movement != Vector3.zero)
        {
            // Calculate the target rotation based on movement direction
            Quaternion targetRotation = Quaternion.LookRotation(movement);
            transform.rotation = targetRotation;  // Directly set the rotation to match the movement direction
        }

        // Update camera position but keep it fixed (top-down view)
        Vector3 cameraPosition = new Vector3(transform.position.x, cameraTransform.position.y, transform.position.z);
        cameraTransform.position = cameraPosition;

        // Ensure camera does not rotate (keeps the top-down view)
        cameraTransform.rotation = Quaternion.Euler(90f, 0f, 0f);  // This ensures the camera remains top-down
    }


    public void IncreaseSize(float size)
    {
       currentSize *= grow; // Update the current size based on growth factor
        transform.localScale = new Vector3(currentSize, currentSize, currentSize);

        score += 10;
        scoreText.text = "Score : " + score;

        if (score > 130) {
            winLosePanel.SetActive(true);
            winText.gameObject.SetActive(true);
        }
    }

    public float GetCurrentSize()
    {
        return currentSize;
    }
}