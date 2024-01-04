using UnityEngine;

public class BirdController : MonoBehaviour
{
    public float minSpeed = 0.5f; // Adjust this as needed
    public float maxSpeed = 1.5f; // Adjust this as needed
    public float minRadius = 3f; // Adjust this as needed
    public float maxRadius = 7f; // Adjust this as needed

    private float speed;
    private float radius;
    private float angle;
    private Vector3 startPosition;

    void Start()
    {
        startPosition = transform.position;

        // Initialize speed, radius, and angle with random values
        speed = Random.Range(minSpeed, maxSpeed);
        radius = Random.Range(minRadius, maxRadius);
        angle = Random.Range(0f, 360f);
    }

    void Update()
    {
        angle += speed * Time.deltaTime;

        float x = Mathf.Cos(angle) * radius;
        float z = Mathf.Sin(angle) * radius;
        Vector3 newPosition = startPosition + new Vector3(x, 0, z);

        Vector3 direction = (newPosition - transform.position).normalized;

        // Rotate the bird to face the direction of its movement
        if (direction != Vector3.zero)
        {
            Quaternion toRotation = Quaternion.LookRotation(direction, Vector3.up);
            toRotation *= Quaternion.Euler(0, 90, 0); // Add 90 degrees rotation around the Y-axis

            transform.rotation = Quaternion.Lerp(transform.rotation, toRotation, speed * Time.deltaTime);
        }

        transform.position = newPosition;

    }
}
