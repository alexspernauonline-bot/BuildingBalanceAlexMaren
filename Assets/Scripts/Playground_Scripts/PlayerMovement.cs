using UnityEngine;

public class PlayerMovement : MonoBehaviour
{
    //Verbindung zum Character controller
    public CharacterController controller;

    //Verbindung zum Spawn
    public GameObject spawn;

    //Bewegungsvariablen
    public float normalSpeed = 12f;
    private float actualSpeed;
    public float gravity = -9.81f;
    public float jumpHeight = 2f;
    public float normalHeight = 2;

    //Variablen für GroundCheck
    public Transform groundCheck;
    public float groundDistance = 0.4f;
    public LayerMask groundMask;
    bool isGrounded;

    //Variable für Checkpoints
    Vector3 savedCheckpoint;

    //Geschwindigkeit
    Vector3 velocity;

    public bool hasReachedFinish = false;

    // Start is called once before the first execution of Update after the MonoBehaviour is created
    void Start()
    {
        //Setzen und speichern der Startposition
        transform.position = spawn.transform.position;
        savedCheckpoint = transform.position;
    }

    // Update is called once per frame
    void Update()
    {
        //Ground Check
        isGrounded = Physics.CheckSphere(groundCheck.position, groundDistance, groundMask);

        //Zurücksetzen der Schwerkraft, wenn Player den Boden berührt (nach unten gerichtete Velocity)
        if(isGrounded && velocity.y < 0)
        {
            velocity.y = -2f;
        }

        //Tastatureingaben speichern (WASD/Pfeiltasten)
        float x = Input.GetAxis("Horizontal");
        float z = Input.GetAxis("Vertical");

        //Bewegungsvektor
        Vector3 move = transform.right * x + transform.forward * z;

        //Sprint Bewegung (Shift)
        if (Input.GetKey(KeyCode.LeftShift))
        {
            actualSpeed = normalSpeed * 1.5f;
            controller.height = normalHeight;
        }
        //Crouch Bewegung (Ctrl)
        else if (Input.GetKey(KeyCode.LeftControl))
        {
            actualSpeed = normalSpeed * 0.5f;
            controller.height = normalHeight * 0.5f;
        }
        //Normale Bewegung
        else
        {
            actualSpeed = normalSpeed;
            controller.height = normalHeight;
        }

        //Bewegung des Players
        controller.Move(move * actualSpeed * Time.deltaTime);

        //Jump Funktion (Leertaste)
        if (Input.GetKeyDown(KeyCode.Space) && isGrounded)
        {
            velocity.y = Mathf.Sqrt(jumpHeight * -2f * gravity);
        }

        //Erhöhung der Schwerkraft
        velocity.y += 1.5f * gravity * Time.deltaTime;

        //Schwerkraft beeinflusst Spieler
        controller.Move(velocity * Time.deltaTime);
    }

    void OnTriggerEnter(Collider other)
    {
        if (other.CompareTag("Finish"))
        {
            hasReachedFinish = true;
        }
        else if (other.CompareTag("Respawn"))
        {
            print("Respawn!");
            ResetPosition();
        }
    }

    //Zurücksetzen der Position auf eine gespeicherte Startposition/Checkpoint
    private void ResetPosition()
    {
        print(savedCheckpoint);
        //ISSUE LIES SOMEWHERE HERE IDKKK
        transform.position = spawn.transform.position;
    }
}
