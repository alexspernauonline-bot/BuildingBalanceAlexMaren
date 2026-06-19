using System.Collections.Generic;
using UnityEngine;

public class GameManager : MonoBehaviour
{
    //VORSICHT ! Dieser Skript ist noch nicht aktiv. Erst Implimentieren wenn die Platzierlogik existiert.
  
    // Das ist das Singleton. Damit können alle anderen Skripte diesen Manager finden.
    public static GameManager Instance;

    // Hier speichern wir das fertige Ergebnis für diese Runde
    public Material lightMaterial;
    public Material mediumMaterial;
    public Material heavyMaterial;

    //Aktuelle Gewichte der Waagschalen
    public float weightLeft = 0f;
    public float weightRight = 0f;
    // Referenzen zu den Waagschalen, damit wir sie später ansprechen können
    public ScaleSide leftScale;
    public ScaleSide rightScale;
    //Rätsel-Einstellungen
    public float tolerance = 0.1f; // Erlaubte Abweichung (z.B. falls die Physik leicht zittert)
    private bool isBalanced = false;

    // Awake wird noch VOR Start() aufgerufen. Perfekt, um die Regeln festzulegen,bevor die Blöcke spawnen.
    void Awake()
    {
        // Singleton initialisieren
        if (Instance == null)
        {
            Instance = this;
        }

        AssignRandomColors();
    }
    public void AssignRandomColors()
    {
        // 1. Wir packen alle drei Materialien in eine Liste (unseren "Beutel")
        
        List<Material> availableMaterials = new List<Material>
        {
            Resources.Load<Material>("redMaterial"), 
            Resources.Load<Material>("blueMaterial"),
            Resources.Load<Material>("greenMaterial")
        };

        // 2. Wir ziehen eine zufällige Zahl für das LEICHTE Gewicht
        int randomIndex = Random.Range(0, availableMaterials.Count);
        lightMaterial = availableMaterials[randomIndex];
        availableMaterials.RemoveAt(randomIndex); // Farbe aus dem Beutel nehmen!

        // 3. Wir ziehen aus den verbleibenden ZWEI Farben für das MITTLERE Gewicht
        randomIndex = Random.Range(0, availableMaterials.Count);
        mediumMaterial = availableMaterials[randomIndex];
        availableMaterials.RemoveAt(randomIndex);

        // 4. Das SCHWERE Gewicht bekommt automatisch die letzte übrig gebliebene Farbe
        heavyMaterial = availableMaterials[0];

        Debug.Log("Neue Runde! Farben wurden frisch gemischt.");
    }
    public void CheckBalance()
    {
        // Sicherheits-Check: Sind beide Waagschalen im Inspector zugewiesen?
        if (leftScale == null || rightScale == null) return;
        //Gewichte reinholen
        weightLeft = leftScale.currentWeight;
        weightRight = rightScale.currentWeight;
        // Wir berechnen die absolute Differenz zwischen links und rechts
        float difference = Mathf.Abs(weightLeft - weightRight);

        // Wenn die Differenz innerhalb unserer Toleranz liegt UND überhaupt Gewicht draufliegt
        if (difference <= tolerance && (weightLeft > 0 || weightRight > 0))
        {
            if (!isBalanced)
            {
                isBalanced = true;
                Debug.Log(" DIE WAAGE IST RECHNERISCH BALANCIERT!");

                // HIER kommt später der Aufruf für deine Tür-Cutscene rein!
                // z.B. GetComponent<PlayableDirector>().Play();
            }
        }
        else
        {
            if (isBalanced)
            {
                isBalanced = false;
                Debug.Log(" Waage wieder aus dem Gleichgewicht geraten.");
            }
        }
    }
}
