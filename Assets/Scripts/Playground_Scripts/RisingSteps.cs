using UnityEngine;

public class RisingSteps : MonoBehaviour
{
    //Festgelegte Punkte
    private float yA = -1f;
    private float yB = 2.5f;

    private Vector3 pointA;
    private Vector3 pointB;

    //Geschwindigkeit
    public float speed = 3;

    private float timeOffset;

    // Start is called once before the first execution of Update after the MonoBehaviour is created
    void Start()
    {
        //Bewegung nur entlang der y-Achse
        pointA = new Vector3(transform.position.x, yA, transform.position.z);
        pointB = new Vector3(transform.position.x, yB, transform.position.z);

        timeOffset = Random.Range(0f, 100f);
    }

    // Update is called once per frame
     void Update()
    {
        //Loop zwischen PointA und PointB für kontinuierliche Bewegung der Plattform
        transform.position = Vector3.Lerp(pointA, pointB, Mathf.PingPong((Time.time + timeOffset) / speed, 1));
    }
}
