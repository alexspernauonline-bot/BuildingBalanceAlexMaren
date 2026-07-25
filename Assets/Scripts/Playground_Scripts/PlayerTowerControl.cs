using UnityEngine;

public class PlayerTowerControl : MonoBehaviour
{

    //Referenz zum Spieler
    private GameObject playerRef;

    //Offset für die Position des Spielerturms + Festlegen der Größe
    private Vector3 offset = new Vector3(0f, 0f, 1.5f);
    private Vector3 scaleChange = new Vector3(0.3f, 0.3f, 0.3f);


    // Start is called once before the first execution of Update after the MonoBehaviour is created
    void Start()
    {
        //Verbindung zum Spieler
        playerRef = GameObject.FindWithTag("Player");
        transform.SetParent(playerRef.transform, false);

        //Setzen von Position und Größe
        transform.localPosition += offset;
        transform.localScale = scaleChange;

        foreach (Transform childTransform in this.transform)
        {
            Rigidbody blockRb = childTransform.GetComponent<Rigidbody>();
            blockRb.isKinematic = true;
        }
    }

    // Update is called once per frame
    void Update()
    {
        
    }
}
