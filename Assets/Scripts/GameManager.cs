using System.Collections.Generic;
using UnityEngine;

public class GameManager : MonoBehaviour

{
    // Das Material, das die Farben von der Lï¿½sung verdeckt
    [SerializeField] private Material mysteryMaterial;

    // Das ist das Singleton. Damit kï¿½nnen alle anderen Skripte diesen Manager finden.
    public static GameManager Instance;
   

    // Hier speichern wir das fertige Ergebnis fï¿½r diese Runde
    public Material lightMaterial;
    public Material mediumMaterial;
    public Material heavyMaterial;

    //Aktuelle Gewichte der Waagschalen
    public float weightLeft = 0f;
    public float weightRight = 0f;
    // Referenzen zu den Waagschalen, damit wir sie spï¿½ter ansprechen kï¿½nnen
    public ScaleSide leftScale;
    public ScaleSide rightScale;

    //Rï¿½tsel-Einstellungen
    public float tolerance = 0.1f; // Erlaubte Abweichung (z.B. falls die Physik leicht zittert)

    //Tower Transition Manager und Blink Animation
    public ScaleVisualizer leftVisualizer;
    public ScaleVisualizer rightVisualizer;
    public TowerTransitionManager transitionManager;
    // Wie lange die Waage im Gleichgewicht bleiben muss timer
    public float requiredBalanceTime = 3.0f;

    //Gewinnton
    public AudioClip winSound;
    private AudioSource gameManagerAudioSource;

    // Timer start
    private float currentBalanceTime = 0f;
    private bool isTransitioning = false;

    private bool isBalanced = false;

<<<<<<< Updated upstream
    // Awake wird noch VOR Start() aufgerufen. Perfekt, um die Regeln festzulegen,bevor die Blï¿½cke spawnen.
=======
    private float finishTime = 0;
    // Awake wird noch VOR Start() aufgerufen. Perfekt, um die Regeln festzulegen,bevor die Blöcke spawnen.
>>>>>>> Stashed changes
    void Awake()
    {
        // Singleton initialisieren
        if (Instance == null)
        {
            Instance = this;
        }
        AssignRandomColors();

        gameManagerAudioSource = GetComponent<AudioSource>();
    }

    // GEFIXT: Update-Methode hinzugefï¿½gt, damit die Waage jeden Frame geprï¿½ft wird
    void Update()
    {
        CheckBalance();
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

        // 2. Wir ziehen eine zufï¿½llige Zahl fï¿½r das LEICHTE Gewicht
        int randomIndex = Random.Range(0, availableMaterials.Count);
        lightMaterial = availableMaterials[randomIndex];
        availableMaterials.RemoveAt(randomIndex); // Farbe aus dem Beutel nehmen!

        // 3. Wir ziehen aus den verbleibenden ZWEI Farben fï¿½r das MITTLERE Gewicht
        randomIndex = Random.Range(0, availableMaterials.Count);
        mediumMaterial = availableMaterials[randomIndex];
        availableMaterials.RemoveAt(randomIndex);

        // 4. Das SCHWERE Gewicht bekommt automatisch die letzte ï¿½brig gebliebene Farbe
        heavyMaterial = availableMaterials[0];

        Debug.Log("Neue Runde! Farben wurden frisch gemischt.");
    }

    //ex
    // ACHTUNG: Das 'private' wurde entfernt, damit der SpawnManager zugreifen kann
    public void SetupTower(GameObject towerObject)
    {
        // Sucht ALLE Blï¿½cke im gesamten Turm auf einmal zusammen
        BlockIdentifier[] allBlocksInTower = towerObject.GetComponentsInChildren<BlockIdentifier>();

        foreach (BlockIdentifier block in allBlocksInTower)
        {
            MeshRenderer renderer = block.GetComponentInChildren<MeshRenderer>();
            Rigidbody rb = block.GetComponent<Rigidbody>();

            // Prï¿½fen, welcher Ausweis vorgezeigt wird
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
            //Lï¿½sung wird mit grau ï¿½bermalt
            if (renderer != null && mysteryMaterial != null)
            {
                renderer.material = mysteryMaterial;
            }
        }
    }

    // Das Placement-System ï¿½bergibt hier den frisch gespawnten Block und dessen gewï¿½nschte Gewichtsklasse
    public void ConfigurePlayerBlock(GameObject playerBlock, BlockWeightCategory category)
    {
        MeshRenderer renderer = playerBlock.GetComponentInChildren<MeshRenderer>();
        Rigidbody rb = playerBlock.GetComponent<Rigidbody>();

        // WICHTIG: Das Spieler-Skript (z.B. fï¿½r die Waage) braucht evtl. auch den Identifier
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

        // Wenn sich die Masse ï¿½ndert diesen Block nicht mehr schlafen zu lassen, damit die Waage den neuen Druck spï¿½rt.
        if (rb != null)
        {
            rb.WakeUp();
        }
    }

    //ex
    public void CheckBalance()
    {
        // Sicherheits-Check: Sind beide Waagschalen im Inspector zugewiesen?
        if (leftScale == null || rightScale == null || isTransitioning) return;

        //Gewichte reinholen
        weightLeft = leftScale.currentWeight;
        weightRight = rightScale.currentWeight;

        // Wir berechnen die absolute Differenz zwischen links und rechts
        float difference = Mathf.Abs(weightLeft - weightRight);
        // Wenn die Differenz innerhalb unserer Toleranz liegt UND ï¿½berhaupt Gewicht draufliegt
        if (difference <= tolerance && weightLeft > tolerance && weightRight > tolerance)
        {
            currentBalanceTime += Time.deltaTime;

            //Blink starten 
            if (leftVisualizer != null) leftVisualizer.UpdateBlink(true, currentBalanceTime);
            if (rightVisualizer != null) rightVisualizer.UpdateBlink(true, currentBalanceTime);

            if (currentBalanceTime >= requiredBalanceTime && !isBalanced)
            {
                gameManagerAudioSource.PlayOneShot(winSound, 1);
                print("IsBalanced Starting Timer Sound");
                isBalanced = true;
                isTransitioning = true;
                if (leftVisualizer != null) leftVisualizer.UpdateBlink(false, 0f);
                if (rightVisualizer != null) rightVisualizer.UpdateBlink(false, 0f);
                Debug.Log("DIE WAAGE IST Fï¿½R 3 SEKUNDEN BALANCIERT!");

                // GEFIXT: Hier speichern wir jetzt die Punkte, bevor wir die Szene wechseln!
                CalculateAndSaveAccuracy(difference);

                //Jetzt werden die blï¿½cke dem Szenen ï¿½bergang gegeben, damit der spieler den Selbstgebauten Turm balancieren kann.
                if (transitionManager != null)
                {
                    transitionManager.TransferTowerAndLoadScene();
                }
                // HIER kommt spï¿½ter der Aufruf fï¿½r deine Tï¿½r-Cutscene rein!
                // z.B. GetComponent<PlayableDirector>().Play();
            }
        }
        else
        {
            if (currentBalanceTime > 0)
            {
                isBalanced = false;
                currentBalanceTime = 0f;

                if (leftVisualizer != null) leftVisualizer.UpdateBlink(false, 0f);
                if (rightVisualizer != null) rightVisualizer.UpdateBlink(false, 0f);

                Debug.Log("Waage aus dem Gleichgewicht. Timer resettet.");
            }
        }
    }

    private void CalculateAndSaveAccuracy(float currentDifference)
    {
        // A. GEWICHTS-GENAUIGKEIT
        float weightAccuracy = 100f - ((currentDifference / tolerance) * 100f);
        weightAccuracy = Mathf.Clamp(weightAccuracy, 0f, 100f);

        // B. VORGABE-BLï¿½CKE Zï¿½HLEN
        GameObject targetTower = GameObject.FindWithTag("TargetTower");
        int targetBlockCount = 0;

        if (targetTower != null)
        {
            targetBlockCount = targetTower.transform.childCount;
        }
        else
        {
            Debug.LogWarning("Vorgabe-Turm nicht gefunden! Hast du den Tag 'TargetTower' gesetzt?");
        }

        // C. SPIELER-BLï¿½CKE Zï¿½HLEN
        GameObject[] allTowerBlocks = GameObject.FindGameObjectsWithTag("TowerBlock");
        int playerBlockCount = 0;

        foreach (GameObject block in allTowerBlocks)
        {
            float distToLeft = Vector3.Distance(block.transform.position, leftScale.transform.position);
            float distToRight = Vector3.Distance(block.transform.position, rightScale.transform.position);

            if (distToRight < distToLeft)
            {
                playerBlockCount++;
            }
        }

        // D. ANZAHL-GENAUIGKEIT
        float blockAccuracy = 0f;
        if (targetBlockCount > 0)
        {
            float blockDifference = Mathf.Abs(targetBlockCount - playerBlockCount);
            blockAccuracy = 100f - ((blockDifference / targetBlockCount) * 100f);
            blockAccuracy = Mathf.Clamp(blockAccuracy, 0f, 100f);
        }

        // E. GESAMTPUNKTZAHL
        float finalAccuracyPercentage = (weightAccuracy + blockAccuracy) / 2f;

        PlayerPrefs.SetFloat("TowerAccuracy", finalAccuracyPercentage);
        PlayerPrefs.Save();

        Debug.Log($"Punkte berechnet! Gewicht: {weightAccuracy}% | Blï¿½cke: {blockAccuracy}% | Gesamt: {finalAccuracyPercentage}%");
    }

    //Ende des Spiels
    private void onTriggerEnter(Collider other)
    {
        if (other.CompareTag("Finish"))
        {
            FinishTime();
            print("Finish");
            print("finishTime");
        }
    }

    private void FinishTime()
    {
        finishTime = Time.time;
    }
}