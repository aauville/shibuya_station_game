using UnityEngine;
using UnityEngine.SceneManagement;

public class SceneController : MonoBehaviour
{

    private void Start()
    {
        // S'abonne à l'événement GameOver
        GameManager.OnGameOver += GameOver;
    }

    public void StartGame()
    {
        SceneManager.LoadScene("GameScene");
        Debug.Log("Le bouton 'Commencer le jeu' a été cliqué !");
    }

    public void GameOver()
    {
        SceneManager.LoadScene("GameOverScene");
        Debug.Log("Le jeu est perdu");
    }

    public void RestartGame()
    {
        SceneManager.LoadScene("GameScene");
        Debug.Log("Le bouton 'Recommencer' a été cliqué !");
    }

    public void GoToMenu()
    {
        SceneManager.LoadScene("MenuScene");
        Debug.Log("Le bouton 'Menu' a été cliqué !");
    }
}
