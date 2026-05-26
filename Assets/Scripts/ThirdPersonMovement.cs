using UnityEngine;

public class ThirdPersonMovement : MonoBehaviour
{
    public Rigidbody playerRb;
    public Transform camera;

    public float speed = 6f;

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
            playerRb.transform.Translate(moveDirection.normalized * speed * Time.deltaTime);
        }
        

    }
}
