using UnityEngine;
using UnityEngine.UI;
using UnityEngine.EventSystems;
using System.Collections;
using UnityEngine.SceneManagement;

public class PauseGame : MonoBehaviour
{
    //Verbindung zu den Buttons
    public Button resumeBtn;
    public Button mainMenuBtn;
    public Button retryBtn;

    // Start is called once before the first execution of Update after the MonoBehaviour is created
    void Start()
    {
       
    }

    public void RestartCurrentLevel()
    {
        Debug.Log("Restart");
        // 1. Die Zeit wieder auf normal stellen (falls das Spiel pausiert war!)
        Time.timeScale = 1f;

        // 2. Den Namen der AKTUELLEN Szene herausfinden
        string currentSceneName = SceneManager.GetActiveScene().name;

        // 3. Diese Szene genau jetzt neu laden
        SceneManager.LoadScene(currentSceneName);
    }

    public void LoadMainMenu()
    {
        Debug.Log("Main Menu");
        // 1. GANZ WICHTIG: Zeit wieder auftauen! 
        // Sonst sind Animationen im Hauptmenü eingefroren.
        Time.timeScale = 1f;

        // 2. Lade die Szene mit dem Namen deines Hauptmenüs.
        // ACHTUNG: Trage hier exakt den Namen deiner Hauptmenü-Szene ein!
        SceneManager.LoadScene("Menu");
    }

    public void ResumePlaythrough()
    {
        Debug.Log("Resume");
        Time.timeScale = 1f;
        gameObject.SetActive(false);
    }
}
