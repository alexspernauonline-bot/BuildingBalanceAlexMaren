using UnityEngine;
using TMPro;
public class LiveTipps : MonoBehaviour
{
    //Dropdown damit dieses Skipt für beide Szenen genutzt werden kann
    public enum TutorialMode { Level1_Bauen, Level2_Bewegung }
    // Aktueller Modus, der im Inspector eingestellt werden kann
    public TutorialMode currentMode;
    public GameObject tutorialPanel; // Das Hintergrund-Panel für die Tipps
    public TextMeshProUGUI tutorialText; // Der Text selbst
    public bool hasResetBlock = false;

    private int tutorialStep = 0; // Merkt sich, wo wir im Tutorial sind
    private string saveKey = "";//merkt sich den Key für PlayerPrefs, damit wir wissen, ob das Tutorial schon abgeschlossen wurde
    void Start()
    {
        // 1. Stempel-Namen festlegen, je nachdem, in welchem Level wir sind
        if (currentMode == TutorialMode.Level1_Bauen)
            saveKey = "Tut_Level1_Done";
        else
            saveKey = "Tut_Level2_Done";

        // 2. Prüfen, ob DIESES Tutorial schon abgeschlossen wurde
        if (PlayerPrefs.GetInt(saveKey, 0) == 1)
        {
            if (tutorialPanel != null) tutorialPanel.SetActive(false);
            tutorialStep = 99; // 99 bedeutet: Wir sind fertig, Update-Schleife ignorieren!
            return;
        }

        // 3. Wenn nicht: Panel an und den ERSTEN Text für das jeweilige Level setzen
        if (tutorialPanel != null) tutorialPanel.SetActive(true);

        if (currentMode == TutorialMode.Level1_Bauen)
        {
            if (tutorialText != null) tutorialText.text = "Maus [ Links-Klick ] um einen Block zu greifen";
        }
        else // Level 2
        {
            if (tutorialText != null) tutorialText.text = "Nutze [ W A S D ] um dich zu bewegen";
        }
    }

    void Update()
    {
        // Wenn Tutorial durch ist, mach hier gar nichts mehr
        if (tutorialStep == 99) return;
        // LOGIK FÜR LEVEL 1 (Nur Maus & Rotieren)
        if (currentMode == TutorialMode.Level1_Bauen)
        {
            if (tutorialStep == 0) // Warten auf Linksklick
            {
                if (Input.GetMouseButton(0))
                {
                    tutorialStep = 1;
                    tutorialText.text = "Drücke [ R ] um den Block zu rotieren";
                }
            }
            if (tutorialStep == 1) // Warten auf R
            {
                if (Input.GetKeyUp(KeyCode.R))
                {
                    tutorialStep = 2;
                    tutorialText.text = "Lege den Block in die Reset Area um ihn zurückzusetzen";
                }
            }
            else if (tutorialStep == 2) // Warten auf Warten auf Reset Area
            {
                if (hasResetBlock == true)
                {
                    tutorialStep = 99;
                    tutorialText.text = "Perfekt!";
                    Invoke("HideTutorial", 2f); // Verschwindet nach 2 Sekunden
                }
            }
        }
        // LOGIK FÜR LEVEL 2 (Bewegung & Springen)
        else if (currentMode == TutorialMode.Level2_Bewegung)
        {
            if (tutorialStep == 0) // Warten auf WASD
            {
                if (Input.GetKeyDown(KeyCode.W) || Input.GetKeyDown(KeyCode.A) ||
                    Input.GetKeyDown(KeyCode.S) || Input.GetKeyDown(KeyCode.D))
                {
                    tutorialStep = 1;
                    tutorialText.text = "[ Space ] zum Springen, [ Shift ] zum Sprinten, [ Strg ] zum Schleichen";
                }
            }
            else if (tutorialStep == 1) // Warten auf Space oder Shift
            {
                if (Input.GetKeyDown(KeyCode.Space) || Input.GetKeyDown(KeyCode.LeftShift))
                {
                    tutorialStep = 99;
                    tutorialText.text = "Los geht's!";
                    Invoke("HideTutorial", 2f);
                }
            }
        }
    }

    void HideTutorial()
    {
        if (tutorialPanel != null) tutorialPanel.SetActive(false);

        // Stempel aufdrücken, damit es nicht mehr auftaucht!
        PlayerPrefs.SetInt(saveKey, 1);
        PlayerPrefs.Save();
    }
}
