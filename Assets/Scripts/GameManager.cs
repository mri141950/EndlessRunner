using UnityEngine;
using TMPro;
using UnityEngine.SceneManagement;
using System;

public class GameManager : MonoBehaviour
{
    public static GameManager Instance { get; private set; }

    [Header("UI References")]
    public TMP_Text gameOverText;

    public static event Action OnGameOver;

    public static event Action OnRestart;

    private bool isGameOver = false;

    private void Awake()
    {

        if (Instance != null && Instance != this)
        {
            Destroy(gameObject);
            return;
        }
        Instance = this;
    }

    void Start()
    {
        gameOverText.gameObject.SetActive(false);
        Time.timeScale = 1f;
    }

    void Update()
    {
        if (isGameOver && Input.GetKeyDown(KeyCode.R))
        {
            Time.timeScale = 1f;
            SceneManager.LoadScene(SceneManager.GetActiveScene().buildIndex);
        }
        if (isGameOver && Input.GetKeyDown(KeyCode.R))
        {
            OnRestart?.Invoke();
            Time.timeScale = 1f;
            SceneManager.LoadScene(SceneManager.GetActiveScene().buildIndex);
        }

    }

    public void TriggerGameOver()
    {
        if (isGameOver) return;

        Time.timeScale = 0f;
        gameOverText.gameObject.SetActive(true);
        isGameOver = true;

        OnGameOver?.Invoke();
    }
}
