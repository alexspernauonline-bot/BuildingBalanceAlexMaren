using UnityEngine;

public class ThirdPersonMovement : MonoBehaviour
{
    public Rigidbody playerRb;
    public Transform camera;

    public float speed = 6f;
    public float jumpForce = 5f;
    private bool isGrounded = true;

    public float turnSmoothTime = 0.1f;
    private float turnSmoothVelocity;

    // Update is called once per frame
    void Update()
    {
        //Variablen für Spielersteuerung WASD
        float horizontalInput = Input.GetAxisRaw("Horizontal");
        float verticalInput = Input.GetAxisRaw("Vertical");

        //Richtungsvektor für Bewegung, normalized damit auch diagonale Bewegung gleich schnell ist
        Vector3 direction = new Vector3(horizontalInput, 0f, verticalInput).normalized;

        //Gleichmäßige Drehbewegung des Spielers
        float targetAngle = Mathf.Atan2(direction.x, direction.z) * Mathf.Rad2Deg + camera.rotation.eulerAngles.y;
        float angle = Mathf.SmoothDampAngle(transform.eulerAngles.y, targetAngle, ref turnSmoothVelocity, turnSmoothTime);
        transform.rotation = Quaternion.Euler(0, angle, 0);

        //Bewegung des Spielers in Kamera-Richtung
        Vector3 moveDirection = Quaternion.Euler(0f, targetAngle, 0f) * Vector3.forward;
        if(horizontalInput != 0f || verticalInput != 0f)
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
            
            playerRb.transform.Translate(moveDirection.normalized * finalSpeed * Time.deltaTime);
        }

        //Hüpf-Funktion auf Leertaste
        if (Input.GetKeyDown(KeyCode.Space) && isGrounded)
        {
            playerRb.AddForce(Vector3.up * jumpForce, ForceMode.Impulse);
            isGrounded = false;
        }
    }

    public bool PlayerIsGrounded()
    {
        isGrounded = true;
        return isGrounded;
    }
}
