using UnityEngine;
using UnityEngine.SceneManagement;


public class TowerTransitionManager : MonoBehaviour
{
    //Spieler baut den Turm und drückt dann auf den Button, um in die Parkour Szene zu wechseln.
    public GameObject levelEndPromptPanel;
    //Hier kommt der Name von der Parkour Szene rein
    public string nextSceneName = "BuildingBalance 1";
    

    // So weiß er welcher Turm welcher ist
    public Transform leftScale;
    public Transform rightScale;
    void Start()
    {
        // Das Panel wird zu Beginn deaktiviert, damit es nicht sofort sichtbar ist
        if (levelEndPromptPanel != null)
        {
            levelEndPromptPanel.SetActive(false);
        }
    }
    public void ShowEndPrompt()
    {
        if (levelEndPromptPanel != null)
        {
            levelEndPromptPanel.SetActive(true);
            //Turm friert ein
            Time.timeScale = 0f; 
        }
    }
    // Wird vom "Weiterbauen"-Button aufgerufen
    public void ContinueBuilding()
    {
        levelEndPromptPanel.SetActive(false);

        // Spielzeit wieder normal weiterlaufen lassen, falls pausiert wurde
        // Time.timeScale = 1f;

        Debug.Log("Spieler baut weiter für Highscore!");
    }
    // Wird vom "Nächste Szene"-Button aufgerufen
    public void TransferTowerAndLoadScene()
    {
        //Turm darf wieder bewegen
        Time.timeScale = 1f;
        GameObject leftContainer = new GameObject("SavedTower_Left");
        GameObject rightContainer = new GameObject("SavedTower_Right");

        rightContainer.transform.position = rightScale.position;
        leftContainer.transform.position = leftScale.position;


        GameObject[] allBlocks = GameObject.FindGameObjectsWithTag("TowerBlock");

        foreach (GameObject block in allBlocks)
        {
            float distToLeft = Vector3.Distance(block.transform.position, leftScale.position);
            float distToRight = Vector3.Distance(block.transform.position, rightScale.position);

            if (distToLeft < distToRight)
            {
                block.transform.SetParent(leftContainer.transform, true);
            }
            else
            {
                block.transform.SetParent(rightContainer.transform, true);
            }
            Rigidbody rb = block.GetComponent<Rigidbody>();
            if (rb != null)
            {
                // Er behält sein Gewicht, bewegt sich aber nicht mehr von selbst.
               
                rb.linearVelocity = Vector3.zero;
                rb.angularVelocity = Vector3.zero;
            }
        }

        rightContainer.tag = "PlayerTower";

        DontDestroyOnLoad(leftContainer);
        DontDestroyOnLoad(rightContainer);
        Debug.Log("Lade nächste Szene: " + nextSceneName);
        SceneManager.LoadScene(nextSceneName);
    }

}
