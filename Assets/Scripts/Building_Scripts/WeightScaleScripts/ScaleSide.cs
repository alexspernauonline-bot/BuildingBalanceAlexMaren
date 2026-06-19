using UnityEngine;
using System.Collections.Generic;

public class ScaleSide : MonoBehaviour
{
    [Header("Aktuelles Gesamtgewicht")]
    public float currentWeight = 0f;

    // Speichert, welcher Rigidbody gerade wie viel Kraft (Impuls) ausübt
    private Dictionary<Rigidbody, float> impulsePerRigidBody = new Dictionary<Rigidbody, float>();

    private void UpdateWeight()
    {
        float combinedForce = 0f;

        // Alle wirkenden Kräfte zusammenrechnen
        foreach (var force in impulsePerRigidBody.Values)
        {
            combinedForce += force;
        }

        // Kraft durch Erdbeschleunigung teilen = Masse/Gewicht
        // (Wenn deine Blöcke Rigidbody-Massen von 1, 5 und 10 haben, kommt hier exakt 1, 5 oder 10 raus!)
        currentWeight = combinedForce / Physics.gravity.magnitude;

        // Sicherheitsnetz gegen minimale Physik-Zitterer unter 0
        currentWeight = Mathf.Max(0f, currentWeight);

        // DIREKT DEN MANAGER INFORMIEREN
        if (GameManager.Instance != null)
        {
            GameManager.Instance.CheckBalance();
        }
    }

    // Enter und Stay nutzen jetzt zusammen diese eine saubere Methode
    private void HandleCollision(Collision collision)
    {
        if (collision.rigidbody != null && collision.gameObject.CompareTag("Block"))
        {
            // Unity misst den Impuls. geteilt durch fixedDeltaTime ergibt das die echte Kraft in Newton.
            float forceY = collision.impulse.y / Time.fixedDeltaTime;

            if (impulsePerRigidBody.ContainsKey(collision.rigidbody))
                impulsePerRigidBody[collision.rigidbody] = forceY;
            else
                impulsePerRigidBody.Add(collision.rigidbody, forceY);

            UpdateWeight();
        }
    }

    private void OnCollisionEnter(Collision collision) => HandleCollision(collision);
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
        // Bei OnCollisionEnter holen wir uns den Rigidbody über collision.gameObject
        Rigidbody rb = collision.gameObject.GetComponent<Rigidbody>();

        if (rb != null)
        {
            // Masse dieses Objekts addieren
            currentWeight += rb.mass;
            Debug.Log(gameObject.name + " Gewicht erhöht auf: " + currentWeight);
            //Gibt dem Game Manager den Befehl das Gewicht neu zu Prüfen
            GameManager.Instance.CheckBalance();
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
            //Gibt dem Game Manager den Befehl das Gewicht neu zu Prüfen
            GameManager.Instance.CheckBalance();
        }
    }
    */

