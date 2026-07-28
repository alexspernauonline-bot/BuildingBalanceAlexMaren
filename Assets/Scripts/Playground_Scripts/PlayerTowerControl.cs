using UnityEngine;
using System.Collections;

public class PlayerTowerControl : MonoBehaviour
{
    //Referenz zum Spieler
    private GameObject camRef;
    private PlayerMovement playerSkript;
    private GameObject playerRef;

    //Offset für die Position des Spielerturms + Festlegen der Größe
    private Vector3 offset = new Vector3(0f, -0.5f, 1.5f);
    private Vector3 scaleChange = new Vector3(0.3f, 0.3f, 0.3f);

    //Variablen für Wobbly Tower
    public float xAmplitude = 0.5f;
    public float xFrequency = 3.3f;
    public float yAmplutide = 0.5f;
    public float yFrequency = 3.3f;
    
    Quaternion targetRot;
    public float wobbleSpeed = 2f;
    public float maxRot = 50f;
    public bool isWobbly = false;
    public bool towerHasFallen;

    //Variable für Turm-Punkte
    public int towerDestructions = 0;

    void Start()
    {
        //Verbindung zum Spieler
        camRef = GameObject.FindWithTag("MainCamera");
        playerSkript = GameObject.FindWithTag("Player").GetComponent<PlayerMovement>();
        playerRef = GameObject.FindWithTag("Player");

        //Start-Sitaution setzen
        towerHasFallen = false;

        //Vorbereiten der Blöcke im Turm
        foreach (Transform block in this.transform)
        {
            Rigidbody blockRb = block.GetComponent<Rigidbody>();
            blockRb.isKinematic = true;
        }

        //Setzen von Position und Größe
        transform.localPosition = new Vector3(0f, 0f, 0f);
        transform.SetParent(camRef.transform, false);
        transform.localPosition += offset;
        transform.localScale = scaleChange;

        InvokeRepeating("TimeTillFalldown", 1, 2);

    }

    // Update is called once per frame
    void Update()
    {
        //Turm wackelt nach dem Springen
        if ((playerSkript.hasJumped || playerSkript.isSprinting) && playerSkript.hitGround)
        {
            isWobbly = true;
        }

        //Das Wackeln des Turms wird nicht durch erneutes Springen unterbrochen
        if (isWobbly)
        {
            Wobble();

            //Spieler kann Turm wieder ins Gleichgewicht bringen (E)
            if (Input.GetKeyDown(KeyCode.E))
            {
                wobbleSpeed--;
            }
        }

        //Turm ist wieder im Gleichgewicht
        if (wobbleSpeed <= 0)
        {
            print("Tower is balanced again!");
            playerSkript.hasJumped = false;
            isWobbly = false;
            wobbleSpeed = 2f;
        }
        else if (wobbleSpeed >= 10 || playerSkript.hasCollided)
        {
            towerHasFallen = true;

            playerSkript.hasJumped = false;
            playerSkript.hasCollided = false;
            isWobbly = false;
            wobbleSpeed = 2f;

            TowerDestructionCount();
        }
    }

    //Wackeln des Turms über z-Rotation der einzelnen Blöcke
    private void Wobble()
    {
        foreach (Transform block in this.transform)
        {
            targetRot = Quaternion.Euler(0f, 0f, Random.Range(-maxRot, maxRot));

            block.localRotation = Quaternion.Slerp(block.localRotation, targetRot, wobbleSpeed * Time.deltaTime);
        }
    }

    //Erhöht das Wackeln, bis der Turm umfällt oder wieder ins Gleichgewicht gebracht wurde
    private void TimeTillFalldown()
    {
        if (isWobbly)
        {
            wobbleSpeed += 2f;
        }
    }

    //Zählt, wie oft der Turm umgefallen ist => wichtig für Punkte-Vergabe
    public void TowerDestructionCount()
    {
        towerDestructions++;
    }
}
