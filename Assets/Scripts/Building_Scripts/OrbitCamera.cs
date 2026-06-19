using UnityEngine;

public class OrbitCamera : MonoBehaviour
{
    // Fokus-Ankerpunkt (Zieh hier ein leeres Objekt in der Mitte der Waage rein)
    [SerializeField] private Transform target;

    // Tastatur-Einstellungen
    [SerializeField] private float rotationSpeed = 80f; // Grad pro Sekunde
    [SerializeField] private float zoomSpeed = 5f;
    [SerializeField] private float minZoom = 3f;
    [SerializeField] private float maxZoom = 15f;

    // Winkel-Begrenzung
    [SerializeField] private float minVerticalAngle = 10f; // Nicht ganz flach auf den Boden
    [SerializeField] private float maxVerticalAngle = 80f; // Nicht exakt senkrecht von oben

    private float currentX = 0f;  // Horizontale Rotation (A/D)
    private float currentY = 30f; // Vertikale Rotation (W/S) - Startwinkel
    private float currentDistance = 8f; // Start-Abstand

    void Start()
    {
        if (target == null)
        {
            GameObject scale = GameObject.Find("BuildingBalance1");
            if (scale != null) target = scale.transform;
        }

        // Initiale Rotation auslesen, damit die Kamera nicht springt
        Vector3 angles = transform.eulerAngles;
        currentX = angles.y;
    }

    void LateUpdate()
    {
        if (target == null) return;

        // Horizontale Rotation über A/D
        currentX -= Input.GetAxis("Horizontal") * rotationSpeed * Time.deltaTime;

        // Vertikale Rotation über W/S (Invertiert durch das Pluszeichen)
        currentY += Input.GetAxis("Vertical") * rotationSpeed * Time.deltaTime;

        // Begrenzen, damit die Kamera nicht unter den Boden oder über Kopf fliegt
        currentY = Mathf.Clamp(currentY, minVerticalAngle, maxVerticalAngle);

        // Zoom über das Mausrad
        currentDistance -= Input.GetAxis("Mouse ScrollWheel") * zoomSpeed;
        currentDistance = Mathf.Clamp(currentDistance, minZoom, maxZoom);

        // Positionierung um den Ankerpunkt rechnen
        Quaternion rotation = Quaternion.Euler(currentY, currentX, 0f);
        Vector3 direction = new Vector3(0, 0, -currentDistance);

        // Kamera bewegen und starr auf den Ankerpunkt ausrichten
        transform.position = target.position + (rotation * direction);
        transform.LookAt(target.position);
    }
}