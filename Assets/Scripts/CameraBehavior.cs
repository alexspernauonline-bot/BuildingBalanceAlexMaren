using UnityEngine;

public class CameraBehavior : MonoBehaviour
{
    // Referenz zum Spieler
    public GameObject player;

    // Abstand zum Spieler
    public Vector3 offset;

    //Geschwindigkeit der Drehbewegung
    public float turnSpeed = 4.0f;

    //Abstand zum Player
    private float playerDistance;

    //Drebewegung einschränken
    public float minTurnAngle = -90;
    public float maxTurnAngle = 0;
    private float rotationY;

    void Start()
    {
        //Abstand Kamera zum Spieler speichern
        playerDistance = Vector3.Distance(transform.position, player.transform.position);
    }

    void Update()
    {
        //Maus-Angaben speichern
        float mouseX = Input.GetAxis("Mouse X") * turnSpeed;

        rotationY += mouseX;

        /*//Vertikale Rotation einschränken
        mouseY = Mathf.Clamp(mouseY, minTurnAngle, maxTurnAngle);*/

        //Kamera rotieren
        transform.eulerAngles = new Vector3(15, rotationY, 0);

        //Kamera-Position anpassen
        transform.position = player.transform.position - (transform.forward * playerDistance/2);
    }

    /*// Update is called once per frame
    void LateUpdate()
    {
        //Kamera folgt Spieler
        transform.position = player.transform.position + offset;

        //Rotation passt sich der Spieler-Rotation an 
        //PROBLEM: Funktioniert nur so mäßig, muss überarbeitet werden
        transform.rotation = player.transform.rotation;

    }*/
   
}
