using UnityEngine;

public class ColorWeight : MonoBehaviour
{
    // 1. We create a custom list of categories using an 'enum'.
    // Think of this as creating your own custom dropdown menu.
    public enum BlockType
    {
        Light,
        Medium,
        Heavy
    }

    // 2. This variable makes our custom dropdown show up in the Unity Inspector.
    public BlockType myWeightType;

    // Start is called exactly once, the moment the object appears in the game.
    void Start()
    {

        int randomPick = Random.Range(0, 3);
        myWeightType = (BlockType)randomPick;
        // 3. We grab the physics (Rigidbody) and visual (MeshRenderer) components attached to this block.
        Rigidbody rb = GetComponent<Rigidbody>();
        
        MeshRenderer meshRenderer = GetComponent<MeshRenderer>();

        // 4. We check what you selected in the dropdown, and assign the color and mass to match!
        if (myWeightType == BlockType.Light)
        {
            rb.mass = 1f;                                // Set weight to 1
            meshRenderer.material.color = Color.green;   // Set color to Green
        }
        else if (myWeightType == BlockType.Medium)
        {
            rb.mass = 5f;                                // Set weight to 5
            meshRenderer.material.color = Color.blue;  // Set color to Blue
        }
        else if (myWeightType == BlockType.Heavy)
        {
            rb.mass = 10f;                               // Set weight to 10
            meshRenderer.material.color = Color.red;     // Set color to Red
        }
    }
}