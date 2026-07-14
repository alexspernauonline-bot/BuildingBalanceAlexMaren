using System.Collections.Generic;
using UnityEngine;

public class GameManager : MonoBehaviour
{
    //Nur zum Testen von Turm Spawns 
    [SerializeField] private bool testModeSequential = false;

    // speichern von Prefabs für die verschiedenen Rätsel-Türme
    [SerializeField] private GameObject[] puzzlePrefabs;
    //Lösungs Turm SpawnPunkt
    [SerializeField] private Transform towerSpawnArea;

    // Das Material, das die Farben von der Lösung verdeckt
    [SerializeField] private Material mysteryMaterial;

    //Blöcke zum platzieren für den Spieler
    [SerializeField] private GameObject[] playerBlockPrefabs; // Das Prefab für den greifbaren Block
    [SerializeField] private BoxCollider spawnVolume; // Wo die Blöcke hinfallen sollen

    [SerializeField] private int amountOfBlocksToSpawn = 8;  // Wie viele Blöcke spawnen sollen


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
        //Nur fürs Testen 
        int towerIndex = 0;

        //Prüfen ob Test Modus an ist
        if (testModeSequential == true)
        {
            //TEST-MODUS
            towerIndex = PlayerPrefs.GetInt("TowerIndex", 0);

            int nextIndex = towerIndex + 1;
            if (nextIndex >= puzzlePrefabs.Length) nextIndex = 0;

            PlayerPrefs.SetInt("TowerIndex", nextIndex);
            PlayerPrefs.Save();

            Debug.Log(" TEST-MODUS AKTIV: Spawne Turm Nr. " + towerIndex);
        }
        else
        {
            //SPIELER-MODUS (Zufall)
            towerIndex = Random.Range(0, puzzlePrefabs.Length);
            Debug.Log(" SPIELER-MODUS AKTIV: Spawne zufälligen Turm Nr. " + towerIndex);
        }

        // Den kompletten Turm am Spawn-Punkt erschaffen
        // "towerIndex", damit er das Ergebnis von oben nutzt!
        GameObject spawnedTower = Instantiate(puzzlePrefabs[towerIndex], towerSpawnArea.position, towerSpawnArea.rotation);

        // Dem neuen Turm seine Gewichte und Farben zuweisen
        SetupTower(spawnedTower);
        // Spawnt den Vorrat an Spieler-Blöcken
        if (spawnVolume != null)
        {
            Bounds bounds = spawnVolume.bounds;

            for (int i = 0; i < amountOfBlocksToSpawn; i++)
            {
                int shapeIndex;
                if (i < playerBlockPrefabs.Length)
                {
                    // Die ersten Blöcke gehen strikt die Liste durch (0, 1, 2...)
                    shapeIndex = i;
                }
                else
                {
                    // Alle restlichen Blöcke werden zufällig aufgefüllt
                    shapeIndex = Random.Range(0, playerBlockPrefabs.Length);
                }

                // Zufällige Position exakt innerhalb der Grenzen des BoxColliders suchen
                float randomX = Random.Range(bounds.min.x, bounds.max.x);
                float fixedY = spawnVolume.transform.position.y + (i * 0.5f);
                float randomZ = Random.Range(bounds.min.z, bounds.max.z);

                Vector3 spawnPos = new Vector3(randomX, fixedY, randomZ);

                // Random.Range(0, 4) würfelt eine 0, 1, 2 oder 3. 
                // Multipliziert mit 90 ergibt das exakt: 0, 90, 180 oder 270.
                float rotX = Random.Range(0, 4) * 90f;
                float rotY = Random.Range(0, 4) * 90f;
                float rotZ = Random.Range(0, 4) * 90f;
                Quaternion startRotation = Quaternion.Euler(rotX, rotY, rotZ);

                // Block erschaffen (mit zufälliger Start-Drehung, damit es natürlicher wirkt!)
                GameObject newBlock = Instantiate(playerBlockPrefabs[shapeIndex], spawnPos, startRotation);

                int layerZahl = LayerMask.NameToLayer("placableBlock");
                newBlock.layer = layerZahl;
                foreach (Transform child in newBlock.GetComponentsInChildren<Transform>(true))
                {
                    child.gameObject.layer = layerZahl;
                }

                // Das Skript hinzufügen, das die Gewichtskategorie wechseln kann
                PlayerBlockSwitcher switcher = newBlock.AddComponent<PlayerBlockSwitcher>();

                // welches Gewicht der Block in seinem Prefab gespeichert
                BlockIdentifier identifier = newBlock.GetComponent<BlockIdentifier>();
                if (identifier != null)
                {
                    // Wir stellen den Schalter auf das Original-Gewicht ein
                    switcher.currentCategory = identifier.myCategory;

                    // Wir konfigurieren Farbe und Masse basierend auf diesem Original-Gewicht!
                    ConfigurePlayerBlock(newBlock, identifier.myCategory);
                }
                else
                {
                    // Nur zur Sicherheit, falls mal ein Ausweis fehlt
                    ConfigurePlayerBlock(newBlock, BlockWeightCategory.Light);
                }
            }
        }
        else
        {
            Debug.LogError("Fehler: Du hast keinen BoxCollider in das Feld 'Spawn Volume' gezogen!");
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
            //Lösung wird mit grau übermalt
            if (renderer != null && mysteryMaterial != null)
            {
                renderer.material = mysteryMaterial;
            }
          
        }
    }

    // Das Placement-System übergibt hier den frisch gespawnten Block und dessen gewünschte Gewichtsklasse
    public void ConfigurePlayerBlock(GameObject playerBlock, BlockWeightCategory category)
    {
        MeshRenderer renderer = playerBlock.GetComponentInChildren<MeshRenderer>();
        Rigidbody rb = playerBlock.GetComponent<Rigidbody>();

        // WICHTIG: Das Spieler-Skript (z.B. für die Waage) braucht evtl. auch den Identifier
        BlockIdentifier identifier = playerBlock.GetComponent<BlockIdentifier>();
        if (identifier != null)
        {
            identifier.myCategory = category;
        }

        if (category == BlockWeightCategory.Light)
        {
            if (renderer != null) renderer.material = lightMaterial;
            if (rb != null) rb.mass = 1f; // Muss exakt denselben Wert haben wie in SetupTower!
        }
        else if (category == BlockWeightCategory.Medium)
        {
            if (renderer != null) renderer.material = mediumMaterial;
            if (rb != null) rb.mass = 5f;
        }
        else if (category == BlockWeightCategory.Heavy)
        {
            if (renderer != null) renderer.material = heavyMaterial;
            if (rb != null) rb.mass = 10f;
        }
        // Wenn sich die Masse ändert diesen Block nicht mehr schlafen zu lassen, damit die Waage den neuen Druck spürt.
        if (rb != null)
        {
            rb.WakeUp();
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
