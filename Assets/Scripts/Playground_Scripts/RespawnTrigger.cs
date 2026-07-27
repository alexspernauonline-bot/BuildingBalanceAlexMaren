using UnityEngine;
using System.Collections;

public class RespawnTrigger : MonoBehaviour
{
    public GameObject respawnPoint; // Das Ziel, zu dem der Spieler respawnen soll

    private GameObject playerRef;

    void Start()
    {
        playerRef = GameObject.FindWithTag("Player");

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

            //Open Respawn Menue

            //Beginn Respawn 
            StartRespawn();
        }
    }

    //Started Coroutine für Delay + Zugriff von anderen Skripten möglich
    public void StartRespawn()
    {
        StartCoroutine(RespawnPlayer(playerRef));
    }

    //Tatsächliche Respawn nach 3 Sekunden
    IEnumerator RespawnPlayer(GameObject player)
    {
        yield return new WaitForSeconds(3);

        PlayerMovement playerSkript = player.GetComponent<PlayerMovement>();
        CharacterController playerCc = player.GetComponent<CharacterController>();

        //Bewegung durch Character Controller des Spielers verhindert
        playerSkript.move = Vector3.zero;

        playerCc.enabled = false;
        player.transform.position = respawnPoint.transform.position;
        playerCc.enabled = true;
    }
}
