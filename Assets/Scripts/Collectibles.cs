using System;
using UnityEngine;

public class Collectibles : MonoBehaviour
{
    private int score;

    private AudioManager audioManager;

    private void Awake()
    {
        audioManager = GameObject.FindGameObjectWithTag("Audio").GetComponent<AudioManager>();
    }

    private void Start()
    {
        score = 1;
    }

    private void OnTriggerEnter2D(Collider2D other)
    {
        if (other.CompareTag("Player"))
        {
            ScoreManager.Instance.AddScore(score);
            gameObject.SetActive(false);
            audioManager.PlayCollecion();
        }
    }


}
