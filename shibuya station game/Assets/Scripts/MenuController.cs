using UnityEngine;
using UnityEngine.SceneManagement;

public class MenuController : MonoBehaviour
{
    public void StartGame()
    {
        SceneManager.LoadScene("GameScene"); // Charge la scène de jeu
        Debug.Log("Le bouton 'Commencer le jeu' a été cliqué !");
    }
}
