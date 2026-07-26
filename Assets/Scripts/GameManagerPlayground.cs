using UnityEngine;

public class GameManagerPlayground : MonoBehaviour
{
    //Referenz zum Player
    public PlayerMovement playerScript;

    //Referenz zu gebauten Turm des Spielers
    private GameObject playerTower;

    void Awake()
    {
        playerTower = GameObject.FindWithTag("PlayerTower");
        playerTower.AddComponent<PlayerTowerControl>();
    }

    // Start is called once before the first execution of Update after the MonoBehaviour is created
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        if (playerScript.hasReachedFinish)
        {
            GameFinished();
        }
    }

    //Spielende
    private void GameFinished()
    {
        print("Finished!)");
        //add loading Finish Screen
        //add measure points
    }
}
