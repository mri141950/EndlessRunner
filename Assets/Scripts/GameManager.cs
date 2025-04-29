using UnityEngine;
using TMPro;
using UnityEngine.SceneManagement;

public class GameManager : MonoBehaviour
{
    [Header("UI References")]
    public TMP_Text gameOverText;  // Reference to the Game Over UI
    private bool isGameOver = false;

    void Start()
    {
        // At start, hide the Game Over text
        gameOverText.gameObject.SetActive(false);
        // Ensure normal time scale
        Time.timeScale = 1f;
    }

    void Update()
    {
        if (isGameOver)
        {
            // Press R to reload the current scene
            if (Input.GetKeyDown(KeyCode.R))
            {
                // Resume time and reload scene
                Time.timeScale = 1f;
                SceneManager.LoadScene(SceneManager.GetActiveScene().buildIndex);
            }
        }
    }

    // Called by other scripts to trigger game over
    public void GameOver()
    {
        // Stop game time
        Time.timeScale = 0f;
        // Show Game Over UI
        gameOverText.gameObject.SetActive(true);
        isGameOver = true;
    }
}
