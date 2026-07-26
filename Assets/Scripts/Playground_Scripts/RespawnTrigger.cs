using UnityEngine;

public class RespawnTrigger : MonoBehaviour
{
    public GameObject respawnPoint; // Das Ziel, zu dem der Spieler respawnen soll

    void Start()
    {

        // Stelle sicher, dass der Collider als Trigger eingestellt ist
        Collider collider = GetComponent<Collider>();
        if (collider != null)
        {
            collider.isTrigger = true;
        }
        else
        {
            Debug.LogError("No Collider component found on this GameObject. Please add a Collider and set it as a trigger.");
        }
    }
    //Methode wird aufgerufen, wenn ein Collider in den Trigger eintritt
    void OnTriggerEnter(Collider other)
    {
        // �berpr�ft, ob das Objekt, das den Trigger betreten hat, ein Gegner oder der Spieler ist
        if (other.gameObject.CompareTag("Enemy"))
        {
            Destroy(other.gameObject); // Der Gegner wird zerst�rt, wenn er in den Trigger f�llt
            Debug.Log("Enemy has died!");

        }
        if (other.gameObject.CompareTag("Player"))
        {
            Debug.Log("Player has fallen into the ball Pit! Respawning...");

            // HIER STARTET DER RESPAWN-PROZESS:
            if (respawnPoint != null)
            {
                TeleportPlayer(other.gameObject);
            }
            else
            {
                Debug.LogWarning("Kein RespawnPoint im Inspector zugewiesen!");
            }


            //Hier kannst du Code hinzufügen um einen Game-Over-Bildschirm anzuzeigen.
        }
    }
    void TeleportPlayer(GameObject player)
    {
        //Rigidbody vom Spieler
        Rigidbody rb = player.GetComponent<Rigidbody>();

        if (rb != null)
        {
            //KINETIC die Physik f�r einen winzigen Moment auf "kinematic" geschalten.
            
            rb.isKinematic = true;

            // TELEPORT: neue Position setzten, damit der Spieler sofort am RespawnPoint steht
            player.transform.position = respawnPoint.transform.position;

            // RESET: alle Bewegungsenergien (Fallen, Rutschen) komplett auf 0 setzten
            rb.linearVelocity = Vector3.zero;
            rb.angularVelocity = Vector3.zero;

            //PHYSIK WIEDER AN:
            rb.isKinematic = false;
        }
        else
        {
            // Falls der Rigidbody auf einem Kind-Objekt liegt
            player.transform.position = respawnPoint.transform.position;
        }

        //AM BODEN RESETTEN:der Spieler darf wieder laufen 
        /*PlayerMovement movement = player.GetComponent<PlayerMovement>();
        if (movement != null)
        {
            movement.PlayerIsGrounded();
        }*/
    }
}
