using UnityEngine;

public enum BlockWeightCategory
{
    Light,
    Medium,
    Heavy
}

public class BlockIdentifier : MonoBehaviour
{
    // Hier kannst du im Inspector die Gewichtsklasse einstellen
    public BlockWeightCategory myCategory;

    // Hier merkt sich der Block später heimlich seine echte Farbe
    public Material trueMaterial;
}