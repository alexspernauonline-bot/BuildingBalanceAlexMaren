using UnityEngine;

public class MagneticSnap: MonoBehaviour
{
    private bool hasSnapped = false;
    private Rigidbody rb;

    void Awake()
    {
        rb = GetComponent<Rigidbody>();
    }

    private void OnCollisionEnter(Collision collision)
    {
        if (!hasSnapped && collision.gameObject.CompareTag("Block"))
        {
            if (collision.contactCount > 0)
            {
                Vector3 currentRotation = transform.rotation.eulerAngles;
                ContactPoint contact = collision.GetContact(0);

                if (contact.normal.y > 0.7f) 
                {
                    float snappedX = Mathf.Round(currentRotation.x / 90f) * 90f;
                    float snappedZ = Mathf.Round(currentRotation.z / 90f) * 90f;
                    float snappedY = currentRotation.y;

                    transform.rotation = Quaternion.Euler(snappedX, snappedY, snappedZ);

                    rb.constraints = RigidbodyConstraints.FreezeRotationX | RigidbodyConstraints.FreezeRotationZ;

                    rb.linearVelocity = Vector3.zero;
                    rb.angularVelocity = Vector3.zero;

                    hasSnapped = true;
                }
            }
        }
    }
    /*
    {
    private bool hasSnapped = false;
    private Rigidbody rb;

    void Awake()
    {
        rb = GetComponent<Rigidbody>();
    }

    private void OnCollisionEnter(Collision collision)
    {
        // Nur snappen, wenn wir ein anderes Bauobjekt treffen und noch nicht gesnappt sind
        if (!hasSnapped && (collision.gameObject.CompareTag("Block")))
        {
            Vector3 currentRotation = transform.rotation.eulerAngles;
            // Wir prüfen, ob der Aufprall hauptsächlich von OBEN kam (Normalen-Vektor zeigt nach oben)
            // contact.normal zeigt von der getroffenen Oberfläche weg. Wenn sie nach oben zeigt, landen wir auf der Oberseite.
            if (collision.contactCount > 0)
            {
                ContactPoint contact = collision.GetContact(0);
                if (contact.normal.y > 0.7f) // 1f wäre perfekt gerade von oben, 0.7f erlaubt leicht schräges Aufkommen
                {
                    // Wir runden die X- und Z-Rotation auf die nächste 90-Grad-Kante (oder 0°, 180°, 270°)
                    // Dadurch bleibt der Block auf der Seite liegen, wenn der Spieler ihn auf die Seite gedreht hat,
                    // wird aber trotzdem perfekt gerade ausgerichtet, damit er nicht wackelt!
                    float snappedX = Mathf.Round(currentRotation.x / 90f) * 90f;
                    float snappedZ = Mathf.Round(currentRotation.z / 90f) * 90f;

                    // Die Y-Rotation bleibt die vom Spieler (z.B. deine 45-Grad-Schritte mit 'R')
                    float snappedY = currentRotation.y;
                    transform.rotation = Quaternion.Euler(snappedX, snappedY, snappedZ);

                    // 3. ROTATIONS-SPERRE FÜR STABILITÄT
                    // Sobald er liegt, darf er nicht mehr um die X- und Z-Achse wegkippen.
                    rb.constraints = RigidbodyConstraints.FreezeRotationX | RigidbodyConstraints.FreezeRotationZ;

                    // 4. TRÄGHEIT UND BEWEGUNG STOPPEN
                    // Wir nehmen den kompletten Schwung aus dem Aufprall, damit es keine Mikro-Bouncer gibt.
                    rb.linearVelocity = Vector3.zero;
                    rb.angularVelocity = Vector3.zero;

                    hasSnapped = true;
                }
            }

            
        }
    }
    */
}