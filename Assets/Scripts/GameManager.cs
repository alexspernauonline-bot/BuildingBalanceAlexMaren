using UnityEngine;

public class GameManager : MonoBehaviour
{
    //VORSICHT ! Dieser Skript ist noch nicht aktiv. Erst Implimentieren wenn die Platzierlogik existiert.
    /*
    // Das ist das Singleton. Damit können alle anderen Skripte diesen Manager finden.
    public static RoundManager Instance;

    // Hier speichern wir das fertige Ergebnis für diese Runde
    public Material lightMaterial;
    public Material mediumMaterial;
    public Material heavyMaterial;

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
    // Start is called once before the first execution of Update after the MonoBehaviour is created
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        
    }
    public void AssignRandomColors()
    {
        // 1. Wir packen alle drei Materialien in eine Liste (unseren "Beutel")
        // Beachte: Hier nutze ich wieder Resources.Load, wie wir es vorher besprochen haben.
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
    */
}
