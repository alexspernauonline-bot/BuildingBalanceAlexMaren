
using UnityEngine;
public Material trueMaterial; // Variable, um das ursprüngliche Material zu speichern
// Ein einfaches Enum (eine Auswahlliste) für das Dropdown-Menü im Inspector
public enum BlockWeightCategory
{
    Light,
    Medium,
    Heavy
}

public class BlockIdentifier : MonoBehaviour
{
    // Hier kannst du im Inspector für jeden Block im Prefab einstellen, was er sein soll
    public BlockWeightCategory myCategory;
}