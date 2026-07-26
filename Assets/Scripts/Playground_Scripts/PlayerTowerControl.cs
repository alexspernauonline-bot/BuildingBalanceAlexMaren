using UnityEngine;

public class PlayerTowerControl : MonoBehaviour
{

    //Referenz zum Spieler
    private GameObject camRef;

    //Offset für die Position des Spielerturms + Festlegen der Größe
    private Vector3 offset = new Vector3(0f, -0.5f, 1.5f);
    private Vector3 scaleChange = new Vector3(0.3f, 0.3f, 0.3f);


    // Start is called once before the first execution of Update after the MonoBehaviour is created
    void Start()
    {
        //Verbindung zum Spieler
        camRef = GameObject.FindWithTag("MainCamera");

        //Vorbereiten der Blöcke im Turm
        foreach (Transform block in this.transform)
        {
            Rigidbody blockRb = block.GetComponent<Rigidbody>();
            blockRb.isKinematic = true;
        }


        //Setzen von Position und Größe
        transform.SetParent(camRef.transform, false);
        transform.localPosition += offset;
        transform.localScale = scaleChange;
    }

    // Update is called once per frame
    void Update()
    {
        
    }
}
