using UnityEngine;

public class PlacementController : MonoBehaviour
{
    private GameObject currentBlock = null;
    private Rigidbody currentRb = null;

    void Update()
    {
        // WIR SCHIESSEN NUR NOCH EINEN EINZIGEN STRAHL!
        Ray ray = Camera.main.ScreenPointToRay(Input.mousePosition);

        if (Physics.Raycast(ray, out RaycastHit hit, Mathf.Infinity))
        {
            // ZUSTAND A: Wir tragen gerade einen Block und wollen ihn absetzen
            if (currentBlock != null)
            {
                currentBlock.transform.position = hit.point + Vector3.up * 0.75f;

                if (Input.GetKeyDown(KeyCode.R))
                {
                    currentBlock.transform.Rotate(0f, 45f, 0f);
                }

                if (Input.GetMouseButtonDown(0))
                {
                    currentRb.isKinematic = false; // Physik wieder an
                    currentBlock = null;           // Hand frei
                }
            }
            // ZUSTAND B: Hand ist leer, wir wollen den getroffenen Block AUFHEBEN
            else
            {
                if (Input.GetMouseButtonDown(0))
                {
                    // Wir prüfen direkt den ALLERERSTEN Strahl, den wir oben geschossen haben!
                    if (hit.collider.CompareTag("Block"))
                    {
                        GameObject clickedObject = hit.collider.gameObject;

                        // Sicherheitsnetz für Kind-Collider (Brücke/Dach)
                        if (clickedObject.transform.parent != null && clickedObject.transform.parent.CompareTag("Block"))
                        {
                            currentBlock = clickedObject.transform.parent.gameObject;
                        }
                        else
                        {
                            currentBlock = clickedObject;
                        }

                        currentRb = currentBlock.GetComponent<Rigidbody>();

                        if (currentRb != null)
                        {
                            currentRb.isKinematic = true; // Starr an die Maus heften
                        }
                    }
                }
            }
        }
    }
}