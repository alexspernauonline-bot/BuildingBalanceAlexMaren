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
        //Kamera folgt Spieler
        transform.position = player.transform.position + offset;

        //Rotation passt sich der Spieler-Rotation an 
        //PROBLEM: Funktioniert nur so mäßig, muss überarbeitet werden
        transform.rotation = player.transform.rotation;

    }
   
}
