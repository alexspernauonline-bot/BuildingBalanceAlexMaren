using UnityEngine;

public class PlayerMovement : MonoBehaviour
{
    //Verbindung zum Character controller
    public CharacterController controller;

    //Verbindung zum Spawn
    public GameObject spawn;
    private Vector3 savedSpawnPos;

    private PlayerTowerControl towerSkript;

    //Bewegungsvariablen
    public float normalSpeed = 12f;
    private float actualSpeed;
    public float gravity = -9.81f;
    public float jumpHeight = 2f;
    public float normalHeight = 2;
    public Vector3 move;

    //Variablen für GroundCheck
    public Transform groundCheck;
    public float groundDistance = 0.4f;
    public LayerMask groundMask;
    private bool isGrounded;

    //Variablen für Wobbly Tower
    public bool hasJumped = false;
    public bool hitGround = false;
    public bool hasCollided = false;
    public bool isSprinting = false;
    public bool isMoving = false;

    //Geschwindigkeit
    Vector3 velocity;

    //Variable für Spiel-Ende
    public bool hasReachedFinish = false;

    //Clips für Audio
    private AudioSource audioSource;
    public AudioClip walkSound;
    public AudioClip jumpSound;
    public AudioClip fallSound;

    // Start is called once before the first execution of Update after the MonoBehaviour is created
    void Start()
    {
        //Setzen und speichern der Startposition
        transform.position = spawn.transform.position;
        savedSpawnPos = spawn.transform.position;

        //Zuordnen von Komponenten
        towerSkript = GameObject.FindWithTag("PlayerTower").GetComponent<PlayerTowerControl>();
        audioSource = GetComponent<AudioSource>();
    }

    // Update is called once per frame
    void Update()
    {
        //Ground Check
        isGrounded = Physics.CheckSphere(groundCheck.position, groundDistance, groundMask);

        if (isGrounded)
        {
            hitGround = true;
        }
        else
        {
            hitGround= false;
        }

        //Zurücksetzen der Schwerkraft, wenn Player den Boden berührt (nach unten gerichtete Velocity)
        if(isGrounded && velocity.y < 0)
        {
            velocity.y = -2f;
        }

        //Tastatureingaben speichern (WASD/Pfeiltasten)
        float x = Input.GetAxis("Horizontal");
        float z = Input.GetAxis("Vertical");

        //Bewegungsvektor
        move = transform.right * x + transform.forward * z;

        if (move.magnitude > 0 && isGrounded)
        {
            isMoving = true;
        }
        else
        {
            isMoving= false;
        }

        //Sprint Bewegung (Shift)
        if (Input.GetKey(KeyCode.LeftShift))
        {
            actualSpeed = normalSpeed * 1.5f;
            controller.height = normalHeight;
            isSprinting = true;
        }
        //Crouch Bewegung (Ctrl)
        else if (Input.GetKey(KeyCode.LeftControl))
        {
            actualSpeed = normalSpeed * 0.5f;
            controller.height = normalHeight * 0.5f;
            isSprinting = false;
        }
        //Normale Bewegung
        else
        {
            actualSpeed = normalSpeed;
            controller.height = normalHeight;
            isSprinting = false;
        }

        //Bewegung des Players
        controller.Move(move * actualSpeed * Time.deltaTime);

        if (isMoving)
        {
            if (!audioSource.isPlaying)
            {
                audioSource.PlayOneShot(walkSound, 0.2f);
            }
        }
        else
        {
            audioSource.Stop();
        }

        //Jump Funktion (Leertaste)
        if (Input.GetKeyDown(KeyCode.Space) && isGrounded)
        {
            velocity.y = Mathf.Sqrt(jumpHeight * -2f * gravity);
            hasJumped = true;
            audioSource.Stop();
            audioSource.PlayOneShot(jumpSound, 0.1f);
        }

        //Erhöhung der Schwerkraft
        velocity.y += 1.5f * gravity * Time.deltaTime;

        //Schwerkraft beeinflusst Spieler
        controller.Move(velocity * Time.deltaTime);
    }

    //Überprüfen auf Contact mit Collidern
    void OnControllerColliderHit(ControllerColliderHit hit)
    {
        if (hit.collider.CompareTag("Finish"))
        {
            hasReachedFinish = true;
        }
        else if (hit.collider.CompareTag("Obstacle"))
        {
            hasCollided = true;
        }
    }

    void OnTriggerEnter (Collider other)
    {
        if (other.CompareTag("CheckPoint"))
        {
            //Setzen von Chekcpoints in der Parkour
            spawn.transform.position = other.transform.position;
        }
    }
}
