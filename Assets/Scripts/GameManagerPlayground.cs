using UnityEngine;

public class GameManagerPlayground : MonoBehaviour
{
    //Referenz zum Player
    public PlayerMovement playerScript;

    //Referenz zu gebauten Turm des Spielers
    private GameObject playerTower;
    private PlayerTowerControl towerSkript;

    //Variable für den Timer
    private float timer;

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
            timer += Time.deltaTime;
        }
    }

    //Timer zurücksetzen
    private void ResetTimer()
    {
        timer = 0f;
    }

    private void CalculateFinalPoints(float timer, int towerDestructions)
    {

    }

    //Spielende
    private void GameFinished()
    {
        print("Finished!)");
        print(timer);

        //Timer in Sekunden und Minuten umrechnen
        float minutes = Mathf.FloorToInt(timer / 60);
        float seconds = Mathf.FloorToInt(timer % 60);

        print(minutes + " : " + seconds);

        CalculateFinalPoints(timer, towerSkript.towerDestructions);
        //add loading Finish Screen
        //add measure points
    }
}
