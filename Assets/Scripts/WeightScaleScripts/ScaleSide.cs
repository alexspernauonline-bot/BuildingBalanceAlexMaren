using UnityEngine;

public class ScaleSide : MonoBehaviour
{
    
    //Speichert das Gesamtgewicht, das sich derzeit auf dieser Seite befindet.
    public float currentWeight = 0f;

    
    //Läuft automatisch, wenn ein anderes Objekt in die Trigger-Zone eintritt.
    private void OnTriggerEnter(Collider other)
    {
        
        //Checkt ob das Objekt, das in die Zone eingetreten ist, eine Rigidbody-Komponente (Physik) hat.
        Rigidbody rb = other.GetComponent<Rigidbody>();

        // If 'rb' is NOT null, it means we found a Rigidbody.
        // Wenn rb nicht null ist, bedeutet das eine Rigidbody-Komponente wurde gefunden.
        if (rb != null)
        {
            
            //Masse dieses Objekts und addierert zu dem Gesamtgewicht.
            currentWeight += rb.mass;
        }
    }

    // Läuft automatisch, wenn ein Objekt aus der Trigger-Zone herausfällt oder entfernt wird.
    private void OnTriggerExit(Collider other)
    {
        Rigidbody rb = other.GetComponent<Rigidbody>();

        if (rb != null)
        {
            // Wir subtrahieren die Masse, weil das Objekt nicht mehr auf der Waage liegt.
            currentWeight -= rb.mass;

            // Das ist eine Sicherheitsmaßnahme. Physik-Engines können manchmal leicht ungenau sein.
            // Mathf.Max stellt sicher, dass das Gewicht unserer Waage niemals versehentlich unter 0 fällt.
            currentWeight = Mathf.Max(0f, currentWeight);
        }
    }
}
