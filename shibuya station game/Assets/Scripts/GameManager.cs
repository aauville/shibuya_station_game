using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class GameManager : MonoBehaviour
{
    public static GameManager Instance; // Pour accéder à cette instance depuis d'autres scripts

    public int score = 0; // Variable pour stocker le score
    public Text scoreText; // Texte UI pour afficher le score
    public int anger = 0;
    private int bestScore = 0;

    public delegate void GameOverAction();
    public static event GameOverAction OnGameOver;


    private void Start()
    {
        LoadBestScore();
    }

    private void SaveBestScore()
    {
        PlayerPrefs.SetInt("BestScore", bestScore);
        PlayerPrefs.Save();
    }

    private void LoadBestScore()
    {
        if (PlayerPrefs.HasKey("BestScore"))
        {
            bestScore = PlayerPrefs.GetInt("BestScore");
        }
    }

    void Awake()
    {
        // Assurez-vous qu'il n'y a qu'une seule instance du GameManager
        if (Instance == null)
        {
            Instance = this;
        }
        else
        {
            Destroy(gameObject);
        }
    }

    // Fonction pour incrémenter le score
    public void IncrementScore()
    {
        score++;
        UpdateScoreText();
        if (score > bestScore)
        {
            bestScore = score;
            SaveBestScore();
        }
    }

    public void IncrementAnger()
    {
        anger++;
    }

    public void ResetScore()
    {
        score = 0;
        UpdateScoreText();
    }

    public void ResetAnger()
    {
        anger = 0;
    }

    // Fonction pour mettre à jour le texte du score
    void UpdateScoreText()
    {
        if (scoreText != null)
        {
            scoreText.text = "Score: " + score;
        }
    }

    private void Update()
    {
        if (anger > 10)
        {
            if (OnGameOver != null)
            {
                OnGameOver();
            }
        }
    }

}
