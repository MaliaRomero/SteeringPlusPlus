using UnityEngine;

public class KinematicFlee : MonoBehaviour
{
    public Transform target;
    public float maxSpeed = 5f;

    void Update()
    {
        //get the direction away from the target
        Vector3 direction = transform.position - target.position;
        direction.Normalize(); //make it a unit vector
        
        //Velocoty this direction away from the player
        Vector3 velocity = direction * maxSpeed;
        transform.position += velocity * Time.deltaTime;

        // Use LookAt to face the fleeing direction, addition instead
        Vector3 lookDirection = transform.position + velocity;
        transform.LookAt(lookDirection);
    }
}