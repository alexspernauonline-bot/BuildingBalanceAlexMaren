using UnityEngine;

public class ThirdPersonMovement : MonoBehaviour
{
    public Rigidbody playerRb;
    public Transform camera;

    public float speed = 6f;
    public float jumpForce = 5f;
    public float rotationSpeed = 5f;

    private bool isGrounded = true;
    private bool hasSpaceBeenPressed = false;
    private bool hasShiftBeenPressed = false;

    private Vector3 movementInput;
    private Vector3 targetDirection;
    private Vector3 moveDirection;

    private Vector3 cameraForward;
    private Vector3 cameraRight;

    private float targetAngle;

    void Start()
    {
        //Mauszeiger wird unsichtbar
        //Cursor.visible = false;

        //Mauszeiger wird im Game-Window gehalten
        Cursor.lockState = CursorLockMode.Confined;
    }

    // Update is called once per frame
    void Update()
    {
        //Richtungsvektor f�r Bewegung, normalized damit auch diagonale Bewegung gleich schnell ist + Variablen f�r Spielersteuerung WASD
        movementInput = new Vector3(Input.GetAxisRaw("Horizontal"), 0f, Input.GetAxisRaw("Vertical")).normalized;

        //Abfrage der Kamera-Bewegung
        cameraForward = Camera.main.transform.forward;
        cameraRight = Camera.main.transform.right;

        //Abfrage von Leertaste
        if (Input.GetKeyDown(KeyCode.Space))
        {
            hasSpaceBeenPressed = true;
        }

        //Abfrage von Shift
        if (Input.GetKey(KeyCode.LeftShift))
        {
            hasShiftBeenPressed = true;
        }

        if (Input.GetKeyUp(KeyCode.LeftShift))
        {
            hasShiftBeenPressed = false;
        }
    }

    void FixedUpdate()
    {
        //Bewegungsrichtung basierend auf Kamera
        cameraForward.y = 0f;
        cameraRight.y = 0f;

        cameraForward.Normalize();
        cameraRight.Normalize();

        moveDirection = cameraForward * movementInput.z + cameraRight * movementInput.x;

        //Drehbewegung der Spielfigur basierend auf Kamera-Rotation
        if (moveDirection.x != 0 || moveDirection.z != 0)
        {
            playerRb.MoveRotation(Quaternion.Euler(0f,
                                            Camera.main.transform.rotation.eulerAngles.y, //y-Rotation der Kamera bestimmt Rotation des Spielers
                                            0f));
        }
       
        //Sprint-Funktion auf linker Shift/Hochstell-Taste
        float finalSpeed;
        if (hasShiftBeenPressed)
        {
            finalSpeed = speed * 2;
        }
        else
        {
            finalSpeed = speed;
        }

        //Bewegung des Spielers in Kamera-Richtung
        Vector3 horizontalVelocity = moveDirection.normalized * finalSpeed;
        playerRb.linearVelocity = new Vector3(horizontalVelocity.x, playerRb.linearVelocity.y, horizontalVelocity.z);

        //H�pf-Funktion auf Leertaste
        if (hasSpaceBeenPressed)
        {
            if (isGrounded)
            {
                playerRb.AddForce(Vector3.up * jumpForce, ForceMode.Impulse);
                isGrounded = false;
            }
            
            hasSpaceBeenPressed = false;
        }
    }

    //Verhindert, dass Spieler Doppelsprung macht
    public bool PlayerIsGrounded()
    {
        isGrounded = true;
        return isGrounded;
    }
}
