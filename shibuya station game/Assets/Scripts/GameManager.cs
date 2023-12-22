using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class GameManager : MonoBehaviour
{
    public static GameManager Instance; // Pour accéder à cette instance depuis d'autres scripts

    public int score = 0; // Variable pour stocker le score
    public Text scoreText; // Texte UI pour afficher le score

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
    }

    // Fonction pour mettre à jour le texte du score
    void UpdateScoreText()
    {
        if (scoreText != null)
        {
            scoreText.text = "Score: " + score;
        }
    }
}
