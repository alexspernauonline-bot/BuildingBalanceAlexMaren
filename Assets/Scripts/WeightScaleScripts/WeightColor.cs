using UnityEngine;

public class ColorWeight : MonoBehaviour
{
    //Hier sind die drei möglichen Gewichtstypen definiert.
    public enum BlockType
    {
        Light,
        Medium,
        Heavy
    }

    //Damit kann man das Gewicht im Inspector auswählen, bevor das Spiel startet.
    public BlockType myWeightType;

    void Start()
    {
        //Randomiserung des Gewichtstyps, damit jedes Mal, wenn das Spiel gestartet wird, die Blöcke unterschiedliche Gewichte und Farben haben.
        int randomPick = Random.Range(0, 3);
        myWeightType = (BlockType)randomPick;
        // greift auf die Physik (Rigidbody) und die visuelle (MeshRenderer) Komponenten zu, die an diesem Block angehängt sind.
        Rigidbody rb = GetComponent<Rigidbody>();
        
        MeshRenderer meshRenderer = GetComponent<MeshRenderer>();

        // überprüft, welches Gewicht ausgewählt wurde, und weiset die Farbe und Masse zu.
        if (myWeightType == BlockType.Light)
        {
            rb.mass = 1f;                                //setzt die Masse auf 1
            meshRenderer.material.color = Color.green;   //setzt die Farbe auf Grün
        }
        else if (myWeightType == BlockType.Medium)
        {
            rb.mass = 5f;                                //setzt die Masse auf 5
            meshRenderer.material.color = Color.blue;  //setzt die Farbe auf Blau
        }
        else if (myWeightType == BlockType.Heavy)
        {
            rb.mass = 10f;                               //setzt die Masse auf 10
            meshRenderer.material.color = Color.red;     //setzt die Farbe auf Rot
        }
    }
}