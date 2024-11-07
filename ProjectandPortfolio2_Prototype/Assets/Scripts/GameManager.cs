using UnityEngine; // Unity engine library

public class GameManager : MonoBehaviour
{
    public static GameManager instance; // Singleton instance

    [SerializeField] private GameObject player; // Player reference
    public int enemyCount; // Active enemy count

    // Original time scale

    void Awake() // Setup singleton
    {
        if (instance == null)
        {
            instance = this; // Set instance
            DontDestroyOnLoad(gameObject); // Persist across scenes
        }
        else
        {
            Destroy(gameObject); // Destroy duplicates
        }
    }

    void Start() // Ensure player reference
    {
        if (player == null) // If player unassigned
        {
            player = GameObject.FindGameObjectWithTag("Player"); // Find player by tag
            if (player == null)
            {
                Debug.LogError("Player not found!"); // Log error if missing
            }
        }
    }

    public GameObject Player // Player accessor
    {
        get { return player; } // Return player object
    }

    public Vector3 GetPlayerPosition() // Get player position
    {
        if (player != null)
        {
            return player.transform.position; // Return player position
        }
        else
        {
            Debug.LogError("Player reference is null!"); // Log error if player is null
            return Vector3.zero; // Return zero vector as fallback
        }
    }

    // Update game goal

    // Win screen

    // Lose screen

    // Pause the game

    // Unpause the game

}