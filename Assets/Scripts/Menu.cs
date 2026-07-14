using UnityEngine;
using UnityEngine.SceneManagement;
using TMPro;

public class Menu : MonoBehaviour
{
    // Start is called once before the first execution of Update after the MonoBehaviour is created
    void Start()
    {

    }

    // Update is called once per frame
    void Update()
    {

    }
    public void QuitGame()
    {
        Debug.Log("Quit Game");
        // Application.Quit();
    }
    public void StartGame()
    {
        SceneManager.LoadScene(0);
    }
    public void EasyMode()
    {

    }

}
