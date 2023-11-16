using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class ChangeMenu : MonoBehaviour
{
    // Start is called before the first frame update
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        
    }

    public void StartGameScene()
    {
    SceneManager.LoadScene("Game");
    }

    public void GotoMenuScene()
    {
    SceneManager.LoadScene("Menu");
    }
}


