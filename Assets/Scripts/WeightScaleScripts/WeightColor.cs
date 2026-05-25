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
            rb.mass = 1f;                                                         //setzt die Masse auf 1
            meshRenderer.material = Resources.Load<Material>("greenMaterial");   //setzt die Farbe auf Grün
            //meshRenderer.material = RoundManager.Instance.lightMaterial;

        }
        else if (myWeightType == BlockType.Medium)
        {
            rb.mass = 5f;                                                       //setzt die Masse auf 5
            meshRenderer.material = Resources.Load<Material>("blueMaterial");   //setzt die Farbe auf Blau
            //meshRenderer.material = RoundManager.Instance.mediumMaterial;
        }
        else if (myWeightType == BlockType.Heavy)
        {
            rb.mass = 10f;                                                      //setzt die Masse auf 10
            meshRenderer.material = Resources.Load<Material>("redMaterial");    //setzt die Farbe auf Rot
            //meshRenderer.material = RoundManager.Instance.heavyMaterial;
        }
    }
}