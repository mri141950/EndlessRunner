using UnityEngine;
using TMPro;  // 引入 TextMeshPro 命名空间

public class ScoreManager : MonoBehaviour
{
    [Header("UI Reference")]
    public TMP_Text scoreText;   // 使用 TextMeshPro 的 TMP_Text
    public Transform player;     // Reference to the player's transform

    private float score;         // Player's score based on distance

    void Update()
    {
        // Calculate score from player's x-position
        score = player.position.x;
        // Update the TMP text, casting to int for whole-number display
        scoreText.text = "Score: " + ((int)score).ToString();
    }
}
