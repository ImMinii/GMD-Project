using System;
using UnityEngine;
using TMPro;

public class ScoreManager : MonoBehaviour
{
    public static ScoreManager Instance { get; private set; }

    private int score = 0;
    public TextMeshProUGUI scoreText;

    [SerializeField]
    private int NumberofGems = 20;
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
        
    }
    
    public void AddScore(int amount)
    {
        
        scoreText.gameObject.SetActive(true);
        
        score += amount;
        UpdateScoreUI();
    }

    void UpdateScoreUI()
    {
        if (scoreText != null)
        {
            scoreText.text = score + "/" + NumberofGems;
        }
    }
}