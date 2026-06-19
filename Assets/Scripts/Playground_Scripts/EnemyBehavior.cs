using UnityEngine;

public class EnemyBehavior : MonoBehaviour
{
    public float impulseStrength;
    public bool isAsleep;
    public float speed;
    private Rigidbody enemyRb;

    private GameObject player;
    // Start is called once before the first execution of Update after the MonoBehaviour is created
    void Start()
    {
        enemyRb = GetComponent<Rigidbody>();

        player = GameObject.Find("Player");
        isAsleep = true;
    }

    // Update is called once per frame
    void Update()
    {
        if (isAsleep == false)
        {
            Vector3 lookDirection = player.transform.position - transform.position;
            enemyRb.AddForce(lookDirection.normalized * speed * Time.deltaTime);

        }
    }

    private void OnTriggerEnter(Collider other)
    {
        if (other.gameObject.CompareTag("Player") && isAsleep)
        {
            isAsleep = false;

            Vector3 lookDirection = player.transform.position - transform.position;
            enemyRb.AddForce(lookDirection.normalized * impulseStrength * Time.deltaTime, ForceMode.Impulse);
        }
    }
   
}
