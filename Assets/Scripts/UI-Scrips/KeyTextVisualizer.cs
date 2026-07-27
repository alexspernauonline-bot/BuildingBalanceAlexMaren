using UnityEngine;
using TMPro; 

[RequireComponent(typeof(TextMeshProUGUI))]
public class KeyTextVisualizer : MonoBehaviour
{
    [Header("Welche Taste löst es aus?")]
    public KeyCode keyToPress = KeyCode.Mouse0; // Direkt auf Links-Klick voreingestellt

    [Header("Die beiden Texte")]
    public string defaultText = "Block aufheben";
    public string pressedText = "Block ablegen";

    private TextMeshProUGUI uiText;

    void Awake()
    {
        uiText = GetComponent<TextMeshProUGUI>();

        // Zur Sicherheit den Starttext direkt eintragen
        uiText.text = defaultText;
    }

    void Update()
    {
        // Wenn die Taste gedrückt wird
        if (Input.GetKeyDown(keyToPress))
        {
            uiText.text = pressedText;
        }
        // Wenn die Taste wieder losgelassen wird
        else if (Input.GetKeyUp(keyToPress))
        {
            uiText.text = defaultText;
        }
    }
}