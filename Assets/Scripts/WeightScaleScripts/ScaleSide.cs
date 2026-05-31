using UnityEngine;

public class ScaleSide : MonoBehaviour
{
    
    //Speichert das Gesamtgewicht, das sich derzeit auf dieser Seite befindet.
    public float currentWeight = 0f;

 
    private void OnCollisionEnter(Collision collision)
    {
        // Bei OnCollisionEnter holen wir uns den Rigidbody über collision.gameObject
        Rigidbody rb = collision.gameObject.GetComponent<Rigidbody>();

        if (rb != null)
        {
            // Masse dieses Objekts addieren
            currentWeight += rb.mass;
            Debug.Log(gameObject.name + " Gewicht erhöht auf: " + currentWeight);
        }
    }

    // Läuft automatisch, wenn ein Objekt von der Waage herabfällt oder weggenommen wird
    private void OnCollisionExit(Collision collision)
    {
        Rigidbody rb = collision.gameObject.GetComponent<Rigidbody>();

        if (rb != null)
        {
            // Wir subtrahieren die Masse
            currentWeight -= rb.mass;

            // Sicherheitsmaßnahme gegen Rundungsfehler
            currentWeight = Mathf.Max(0f, currentWeight);
            Debug.Log(gameObject.name + " Gewicht verringert auf: " + currentWeight);
        }
    }
}
