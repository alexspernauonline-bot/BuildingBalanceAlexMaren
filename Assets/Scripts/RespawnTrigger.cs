using UnityEngine;

public class RespawnTrigger : MonoBehaviour
{
    void OnTriggerEnter(Collider other)
    {
        if (other.gameObject.CompareTag("Enemy"))
        {
            Destroy(other.gameObject); // Destroy the enemy game object
            Debug.Log("Enemy has died!");

        }
        if (other.gameObject.CompareTag("Player"))
        {
            Debug.Log("Player has fallen into the ball Pit!");
            // Here you can add code to handle the player's death, such as restarting the level or showing a game over screen.
        }
    }
}
