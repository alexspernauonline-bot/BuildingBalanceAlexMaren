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

    // Update is called once per frame
    void Update()
    {
        //Richtungsvektor für Bewegung, normalized damit auch diagonale Bewegung gleich schnell ist + Variablen für Spielersteuerung WASD
        movementInput = new Vector3(Input.GetAxisRaw("Horizontal"), 0f, Input.GetAxisRaw("Vertical")).normalized;
        //targetDirection = new Vector3(transform.position.x, 0, transform.position.z);

        //playerRb.transform.rotation = Quaternion.Slerp(playerRb.transform.rotation, Quaternion.LookRotation(-targetDirection), rotationSpeed * Time.deltaTime);

        //Gleichmäßige Drehbewegung des Spielers
        if (movementInput.magnitude > 0)
        {
            targetAngle = Mathf.Atan2(movementInput.x, movementInput.z) * Mathf.Rad2Deg + camera.rotation.eulerAngles.y; //gezielter World-space Winkel des Spielers basierend auf Kamera (in Richtung Kamera) und User-Input
            float angle = Mathf.SmoothDampAngle(transform.eulerAngles.y, targetAngle, ref turnSmoothVelocity, turnSmoothTime); //für gleichmäßige Drehung
            transform.rotation = Quaternion.Euler(0, angle, 0); //tatsächliche Rotation
        }

        //Bewegung des Spielers in Kamera-Richtung
        Vector3 moveDirection = Quaternion.Euler(0f, targetAngle, 0f) * Vector3.forward; //Kamera-Richtung
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
