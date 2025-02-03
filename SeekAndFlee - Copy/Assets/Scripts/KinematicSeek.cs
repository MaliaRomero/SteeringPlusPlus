using UnityEngine;

public class KinematicSeek : MonoBehaviour
{
    public Transform target;
    public float maxSpeed = 5f;

    void Update()
    {
        //get the direction to the target
        Vector3 direction = target.position - transform.position;
        direction.Normalize(); // Making it a unit vector

        //Velocity this direction at full speed
        Vector3 velocity = direction * maxSpeed;
        transform.position += velocity * Time.deltaTime;

        //Face in direction we want to move
        transform.LookAt(target);
    }
}
