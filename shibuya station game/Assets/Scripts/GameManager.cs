using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class GameManager : MonoBehaviour
{
    public static GameManager Instance; // Pour accéder à cette instance depuis d'autres scripts

    public int score = 0; // Variable pour stocker le score
    public Text scoreText; // Texte UI pour afficher le score
    public Text bestScoreText;
    public Text lastScoreText;
    public int anger = 0;
    private int bestScore = 0;
    private int lastScore = 0;
    public Image angerBar;

    [SerializeField]
    public float maxAnger = 10f;

    public delegate void GameOverAction();
    public static event GameOverAction OnGameOver;


    private void Start()
    {
        LoadBestScore();
        LoadLastScore();
    }

    private void SaveBestScore()
    {
        PlayerPrefs.SetInt("BestScore", bestScore);
        PlayerPrefs.Save();
    }

    private void SaveLastScore()
    {
        PlayerPrefs.SetInt("LastScore", score);
        PlayerPrefs.Save();
    }

    private void LoadBestScore()
    {
        if (PlayerPrefs.HasKey("BestScore"))
        {
            bestScore = PlayerPrefs.GetInt("BestScore");
        }
    }

    private void LoadLastScore()
    {
        if (PlayerPrefs.HasKey("LastScore"))
        {
            lastScore = PlayerPrefs.GetInt("LastScore");
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
        angerBar.fillAmount = anger / maxAnger;
    }

    public void ResetScore()
    {
        score = 0;
        SaveLastScore();
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

    void UpdateBestScoreText()
    {
        if (bestScoreText != null)
        {
            bestScoreText.text = "Best Score: " + bestScore;
        }
    }

    void UpdateLastScoreText()
    {
        if (lastScoreText != null)
        {
            lastScoreText.text = "Score: " + lastScore;
        }
    }
    private void Update()
    {
        if (anger >= maxAnger)
        {
            if (OnGameOver != null)
            {
                SaveLastScore();
                OnGameOver();
            }
        }
        UpdateBestScoreText();
        UpdateLastScoreText();
    }

}
