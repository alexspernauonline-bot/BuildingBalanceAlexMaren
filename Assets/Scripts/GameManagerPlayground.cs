using UnityEngine;
using TMPro;

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
    private float finalPointsParkour;

    void Awake()
    {
        //Tower-Skript auf den aus der Building-Szene übergebenen Turm legen
        playerTower = GameObject.FindWithTag("PlayerTower");
        playerTower.AddComponent<PlayerTowerControl>();
    }

    // Start is called once before the first execution of Update after the MonoBehaviour is created
    void Start()
    {
        ResetTimer();

        //Referenz zum Tower-Skript
        towerSkript = GameObject.FindWithTag("PlayerTower").GetComponent<PlayerTowerControl>();
    }

    // Update is called once per frame
    void Update()
    {
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
        finalPointsParkour = finalTimerPoints + finalTowerPoints;

    }

    //Spielende
    private void GameFinished()
    {
        print("Finished!)");

        //Timer in Sekunden und Minuten umrechnen
        float minutes = Mathf.FloorToInt(timeTaken / 60);
        float seconds = Mathf.FloorToInt(timeTaken % 60);

        print(minutes + " : " + seconds);

        CalculateFinalPoints(timeTaken, towerSkript.towerDestructions);

        print(finalPointsParkour);
        //add loading Finish Screen
    }
}
