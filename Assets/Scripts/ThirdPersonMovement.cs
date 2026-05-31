using UnityEngine;

public class ThirdPersonMovement : MonoBehaviour
{
    public Rigidbody playerRb;
    public Transform camera;

    public float speed = 6f;
    public float jumpForce = 5f;
    private bool isGrounded = true;
    public float rotationSpeed = 5f;

    private Vector3 movementInput;
    private float targetAngle;
    private Vector3 targetDirection;

    public float turnSmoothTime = 0.1f;
    private float turnSmoothVelocity;

    void Start()
    {
        Cursor.visible = false;
    }

    // Update is called once per frame
    void Update()
    {
        //Richtungsvektor für Bewegung, normalized damit auch diagonale Bewegung gleich schnell ist + Variablen für Spielersteuerung WASD
        movementInput = new Vector3(Input.GetAxisRaw("Horizontal"), 0f, Input.GetAxisRaw("Vertical")).normalized;
        
        //Winkel der Bewegung basierend auf User-Input
        targetAngle = Mathf.Atan2(movementInput.x, movementInput.z) * Mathf.Rad2Deg;

        //Bewegungs-Vektor
        Vector3 moveDirection = Quaternion.Euler(0f, targetAngle, 0f) * Vector3.forward;

        //Drehbewegung der Spielfigur basierend auf Kamera-Rotation
        if (moveDirection.x != 0 || moveDirection.z != 0 )
        {
            playerRb.transform.rotation = Quaternion.Euler(playerRb.transform.rotation.eulerAngles.x, 
                                            Camera.main.transform.rotation.eulerAngles.y, //y-Rotation der Kamera bestimmt Rotation des Spielers
                                            playerRb.rotation.eulerAngles.z);
        }

        //Bewegung des Spielers in Kamera-Richtung
        if (movementInput.magnitude > 0) //Bewegung wird nur bei Player-Input ausgeführt
        {
            //Sprint-Funktion auf linker Shift/Hochstell-Taste
            float finalSpeed;
            if (Input.GetKey(KeyCode.LeftShift))
            {
                finalSpeed = speed * 2;
            }
            else
            {
                finalSpeed = speed;
            }

            //tatsächliche Bewegung
            playerRb.transform.Translate(moveDirection.normalized * finalSpeed * Time.deltaTime);
        }

        //Hüpf-Funktion auf Leertaste
        if (Input.GetKeyDown(KeyCode.Space)&& isGrounded)
        {
            playerRb.AddForce(Vector3.up * jumpForce, ForceMode.Impulse);
            isGrounded = false;
        }
    }

    //Verhindert, dass Spieler Doppelsprung macht
    public bool PlayerIsGrounded()
    {
        isGrounded = true;
        return isGrounded;
    }
}
