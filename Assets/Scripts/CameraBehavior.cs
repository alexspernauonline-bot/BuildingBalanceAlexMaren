using UnityEngine;

public class CameraBehavior : MonoBehaviour
{
    // Referenz zum Spieler
    public GameObject player;

    /*// Abstand zum Spieler
    public Vector3 offset;*/

    private const float YMin = -50.0f;
    private const float YMax = 50.0f;

    public float distance = 10;
    private float currentX = 0;
    private float currentY = 0;
    public float sensitivity = 4;


    // Update is called once per frame
    void LateUpdate()
    {
        currentX += Input.GetAxis("Mouse X") * sensitivity * Time.deltaTime;
        currentY += Input.GetAxis("Mouse Y") * sensitivity * Time.deltaTime;

        currentY = Mathf.Clamp(currentY, YMin, YMax);

        Vector3 direction = new Vector3(0, 0, -distance);
        Quaternion rotation = Quaternion.Euler(currentY, currentX, 0);
        transform.position = player.transform.position + rotation * direction;

        transform.player(player.position);



        /*//Kamera folgt Spieler
        transform.position = player.transform.position + offset;

        //Rotation passt sich der Spieler-Rotation an 
        //PROBLEM: Funktioniert nur so mäßig, muss überarbeitet werden
        transform.rotation = player.transform.rotation;*/

    }
   
}
