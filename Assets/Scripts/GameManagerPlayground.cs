using UnityEngine;
using TMPro;
using UnityEngine.UI;
using UnityEngine.EventSystems;
using System.Collections;
using UnityEngine.SceneManagement;

public class GameManagerPlayground : MonoBehaviour
{
    //Referenz zum Player
    public PlayerMovement playerScript;

    //Referenz zu gebauten Turm des Spielers
    private GameObject playerTower;
    private PlayerTowerControl towerSkript;

    public GameObject respawnPoint;

    //Variable für den Timer
    private float timeTaken;
    public TextMeshProUGUI timerText;

    //Variablen für Punkte-Vergabe
    private float maxPointsTower = 50f;
    private float maxTowerDestructions = 10f;
    private float maxPointsTimer = 50f;
    private float graceTime = 120f;
    private float maxTime = 300f;
    private float finalPoints;
    private float highScore;

    public GameObject endScreen;
    public GameObject pauseScreen;

    public TextMeshProUGUI finalPointsText;

    void Awake()
    {
        //Tower-Skript auf den aus der Building-Szene übergebenen Turm legen
        playerTower = GameObject.FindWithTag("PlayerTower");
        playerTower.AddComponent<PlayerTowerControl>();

        //Mögliche gespeicherte Lautstärke-Preferenzen
        float volume = PlayerPrefs.GetFloat("Volume", 1f);
        AudioListener.volume = volume;
    }

    // Start is called once before the first execution of Update after the MonoBehaviour is created
    void Start()
    {
        ResetTimer();

        //Referenz zum Tower-Skript
        towerSkript = GameObject.FindWithTag("PlayerTower").GetComponent<PlayerTowerControl>();

        pauseScreen.SetActive(false);
        endScreen.SetActive(false);

        highScore = PlayerPrefs.GetFloat("highScore");

    }

    // Update is called once per frame
    void Update()
    {
        if (Input.GetKeyDown(KeyCode.Escape))
        {
            Cursor.lockState = CursorLockMode.None;
            pauseScreen.SetActive(true);
            Time.timeScale = 0f;
        }

        if (playerScript.hasReachedFinish)
        {
            //Ende des Spiels
            GameFinished();
        }
        else
        {
            //Messeung der Zeit für die Parkour-Punkte
            timeTaken += Time.deltaTime;
        }

        float timeRemaining = maxTime - timeTaken;
        //Timer in Sekunden und Minuten umrechnen
        float minutes = Mathf.FloorToInt(timeRemaining / 60);
        float seconds = Mathf.FloorToInt(timeRemaining % 60);

        timerText.text = minutes + " : " + seconds;
    }

    //Timer zurücksetzen
    private void ResetTimer()
    {
        timeTaken = 0f;
    }

    private void CalculateFinalPoints(float timeTaken, int towerDestructions)
    {
        float finalTimerPoints;
        float finalTowerPoints;

        float towerAccuracy = PlayerPrefs.GetFloat("TowerAccuracy");

        //Punkte für die gebrauchte Zeit berechnen
        if (timeTaken <= graceTime)
        {
            finalTimerPoints = maxPointsTimer;
        }
        else
        {
            float scoringTime = maxTime - graceTime;
            float normalizedTime = Mathf.Clamp01((timeTaken - graceTime) / scoringTime);

            finalTimerPoints = Mathf.RoundToInt(maxPointsTimer * (1f - normalizedTime));
        }

        //Punkte für den Transport des Turms berechnen
        float normalizedTowerDestructs = Mathf.Clamp01(towerDestructions / maxTowerDestructions);
        finalTowerPoints = Mathf.RoundToInt(maxPointsTower * (1f - normalizedTowerDestructs));

        //Finale Punktzahl für Parkour
        finalPoints = finalTimerPoints + finalTowerPoints + towerAccuracy;

        //Überprüfung nach neuem Highscore
        if (finalPoints > highScore)
        {
            PlayerPrefs.SetFloat("highScore", finalPoints);
        }

        //Ausgabe der erreichten Punkte
        finalPointsText.text = "Erreichte Punkte: " + finalPoints;

    }

    //Spielende
    private void GameFinished()
    {
        Cursor.lockState = CursorLockMode.None;
        endScreen.SetActive(true);

        //Timer in Sekunden und Minuten umrechnen
        float minutes = Mathf.FloorToInt(timeTaken / 60);
        float seconds = Mathf.FloorToInt(timeTaken % 60);

        print(minutes + " : " + seconds);

        CalculateFinalPoints(timeTaken, towerSkript.towerDestructions);

        print(finalPoints);
    }
}
