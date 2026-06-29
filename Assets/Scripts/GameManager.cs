using System.Collections.Generic;
using UnityEngine;

public class GameManager : MonoBehaviour
{
    // Erzeugt eine Liste für die Renderer (die visuellen Hüllen) deiner Turm-Blöcke
    [SerializeField] private MeshRenderer[] towerRenderers;
    [SerializeField] private GameObject[] puzzlePrefabs;
    [SerializeField] private Transform spawnArea;

    // Das Material, das die Farben verstecken soll
    [SerializeField] private Material mysteryMaterial;

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
    void Start()
    {
        // 1. Zufällige Zahl zwischen 0 und 5 ziehen
        int randomIndex = Random.Range(0, puzzlePrefabs.Length);

        // 2. Den kompletten Turm am Spawn-Punkt erschaffen
        GameObject spawnedTower = Instantiate(puzzlePrefabs[randomIndex], spawnArea.position, spawnArea.rotation);

        // 3. Dem neuen Turm seine Gewichte und Farben zuweisen
        SetupTower(spawnedTower);
        // Wir setzen alle Turm-Blöcke auf das Mystery-Material, damit die Farben nicht sofort sichtbar sind
        foreach (MeshRenderer renderer in towerRenderers)
        {
            // So liest du das aktuelle Material aus (z.B. um es in einer Variablen zu speichern)
            Material originalMat = renderer.material;

            renderer.material = Resources.Load<Material>("mysteryMaterial");
        }
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
    //ex
    private void SetupTower(GameObject towerObject)
    {
        // Sucht ALLE Blöcke im gesamten Turm auf einmal zusammen
        BlockIdentifier[] allBlocksInTower = towerObject.GetComponentsInChildren<BlockIdentifier>();

        foreach (BlockIdentifier block in allBlocksInTower)
        {
            MeshRenderer renderer = block.GetComponentInChildren<MeshRenderer>();
            Rigidbody rb = block.GetComponent<Rigidbody>();

            // Prüfen, welcher Ausweis vorgezeigt wird
            if (block.myCategory == BlockWeightCategory.Light)
            {
                if (renderer != null) renderer.material = lightMaterial;
                if (rb != null) rb.mass = 1f; // Beispielgewicht
                block.trueMaterial = lightMaterial;
            }
            else if (block.myCategory == BlockWeightCategory.Medium)
            {
                if (renderer != null) renderer.material = mediumMaterial;
                if (rb != null) rb.mass = 5f;
                block.trueMaterial = mediumMaterial;
            }
            else if (block.myCategory == BlockWeightCategory.Heavy)
            {
                if (renderer != null) renderer.material = heavyMaterial;
                if (rb != null) rb.mass = 10f;
                block.trueMaterial = heavyMaterial;
            }

            // HIER könntest du jetzt als letzten Schritt das Material des Renderers 
            // direkt wieder mit deinem "mysteryMaterial" (z.B. Grau) überschreiben, 
            // damit der Spieler die Farbe nicht sieht!
        }
    }
    //ex
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
