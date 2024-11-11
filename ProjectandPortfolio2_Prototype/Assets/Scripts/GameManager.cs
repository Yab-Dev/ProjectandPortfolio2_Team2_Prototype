using System.Collections;
using System.Collections.Generic;
using UnityEngine;


public class GameManager : MonoBehaviour
{
    public static GameManager instance; // Singleton instance

    [SerializeField] private GameObject player; // Player reference

    [SerializeField] GameObject menuActive;
    [SerializeField] GameObject menuPause;
    [SerializeField] GameObject menuWin;
    [SerializeField] GameObject menuLose;


    public bool isPaused;
    float timeScaleOriginal;

    int enemyCount; // Active enemy count

    // Original time scale

    void Awake() // Setup singleton
    {
        if (instance == null)
        {
            instance = this; // Set instance
            DontDestroyOnLoad(gameObject); // Persist across scenes
            timeScaleOriginal = Time.timeScale;
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


    // Update is called once per frame
    void Update()
    {
        if (Input.GetButtonDown("Cancel"))
        {
            if (menuActive == null)
            {
                statePause();
                menuActive = menuPause;
                menuActive.SetActive(true);
            }
            else if (menuActive == menuPause)
            {
                stateUnpause();
            }
        }
    }

    // Update game goal
    public void updateGameGoal(int amount)
    {
        enemyCount += amount;

        // You WIN!!!
        if (enemyCount <= 0)
        {
            statePause();
            menuActive = menuWin;
            menuActive.SetActive(true);
        }
    }

    // Lose screen
    public void playerLose()
    {
        statePause();
        menuActive = menuLose;
        menuActive.SetActive(true);
    }



    // Pause the game
    public void statePause()
    {
        isPaused = !isPaused;
        Time.timeScale = 0;
        Cursor.visible = true;
        Cursor.lockState = CursorLockMode.Confined;
    }

    // Unpause the game
    public void stateUnpause()
    {
        isPaused = !isPaused;
        Time.timeScale = timeScaleOriginal;
        Cursor.visible = false;
        Cursor.lockState = CursorLockMode.Locked;
        menuActive.SetActive(false);
        menuActive = null;
    }

    

}