using UnityEngine;
using TMPro;

public class ScoreManager : MonoBehaviour
{
    [Header("References")]
    public TMP_Text scoreText;
    public Transform player;

    private float score;
    private bool isCounting = true;

    private void OnEnable()
    {
        GameManager.OnGameOver += StopScore;
    }

    private void OnDisable()
    {
        GameManager.OnGameOver -= StopScore;
    }

    void Update()
    {
        if (!isCounting || player == null) return;

        score = player.position.x;
        scoreText.text = "Score: " + ((int)score).ToString();
    }

    public void ResetScore()
    {
        score = 0f;
        isCounting = true;
        scoreText.text = "Score: 0";
    }

    private void StopScore()
    {
        isCounting = false;
    }
}
