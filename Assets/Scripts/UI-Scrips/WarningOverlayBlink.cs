using UnityEngine;
using System.Collections;

public class WarningOverlayBlink : MonoBehaviour
{
    public PlayerTowerControl towerSkript;
    public GameObject warningScreen;

    private float blinkTimer;

    // Start is called once before the first execution of Update after the MonoBehaviour is created
    void Start()
    {
        towerSkript = GameObject.FindWithTag("PlayerTower").GetComponent<PlayerTowerControl>();
    }

    // Update is called once per frame
    void Update()
    {
        //Überprüfe, ob Turm wackelt
        if (towerSkript.isWobbly)
        {
            Debug.Log("Wackelt!");

            //Timer für den Blink-Effekt
            blinkTimer += Time.deltaTime;

            //Blink-Effekt
            if(blinkTimer >= 1f)
            {
                Debug.Log("Sollte blinken!");

                blinkTimer = 0f;
                warningScreen.SetActive(!warningScreen.activeSelf);
            }
        }
        else
        {
            //Warning Screen aus
            blinkTimer = 0f;
            warningScreen.SetActive(false);
        }
    }
}
