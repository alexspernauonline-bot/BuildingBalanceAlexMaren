using UnityEngine;

public class MovingPlattforms : MonoBehaviour
{
    //Festgelegte Punkte
    private int zA = -10;
    private int zB = 1;

    private Vector3 pointA;
    private Vector3 pointB;

    //Geschwindigkeit
    public float speed = 3;

    // Start is called once before the first execution of Update after the MonoBehaviour is created
    void Start()
    {
        //Bewegung nur entlang der y-Achse
        pointA = new Vector3(transform.position.x, transform.position.y, zA);
        pointB = new Vector3(transform.position.x, transform.position.y, zB);
    }

    // Update is called once per frame
    void Update()
    {
        //Loop zwischen PointA und PointB für kontinuierliche Bewegung der Plattform
        transform.position = Vector3.Lerp(pointA, pointB, Mathf.PingPong(Time.time/speed, 1));
    }
}
