using UnityEngine;

public class Bullet : MonoBehaviour
{
    [SerializeField] private float speed = 25f; // Speed of bullet
    [SerializeField] private int damageAmount = 1; // Damage per hit
    [SerializeField] private float destroyTime = 3f; // Lifetime of bullet

    private Rigidbody rb; // Rigidbody for movement

    void Start() // Initialize bullet settings
    {
        rb = GetComponent<Rigidbody>(); // Get Rigidbody component
        rb.linearVelocity = transform.forward * speed; // Set bullet speed
        Destroy(gameObject, destroyTime); // Destroy after set time
    }

    private void OnTriggerEnter(Collider other) // Detect trigger collision
    {
        if (other.TryGetComponent<IDamage>(out IDamage target)) // Check for IDamage interface
        {
            target.DealDamage(damageAmount); // Deal damage to target
        }
        Destroy(gameObject); // Destroy bullet on impact
    }
}