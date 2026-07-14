using UnityEngine;

public class TagResetZone : MonoBehaviour
{
    
    // (z.B. "Untagged" oder "ReserveBlock")
    public string Tag = "Block";

    // Unity ruft das automatisch auf, sobald ein Block in den BoxCollider fällt oder darin spawnt
    private void OnTriggerEnter(Collider other)
    {
        // Wir prüfen zur Sicherheit, ob das Objekt wirklich einer deiner Spieler-Blöcke ist
        if (other.GetComponent<PlayerBlockSwitcher>() != null)
        {
            other.gameObject.tag = Tag;
        }
    }
}