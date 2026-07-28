using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;
using TMPro;
public class GameManager : MonoBehaviour

{
    // Das Material, das die Farben von der L�sung verdeckt
    [SerializeField] private Material mysteryMaterial;

    // Das ist das Singleton. Damit k�nnen alle anderen Skripte diesen Manager finden.
    public static GameManager Instance;


    // Hier speichern wir das fertige Ergebnis f�r diese Runde
    public Material lightMaterial;
    public Material mediumMaterial;
    public Material heavyMaterial;

    //Aktuelle Gewichte der Waagschalen
    public float weightLeft = 0f;
    public float weightRight = 0f;
    // Referenzen zu den Waagschalen, damit wir sie sp�ter ansprechen k�nnen
    public ScaleSide leftScale;
    public ScaleSide rightScale;

    //R�tsel-Einstellungen
    public float tolerance = 1f; // Erlaubte Abweichung (z.B. falls die Physik leicht zittert)

    //Tower Transition Manager und Blink Animation
    public ScaleVisualizer leftVisualizer;
    public ScaleVisualizer rightVisualizer;
    public TowerTransitionManager transitionManager;
    // Wie lange die Waage im Gleichgewicht bleiben muss timer
    public float requiredBalanceTime = 3.0f;
    // Level Ende UI
    public GameObject levelEndPromptPanel;
    public TextMeshProUGUI scoreText;
    private bool hasLevelEnded = false;
    private bool waitForUnbalance = false;
    //Gewinnton
    public AudioClip winSound;
    private AudioSource gameManagerAudioSource;
    //Win particle effect
    public ParticleSystem explosionParticles;

    // Timer start
    private float currentBalanceTime = 0f;
    private bool isTransitioning = false;

    private bool isBalanced = false;

    // Awake wird noch VOR Start() aufgerufen bevor die Blöcke spawnen.
    void Awake()
    {
        // Singleton initialisieren
        if (Instance == null)
        {
            Instance = this;
        }
        AssignRandomColors();

        gameManagerAudioSource = GetComponent<AudioSource>();

        //Mögliche gespeicherte Lautstärke-Preferenzen
        float volume = PlayerPrefs.GetFloat("Volume", 1f);
        AudioListener.volume = volume;
    }

   
    void Update()
    {
        CheckBalance();
    }

    public void AssignRandomColors()
    {
        // Wir packen alle drei Materialien in eine Liste 
        List<Material> availableMaterials = new List<Material>
        {
            Resources.Load<Material>("redMaterial"),
            Resources.Load<Material>("blueMaterial"),
            Resources.Load<Material>("greenMaterial")
        };

        //  ziehen einer zufälligen Zahl für das LEICHTE Gewicht
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


    // ACHTUNG: Das 'private' wurde entfernt, damit der SpawnManager zugreifen kann
    public void SetupTower(GameObject towerObject)
    {
        // LOG 1: Prüfen, ob die Methode überhaupt ankommt
        Debug.Log("SetupTower wurde aufgerufen für: " + towerObject.name);
        // Sucht ALLE Blöcke im gesamten Turm auf einmal zusammen
        BlockIdentifier[] allBlocksInTower = towerObject.GetComponentsInChildren<BlockIdentifier>();
        // LOG 2: Prüfen, wie viele Blöcke überhaupt gefunden werden
        Debug.Log("Gefundene Blöcke im Turm: " + allBlocksInTower.Length);

        foreach (BlockIdentifier block in allBlocksInTower)
        {
            MeshRenderer renderer = block.GetComponentInChildren<MeshRenderer>(true);
            Rigidbody rb = block.GetComponent<Rigidbody>();
            // --- NEU: DER GAMEMANAGER WÜRFELT JETZT DIE GEWICHTE! ---
            int randomWeight = Random.Range(0, 3); // Zieht eine Zahl: 0, 1 oder 2
            block.myCategory = (BlockWeightCategory)randomWeight; // Weist Light, Medium oder Heavy zu

            // Zur Sicherheit: Falls deine Waage noch auf das alte ColorWeight-Skript schaut
            ColorWeight cw = block.GetComponent<ColorWeight>();
            if (cw != null)
            {
                cw.myWeightType = (ColorWeight.BlockType)randomWeight;
            }

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
            // 2. Fehler-Check für den Renderer
            if (renderer == null)
            {
                Debug.LogWarning($"[WARNUNG] Auf dem Block '{block.gameObject.name}' wurde KEIN MeshRenderer gefunden!");
                continue; // Springt zum nächsten Block
            }

            // 3. Lösung mit Versteck-Material übermalen
            if (mysteryMaterial != null)
            {
                renderer.material = mysteryMaterial;
                Debug.Log($"Block '{block.gameObject.name}' wurde erfolgreich grau übermalt!");
            }
            else
            {
                Debug.LogError("[FEHLER] mysteryMaterial ist NULL auf dem GameManager!");
            }
            
        }
    }

    // Das Placement-System �bergibt hier den frisch gespawnten Block und dessen gew�nschte Gewichtsklasse
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

        // Wenn sich die Masse �ndert diesen Block nicht mehr schlafen zu lassen, damit die Waage den neuen Druck sp�rt.
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
        if (difference > tolerance)
        {
            waitForUnbalance = false;
        }
        // Wenn die Sperre noch aktiv ist, brechen wir hier für diesen Frame ab
        if (waitForUnbalance)
        {
            return;
        }
        // Wenn die Differenz innerhalb unserer Toleranz liegt UND �berhaupt Gewicht draufliegt
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
                explosionParticles.Play();
                isTransitioning = true;
                if (leftVisualizer != null) leftVisualizer.UpdateBlink(false, 0f);
                if (rightVisualizer != null) rightVisualizer.UpdateBlink(false, 0f);
                Debug.Log("DIE WAAGE IST FÜR 3 SEKUNDEN BALANCIERT!");

                // Hier werden  die Punkte gespeichert, bevor  die Szene gewechselt wird!
                CalculateAndSaveAccuracy(difference);

                //Jetzt werden die blöcke dem Szenenübergang gegeben, damit der spieler den Selbstgebauten Turm balancieren kann.
                if (!hasLevelEnded)
                {
                    //Der spieler hat die Waage erfolgreich balanciert. Jetzt kann er entscheiden, ob er weiterbauen oder in die nächste Szene wechseln möchte.
                    hasLevelEnded = true;
                    if (levelEndPromptPanel != null)
                    {
                        levelEndPromptPanel.SetActive(true);
                        //SCORE AUSLESEN UND ANZEIGEN ---
                        if (scoreText != null)
                        {
                            //CalculateAndSaveAccuracy 
                            float currentScore = PlayerPrefs.GetFloat("TowerAccuracy", 0f);

                            // screibt den Score in HUD (F1 rundet auf 1 Nachkommastelle, z.B. "85.5")
                            scoreText.text = "Genauigkeit: " + currentScore.ToString("F1") + " %";
                        }
                    }
                }
                // HIER kommt später der Aufruf für deine Tür-Cutscene rein!
                // z.B. GetComponent<PlayableDirector>().Play();
            }
        }
        else
        {
            if (currentBalanceTime > 0)
            {
                isBalanced = false;
                explosionParticles.Stop();
                currentBalanceTime = 0f;

                if (leftVisualizer != null) leftVisualizer.UpdateBlink(false, 0f);
                if (rightVisualizer != null) rightVisualizer.UpdateBlink(false, 0f);

                Debug.Log("Waage aus dem Gleichgewicht. Timer resettet.");
            }
        }
    }
    //Wie der Score berechnet wird
    private void CalculateAndSaveAccuracy(float currentDifference)
    {
        // A. GEWICHTS-GENAUIGKEIT
        float weightAccuracy = 100f - ((currentDifference / tolerance) * 100f);
        weightAccuracy = Mathf.Clamp(weightAccuracy, 0f, 100f);

        // B. VORGABE-BLÖCKE ZÄHLEN
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

        // C. SPIELER-BLÖCKE ZÄHLEN
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

        Debug.Log($"Punkte berechnet! Gewicht: {weightAccuracy}% | Blöcke: {blockAccuracy}% | Gesamt: {finalAccuracyPercentage}%");
    }

    //UI 

    public void ResetGame()
    {
        // Setzt die gespeicherten Punkte auf 0
        PlayerPrefs.SetFloat("TowerAccuracy", 0f);
        PlayerPrefs.Save();
        SceneManager.LoadScene(SceneManager.GetActiveScene().name);
    }
    public void BackToMenu()
    {
        SceneManager.LoadScene("Menu");
    }
    // Wird vom "Weiterbauen"-Button aufgerufen
    public void ResetEndPromptState()
    {
        hasLevelEnded = false;
        isBalanced = false;      // Gibt die Waage wieder frei
        isTransitioning = false; // Hebt die Blockade für CheckBalance auf
        currentBalanceTime = 0f; // Setzt den Timer zurück
        waitForUnbalance = true; // Sperrt CheckBalance, bis die Waage wieder aus dem Gleichgewicht ist nachem der spieler auf "Weiterbauen" geklickt hat.
        if (levelEndPromptPanel != null)
        {
            levelEndPromptPanel.SetActive(false); // Schaltet das Menü wieder AUS
        }
        Debug.Log("Spieler baut weiter! Waage misst wieder.");
    }

    // Wird vom "Nächste Szene"-Button aufgerufen
    public void ExecuteSceneTransition()
    {
        // Holt deinen auskommentierten Code von oben nach und führt ihn jetzt aus!
        if (transitionManager != null)
        {
            Debug.Log("Spieler ist fertig. Tower Transition Manager übernimmt!");
            transitionManager.TransferTowerAndLoadScene();
        }
    }
}