using UnityEngine;

public class PlacementController : MonoBehaviour
{
    public LayerMask groundLayer; // Hier wählen wir im Inspector "PlacableGround"
    private GameObject currentBlock = null;
    private Rigidbody currentRb = null;

    void Update()
    {
        // 1. STRAHL VON DER MAUS SCHIESSEN
        Ray ray = Camera.main.ScreenPointToRay(Input.mousePosition);

        // Finde heraus, wo die Maus auf dem Boden/der Waage hinzeigt
        if (Physics.Raycast(ray, out RaycastHit hit, Mathf.Infinity, groundLayer))
        {
            // ZUSTAND A: Wir tragen gerade einen Block
            if (currentBlock != null)
            {
                // Verschiebe den Block flüssig zum Mauszeiger (und setze ihn 1.5 Einheiten höher, damit er nicht im Boden versinkt)
                currentBlock.transform.position = hit.point + Vector3.up * 1.0f;

                // ROTATION: Wenn R gedrückt wird, drehe den Block um 45 Grad auf der Y-Achse
                if (Input.GetKeyDown(KeyCode.R))
                {
                    currentBlock.transform.Rotate(0f, 45f, 0f);
                }

                // PLATZIEREN: Wenn wir nochmal klicken, lassen wir den Block fallen
                if (Input.GetMouseButtonDown(0))
                {
                    currentRb.isKinematic = false; // Physik wieder an!
                    currentBlock = null;           // Hand wieder frei!
                }
            }
            // ZUSTAND B: Hand ist leer, wir wollen einen Block aufheben
            else
            {
                if (Input.GetMouseButtonDown(0))
                {
                    // Prüfe, ob wir mit der Maus direkt auf einen Baustein geklickt haben
                    RaycastHit hitBlock;
                    if (Physics.Raycast(ray, out hitBlock, Mathf.Infinity))
                    {
                        if (hitBlock.collider.CompareTag("Block"))
                        {
                            currentBlock = hitBlock.collider.gameObject;
                            currentRb = currentBlock.GetComponent<Rigidbody>();

                            if (currentRb != null)
                            {
                                currentRb.isKinematic = true; // Physik aus, damit er starr an der Maus klebt
                            }
                        }
                    }
                }
            }
        }
    }
}