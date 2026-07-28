using UnityEngine;
using UnityEngine.SceneManagement;
using TMPro;
using UnityEngine.UI;

public class Menu : MonoBehaviour
{
    public TextMeshProUGUI highscoreText;

    // Start is called once before the first execution of Update after the MonoBehaviour is created
    void Start()
    {
        float savedHighscore = PlayerPrefs.GetFloat("highScore");
        highscoreText.text = "Highscore: " + savedHighscore;
    }

    // Update is called once per frame
    void Update()
    {
        
    }
    public void QuitGame()
    {
        Debug.Log("Quit Game");
        Application.Quit();
    }
    public void StartGame()
    {
        Debug.Log("Start Game");
        // Test Modus Türme laden nach jedem start den nächsten (nicht random so wie für den Spieler)
        PlayerPrefs.SetInt("TowerIndex", 0);
        //Läd Bau -Modus Szene
        SceneManager.LoadScene("Building");

    }
}
