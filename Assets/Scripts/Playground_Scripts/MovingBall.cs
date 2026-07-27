using UnityEngine;

public class MovingBall : MonoBehaviour
{
    //Platzhalter für die Vektoren
    private float vx;
    private float vz;

    //Bewegungsgeschwindigkeit
    private float speed = 7f;

    //Vektor Bewegungsrichtung
    private Vector3 moveDirection;

    void Awake()
    {
        //zufällige Festlegung der Bewegungsrichtung
        vx = Random.Range(-1f, 1f);
        vz = Random.Range(-1f, 1f);

        moveDirection = new Vector3(vx, 0, vz);
    }

    void Update()
    {
        //Überprüfung, ob der Ball einen anderen Ball berührt
        Collider[] collidingBalls = Physics.OverlapSphere(transform.position, 1.5f);

        foreach (Collider hit in collidingBalls)
        {   
            //Falls ja, wird die Bewegungsrichtung geändert
            if (hit.gameObject != gameObject && hit.CompareTag("Obstacle"))
            {
                Vector3 away = (transform.position - hit.transform.position).normalized;

                transform.position += away * 0.2f; // Adjust this value as needed
                moveDirection = away;

                break;
            }
        }

        //Tatsächliche Positions- + Rotationsänderung
        transform.position += moveDirection.normalized * speed * Time.deltaTime;
        transform.rotation = Quaternion.LookRotation(moveDirection);

        //Begrenzung des Bewegungsraumes
        if (transform.position.x <= -6 || transform.position.x >= 35)
        {
            moveDirection.x *= -1f;
        }

        if (transform.position.z <= -45 || transform.position.z >= -20)
        {
           moveDirection.z *= -1f;
        }
    }
}
