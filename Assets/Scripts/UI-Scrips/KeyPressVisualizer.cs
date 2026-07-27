using UnityEngine;
using UnityEngine.UI; 

// Erzwingt, dass das Objekt auch wirklich eine Image-Komponente hat
[RequireComponent(typeof(Image))]
public class KeyPressVisualizer : MonoBehaviour
{
    [Header("Welche Taste soll gedrückt werden?")]
    public KeyCode keyToPress; // Erzeugt ein Dropdown im Inspector für ALLE Tasten

    [Header("Die beiden Grafiken")]
    public Sprite defaultSprite; // Normaler Zustand
    public Sprite pressedSprite; // Gedrückter Zustand

    private Image uiImage;

    void Awake()
    {
        // Holt sich automatisch das Image-Bauteil von diesem UI-Element
        uiImage = GetComponent<Image>();

        // Startbild zur Sicherheit setzen
        if (defaultSprite != null)
        {
            uiImage.sprite = defaultSprite;
        }
    }

    void Update()
    {
        // Wenn die Taste in exakt diesem Frame nach unten gedrückt wird
        if (Input.GetKeyDown(keyToPress))
        {
            if (pressedSprite != null) uiImage.sprite = pressedSprite;
        }
        // Wenn die Taste wieder losgelassen wird
        else if (Input.GetKeyUp(keyToPress))
        {
            if (defaultSprite != null) uiImage.sprite = defaultSprite;
        }
    }
}