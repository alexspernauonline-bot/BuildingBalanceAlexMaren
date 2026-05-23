using UnityEngine;

public class PlayerController : MonoBehaviour
{
    //Definiert Geschwindigkeit
    public float speed = 10;
    // Definiert Rotations Geschwindigkeit
    public float turnSpeed;

    // Update is called once per frame
    void Update()
    {
        // Curser-Tasten rechts u links abfragen (Wert zwischen -1 u 1)
        float horizontalInput = Input.GetAxis("Horizontal");
        // Curser-Tasten oben u unten abfragen (Wert zwischen -1 u 1)
        float verticalInput = Input.GetAxis("Vertical");

        // GameObject entlang der X-Achse verschieben
        transform.Translate(Vector3.right * Time.deltaTime * speed * horizontalInput);

        // GameObject entlang der Z-Achse verschieben
        transform.Translate(Vector3.forward * Time.deltaTime * speed * verticalInput);

        // GameObject entlang der Y-Achse rotieren
        if (verticalInput > 0)
        {
            transform.Rotate(Vector3.up, turnSpeed * Time.deltaTime * horizontalInput);
        }
        // Dash funktion bauen
        if (Input.GetKeyDown(KeyCode.Space))
        {
            speed *= 2;
        }
        if (Input.GetKeyUp(KeyCode.Space))
        {
            speed = 20;
        }
    }


}



