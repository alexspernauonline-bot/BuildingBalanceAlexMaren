using UnityEngine;

public class RespawnTrigger : MonoBehaviour
{
    //Methode wird aufgerufen, wenn ein Collider in den Trigger eintritt
    void OnTriggerEnter(Collider other)
    {
        // Überprüft, ob das Objekt, das den Trigger betreten hat, ein Gegner oder der Spieler ist
        if (other.gameObject.CompareTag("Enemy"))
        {
            Destroy(other.gameObject); // Der Gegner wird zerstört, wenn er in den Trigger fällt
            Debug.Log("Enemy has died!");

        }
        if (other.gameObject.CompareTag("Player"))
        {
            Debug.Log("Player has fallen into the ball Pit!");
            //Hier kannst du Code hinzufügen, um den Respawn des Spielers zu behandeln, z.B. das Level neu zu starten oder einen Game-Over-Bildschirm anzuzeigen.
        }
    }
}
