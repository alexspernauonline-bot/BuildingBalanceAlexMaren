using UnityEngine;

public class SpawnManager : MonoBehaviour
{
    //Nur zum Testen von Turm Spawns 
    [SerializeField] private bool testModeSequential = false;

    // speichern von Prefabs für die verschiedenen Rätsel-Türme
    [SerializeField] private GameObject[] puzzlePrefabs;
    //Lösungs Turm SpawnPunkt
    [SerializeField] private Transform towerSpawnArea;

    //Blöcke zum platzieren für den Spieler
    [SerializeField] private GameObject[] playerBlockPrefabs; // Das Prefab für den greifbaren Block
    [SerializeField] private BoxCollider spawnVolume; // Wo die Blöcke hinfallen sollen

    [SerializeField] private int amountOfBlocksToSpawn = 8;  // Wie viele Blöcke spawnen sollen


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

        // Dem neuen Turm seine Gewichte und Farben zuweisen (Hier rufen wir den GameManager an!)
        GameManager.Instance.SetupTower(spawnedTower);

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
                    GameManager.Instance.ConfigurePlayerBlock(newBlock, identifier.myCategory);
                }
                else
                {
                    // Nur zur Sicherheit, falls mal ein Ausweis fehlt
                    GameManager.Instance.ConfigurePlayerBlock(newBlock, BlockWeightCategory.Light);
                }
            }
        }
        else
        {
            Debug.LogError("Fehler: Du hast keinen BoxCollider in das Feld 'Spawn Volume' gezogen!");
        }
    }
}