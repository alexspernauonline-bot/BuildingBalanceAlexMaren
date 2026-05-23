using UnityEngine;

public class PlayerController : MonoBehaviour
{
    //Definiert Geschwindigkeit
    public float speed = 10;
    private float finalSpeed;

    // Definiert Rotations Geschwindigkeit
    public float turnSpeed;

    // Update is called once per frame
    void Update()
    {
        // Curser-Tasten rechts u links abfragen (Wert zwischen -1 u 1)
        float horizontalInput = Input.GetAxis("Horizontal");
        // Curser-Tasten oben u unten abfragen (Wert zwischen -1 u 1)
        float verticalInput = Input.GetAxis("Vertical");

        //Dash Funktion (Über Space? Sicher?)
        if (Input.GetKey(KeyCode.Space))
        {
            finalSpeed = speed * 2;
        }
        else
        {
            finalSpeed = speed;
        }

        // GameObject entlang der X-Achse verschieben
        transform.Translate(Vector3.right * Time.deltaTime * finalSpeed * horizontalInput);

        // GameObject entlang der Z-Achse verschieben
        transform.Translate(Vector3.forward * Time.deltaTime * finalSpeed * verticalInput);

        // GameObject entlang der Y-Achse rotieren
        //PROBLEM: Objekt rotiert, Kamera allerdings nicht
        if (verticalInput > 0)
        {
            transform.Rotate(Vector3.up, turnSpeed * Time.deltaTime * horizontalInput);
        }
        /* Dash funktion bauen
        if (Input.GetKeyDown(KeyCode.Space))
        {
            speed *= 2;
        }
        if (Input.GetKeyUp(KeyCode.Space))
        {
            speed = 20;
        }*/
    }


}



