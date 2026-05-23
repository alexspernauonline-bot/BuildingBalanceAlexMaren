using UnityEngine;

public class CameraBehavior : MonoBehaviour
{
    // Referenz zum Spieler
    public GameObject player;
    // Abstand zum Spieler
    public Vector3 offset;

    // Update is called once per frame
    void LateUpdate()
    {
        transform.position = player.transform.position + offset;

    }
   
}
