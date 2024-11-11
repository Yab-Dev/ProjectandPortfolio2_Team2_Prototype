using UnityEngine;

public class Bullet : MonoBehaviour
{

    [SerializeField] enum damageType { bullet, stationary};
    [SerializeField] damageType type;

    [SerializeField] private float speed = 25f; // Speed of bullet
    [SerializeField] private int damageAmount = 1; // Damage per hit
    [SerializeField] private float destroyTime = 3f; // Lifetime of bullet

    private Rigidbody rb; // Rigidbody for movement

    void Start() // Initialize bullet settings
    {
        if(type == damageType.bullet)
        {
            rb = GetComponent<Rigidbody>(); // Get Rigidbody component
            rb.linearVelocity = transform.forward * speed; // Set bullet speed
            Destroy(gameObject, destroyTime); // Destroy after set time
        }
    }

    private void OnTriggerEnter(Collider other) // Detect trigger collision
    {
        if (other.isTrigger)
            return;

        IDamage dmg = other.GetComponent<IDamage>();

        if (dmg != null )
        {
            dmg.DealDamage(damageAmount);
        }

        if (type == damageType.bullet)
        {
            Destroy(gameObject);
        }


        // BELOW CODE IS NOT IMPLEMENTED CORRECTLY

        //if (other.TryGetComponent<IDamage>(out IDamage target)) // Check for IDamage interface
        //{
        //    target.DealDamage(damageAmount); // Deal damage to target
        //}
        //Destroy(gameObject); // Destroy bullet on impact
    }
}