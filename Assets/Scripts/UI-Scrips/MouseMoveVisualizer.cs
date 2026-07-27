using UnityEngine;
using UnityEngine.UI;

[RequireComponent(typeof(Image))]
public class MouseMoveVisualizer : MonoBehaviour
{
    [Header("Die beiden Grafiken")]
    public Sprite defaultSprite; // Maus steht still
    public Sprite movingSprite;  // Maus bewegt sich (z. B. mit Bewegungslinien)

    [Header("Einstellungen")]
    [Tooltip("Wie lange bleibt das Bewegt-Bild noch sichtbar, wenn die Maus stoppt? Verhindert Flackern.")]
    public float smoothCooldown = 0.15f;

    private Image uiImage;
    private float stopTimer = 0f;

    void Awake()
    {
        uiImage = GetComponent<Image>();

        if (defaultSprite != null)
        {
            uiImage.sprite = defaultSprite;
        }
    }

    void Update()
    {
        // Wir prüfen, ob die Maus auf der X- oder Y-Achse verschoben wird
        float moveX = Mathf.Abs(Input.GetAxis("Mouse X"));
        float moveY = Mathf.Abs(Input.GetAxis("Mouse Y"));

        // Wenn die Bewegung groß genug ist (größer als 0)
        if (moveX > 0.05f || moveY > 0.05f)
        {
            if (movingSprite != null) uiImage.sprite = movingSprite;

            // Timer immer wieder voll aufladen, solange bewegt wird
            stopTimer = smoothCooldown;
        }
        else
        {
            // Maus steht still -> Timer läuft rückwärts
            if (stopTimer > 0)
            {
                stopTimer -= Time.deltaTime;
            }
            else
            {
                // Timer ist abgelaufen, Zeit für das Standard-Bild
                if (defaultSprite != null) uiImage.sprite = defaultSprite;
            }
        }
    }
}