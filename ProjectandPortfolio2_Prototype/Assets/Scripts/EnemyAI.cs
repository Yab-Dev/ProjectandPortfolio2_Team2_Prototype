using System.Collections; // For IEnumerator use
using UnityEngine; // Unity engine
using UnityEngine.AI; // NavMesh for navigation

public class EnemyAI : MonoBehaviour, IDamage // Enemy AI with health
{
    [SerializeField] NavMeshAgent agent; // NavMesh for movement
    [SerializeField] Renderer model; // Renderer to change color
    [SerializeField] Transform shootPos; // Bullet spawn position
    [SerializeField] Transform headPos; // For tracking player
    [SerializeField] int HP; // Enemy health points
    [SerializeField] int faceTargetSpeed; // Turn speed to face target
    [SerializeField] GameObject bullet; // Bullet prefab
    [SerializeField] float shootRate; // Time between shots

    private Color colorOrig; // Store original color
    private bool isShooting; // Shooting state
    private bool playerInRange; // Player range state
    private Vector3 playerDir; // Direction to player
    private MaterialPropertyBlock propBlock; // For individual material properties

    void Start() // Initial setup
    {
        colorOrig = model.material.color; // Cache original color
        propBlock = new MaterialPropertyBlock(); // Initialize MaterialPropertyBlock
        model.GetPropertyBlock(propBlock);
        propBlock.SetColor("_Color", colorOrig); // Set the initial color in propBlock
        model.SetPropertyBlock(propBlock);
        GameManager.instance.updateGameGoal(1);
        // Register this enemy in GameManager
    }

    void Update() // Frame-by-frame updates
    {
        if (playerInRange) // If player in range
        {
            if (GameManager.instance != null && GameManager.instance.Player != null && headPos != null)
            {
                playerDir = (GameManager.instance.Player.transform.position - headPos.position).normalized; // Calculate player direction
                agent.SetDestination(GameManager.instance.Player.transform.position); // Move towards player

                if (agent.remainingDistance <= agent.stoppingDistance) // If within stopping distance
                {
                    FaceTarget(); // Face player
                }

                if (!isShooting) // If not shooting
                {
                    StartCoroutine(Shoot()); // Start shooting coroutine
                }
            }
            else
            {
                Debug.LogError("GameManager, Player, or headPos is null in EnemyAI.");
            }
        }
    }

    private void OnTriggerEnter(Collider other) // Detect player entry
    {
        if (other.CompareTag("Player")) // If collider is player
        {
            playerInRange = true; // Set player in range
        }
    }

    private void OnTriggerExit(Collider other) // Detect player exit
    {
        if (other.CompareTag("Player")) // If collider is player
        {
            playerInRange = false; // Set player out of range
        }
    }

    public void DealDamage(int amount) // Apply damage to enemy
    {
        HP -= amount; // Subtract damage from HP
        StartCoroutine(FlashRed()); // Trigger damage feedback

        agent.SetDestination(GameManager.instance.Player.transform.position); // Re-focus on player

        if (HP <= 0) // If HP is zero or below
        {
            // Deregister in GameManager
            Destroy(gameObject); // Destroy this enemy
            Debug.Log("Enemy destroyed"); // Log destruction
            GameManager.instance.updateGameGoal(-1);

        }
    }

    private IEnumerator FlashRed() // Flash red on damage
    {
        model.material.color = Color.red; // Change color to red

        //model.GetPropertyBlock(propBlock);
        //propBlock.SetColor("_Color", Color.red); // Change to red
        //model.SetPropertyBlock(propBlock);

        yield return new WaitForSeconds(0.1f); // Wait briefly

        model.material.color = colorOrig;
        //propBlock.SetColor("_Color", colorOrig); // Reset to original color
        //model.SetPropertyBlock(propBlock);
        Debug.Log("Reset color"); // Log after resetting color
    }

    private IEnumerator Shoot() // Manage shooting rate
    {
        isShooting = true; // Set shooting active
        Instantiate(bullet, shootPos.position, transform.rotation); // Spawn bullet
        yield return new WaitForSeconds(shootRate); // Wait for shoot rate interval
        isShooting = false; // Reset shooting state
    }

    void FaceTarget() // Rotate to face player
    {
        Quaternion targetRotation = Quaternion.LookRotation(playerDir); // Set target rotation
        transform.rotation = Quaternion.RotateTowards(transform.rotation, targetRotation, faceTargetSpeed * Time.deltaTime); // Smooth rotate towards player
    }


}
