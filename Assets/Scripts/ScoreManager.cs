using System;
using UnityEngine;
using TMPro;

public class ScoreManager : MonoBehaviour
{
    public static ScoreManager Instance { get; private set; }

    private int score;
    public TextMeshProUGUI scoreText;
    public GameObject scoreUI; 
    private bool hasShownUI = false;

    private void Awake()
    {
        // Singleton pattern
        if (Instance == null)
            Instance = this;
        else
        {
            Destroy(gameObject);
            return;
        }
        
        if (scoreText != null)
        {
            scoreText.gameObject.SetActive(false);
        }
    
        if (scoreUI != null)
        {
            scoreUI.SetActive(false);
        } 
    }
    
    public void AddScore(int amount)
    {
        if (!hasShownUI && scoreUI != null)
        {
            scoreUI.SetActive(true);
            scoreText.gameObject.SetActive(true);
            hasShownUI = true;
        }
        score += amount;
        UpdateScoreUI();
    }

    void UpdateScoreUI()
    {
        if (scoreText != null)
        {
            scoreText.text = score.ToString();
        }
    }
}