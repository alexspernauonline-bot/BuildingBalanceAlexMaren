using UnityEngine;
using System.Collections.Generic;

public class ScaleSide : MonoBehaviour
{
   
    public float currentWeight = 0f;

    // Speichert den physikalischen Druck (Impuls) jedes untersten Blocks
    private Dictionary<Rigidbody, float> impulsePerRigidBody = new Dictionary<Rigidbody, float>();

    private float forceToMass;
    private AudioSource blockAudioSource;
    public AudioClip placeSound;
    private void Awake()
    {
        // Wandelt sp�ter die pure Kraft wieder in Kilo um (geteilt durch Erdbeschleunigung)
        forceToMass = 1f / Physics.gravity.magnitude;
        blockAudioSource = GetComponent<AudioSource>();
    }

    public void UpdateWeight()
    {
        float combinedForce = 0f;

        // Alle gespeicherten Kr�fte zusammenrechnen
        foreach (var force in impulsePerRigidBody.Values)
        {
            combinedForce += force;
        }

        // Kraft in deine festgelegten Kilos (1, 5, 10) umrechnen
        float calculatedMass = combinedForce * forceToMass;

        // WICHTIG: Physik zittert oft leicht (z.B. 5.00014f). 
        // Wir runden das auf eine Nachkommastelle, damit deine Waage sauber pr�fen kann!
        calculatedMass = Mathf.Round(calculatedMass * 10f) / 10f;

        // Sicherheitsnetz gegen Werte unter 0
        calculatedMass = Mathf.Max(0f, calculatedMass);

        // Nur dem Manager Bescheid geben, wenn sich das Gewicht WIRKLICH ge�ndert hat
        if (Mathf.Abs(currentWeight - calculatedMass) > 0.01f)
        {
            currentWeight = calculatedMass;

            if (GameManager.Instance != null)
            {
                GameManager.Instance.CheckBalance();
            }
        }
    }

    private void HandleCollision(Collision collision)
    {
       
        if (collision.rigidbody != null && (collision.gameObject.CompareTag("Block") || collision.gameObject.CompareTag("TowerBlock")))
        {
            // Impuls durch die feste Zeit eines Physik-Frames teilen = Konstante Kraft
            float forceY = collision.impulse.y / Time.fixedDeltaTime;

            if (impulsePerRigidBody.ContainsKey(collision.rigidbody))
                impulsePerRigidBody[collision.rigidbody] = forceY;
            else
                impulsePerRigidBody.Add(collision.rigidbody, forceY);
           
            UpdateWeight();
        }
    }

    // Die Kollisions-Trigger
   
    private void OnCollisionStay(Collision collision) => HandleCollision(collision);

    private void OnCollisionExit(Collision collision)
    {
        if (collision.rigidbody != null)
        {
            if (impulsePerRigidBody.ContainsKey(collision.rigidbody))
            {
                impulsePerRigidBody.Remove(collision.rigidbody);
                UpdateWeight();
            }
        }
    }
}
/*
//Speichert das Gesamtgewicht, das sich derzeit auf dieser Seite befindet.
public float currentWeight = 0f;

private void OnCollisionEnter(Collision collision)
{
    // Bei OnCollisionEnter holen wir uns den Rigidbody �ber collision.gameObject
    Rigidbody rb = collision.gameObject.GetComponent<Rigidbody>();

    if (rb != null)
    {
        // Masse dieses Objekts addieren
        currentWeight += rb.mass;
        Debug.Log(gameObject.name + " Gewicht erh�ht auf: " + currentWeight);
        //Gibt dem Game Manager den Befehl das Gewicht neu zu Pr�fen
        GameManager.Instance.CheckBalance();
    }
}

// L�uft automatisch, wenn ein Objekt von der Waage herabf�llt oder weggenommen wird
private void OnCollisionExit(Collision collision)
{
    Rigidbody rb = collision.gameObject.GetComponent<Rigidbody>();

    if (rb != null)
    {
        // Wir subtrahieren die Masse
        currentWeight -= rb.mass;

        // Sicherheitsma�nahme gegen Rundungsfehler
        currentWeight = Mathf.Max(0f, currentWeight);
        Debug.Log(gameObject.name + " Gewicht verringert auf: " + currentWeight);
        //Gibt dem Game Manager den Befehl das Gewicht neu zu Pr�fen
        GameManager.Instance.CheckBalance();
    }
}
*/

