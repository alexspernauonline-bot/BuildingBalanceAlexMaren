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
        //Überprüft, ob Collider der Player ist
        if (other.gameObject.CompareTag("Player"))
        {
            Debug.Log("Player has fallen into the ball Pit! Respawning...");

            TeleportPlayer(other.gameObject);

            //Hier kannst du Code hinzufügen um einen Game-Over-Bildschirm anzuzeigen.
        }
    }

    public void TeleportPlayer(GameObject player)
    {
        PlayerMovement playerSkript = player.GetComponent<PlayerMovement>();
        CharacterController playerCc = player.GetComponent<CharacterController>();

        playerSkript.move = Vector3.zero;

        playerCc.enabled = false;
        player.transform.position = respawnPoint.transform.position;
        playerCc.enabled = true;
    }
}
