using UnityEngine;

public class PlayerShooting : MonoBehaviour
{
    [Header("Shoot Settings")]
    [SerializeField] private LayerMask ignoreMask;
    [SerializeField] private int shootDamage;
    [SerializeField] private float shootSpeed;
    [SerializeField] private float shootDistance;

    [Header("Cache")]
    [SerializeField] private Transform playerCamera;

    // Private Variables
    private float shootCooldown = 0;

    void Update()
    {
        // Debug ray to see what the player is looking at
        Debug.DrawRay(playerCamera.transform.position, playerCamera.transform.forward * shootDistance, Color.red);

        // Shooting cooldown
        if (shootCooldown > 0 )
        {
            shootCooldown -= Time.deltaTime;
        }

        // Shoot Input
        if (Input.GetButton("Fire1") && shootCooldown <= 0)
        {
            Shoot();
        }
    }

    private void Shoot()
    {
        RaycastHit hit;
        if (Physics.Raycast(playerCamera.transform.position, playerCamera.transform.forward, out hit, shootDistance, ~ignoreMask))
        {
            Debug.Log(hit.collider.name);
        }
        shootCooldown = shootSpeed;
    }
}
