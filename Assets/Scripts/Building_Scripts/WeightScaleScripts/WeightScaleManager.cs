using UnityEngine;
using System.Collections.Generic;

public class WeightScaleManager : MonoBehaviour
{
   //Zuweisungen von den Waagschalen im Unity Inspektor.
    
    public ScaleSide leftSide;
    public ScaleSide rightSide;

    // Es wird verfolgt, ob das Spiel bereits gewonnen wurde.
    private bool hasWon = false;
    // Update is called once every single frame of the game.
    void Update()
    {
        if (hasWon == true)
        {
            return;
        }
    }
    
    
    /*
    //Variablen 'left' und 'right' speichern die aktuellen Gewichte von beiden Seiten der Waage.
    float left = leftSide.currentWeight;
    float right = rightSide.currentWeight;
    // Vergleicht die Gewichte, um zu sehen, welche Seite schwerer ist.
    if (left > right)
    {
        Debug.Log("The LEFT side is heavier!");
    }
    else if (right > left)
    {
        Debug.Log("The RIGHT side is heavier!");
    }
    else // Wenn links NICHT größer als rechts ist, und rechts NICHT größer als links... müssen sie gleich sein!
    {
        // Sind sie gleich, weil sie perfekt ausbalanciert sind, oder weil sie leer sind?
        if (left > 0 && right > 0)
        {
            TriggerWinCondition();
        }
        else
        {
            Debug.Log("The scale is empty.");
        }
    }
    void TriggerWinCondition()
    {
        // Sperrt das Spiel, damit diese Funktion nicht erneut aufgerufen werden kann.
        hasWon = true;

        // Spieler hat gewonnen, hier die Belohnung oder den nächsten Schritt auslösen.
        Debug.Log(" YOU WIN! The scale is perfectly balanced! ");

        // Später können wir hier Code hinzufügen, um einen Sieges-Sound abzuspielen,
        // das nächste Level zu laden oder einen UI-Bildschirm anzuzeigen.
    }
    */
}


