using UnityEngine;
using System.Collections;
using UnityEngine.UI;
using UnityEngine.EventSystems;
using TMPro;
using UnityEngine.SceneManagement;

public class RespawnTrigger : MonoBehaviour
{
    public GameObject respawnPoint; // Das Ziel, zu dem der Spieler respawnen soll
    private PlayerTowerControl towerSkript;
    public GameObject respawnScreen;
    public TextMeshProUGUI countdownText;

    private GameObject playerRef;
    private AudioSource audioSource;

    public AudioClip fallSound;

    private float respawnDelay = 2f;

    void Start()
    {
        //Zuordnen der Komponenten
        playerRef = GameObject.FindWithTag("Player");
        audioSource = GetComponent<AudioSource>();
        towerSkript = GameObject.FindWithTag("PlayerTower").GetComponent<PlayerTowerControl>();

        respawnScreen.SetActive(false);

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
        audioSource.PlayOneShot(fallSound, 0.35f);
        respawnScreen.SetActive(true);
        PlayerMovement playerSkript = playerRef.GetComponent<PlayerMovement>();
        playerSkript.enabled = false;

        StartCoroutine(RespawnPlayer(playerRef));
    }

    //Tatsächliche Respawn nach 3 Sekunden
    IEnumerator RespawnPlayer(GameObject player)
    {
        yield return new WaitForSeconds(respawnDelay);

        //Erhöht Zähler für den Turm => wichtig für finale Punkte
        towerSkript.TowerDestructionCount();

        PlayerMovement playerSkript = player.GetComponent<PlayerMovement>();
        CharacterController playerCc = player.GetComponent<CharacterController>();

        //Bewegung durch Character Controller des Spielers verhindert
        playerSkript.enabled = true;
        playerSkript.move = Vector3.zero;

        playerCc.enabled = false;
        player.transform.position = respawnPoint.transform.position;
        playerCc.enabled = true;

        respawnScreen.SetActive(false);
    }
}
