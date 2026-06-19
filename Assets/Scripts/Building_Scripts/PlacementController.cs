using UnityEngine;

public class PlacementController : MonoBehaviour
{
    [SerializeField] private float smoothSpeed = 15f;
    [SerializeField] private float liftHeight = 0.75f;
    [SerializeField] private float ghostTransparency = 0.4f; // 40% Sichtbarkeit für den Geist

    private GameObject currentBlock = null;
    private Rigidbody currentRb = null;
    private MeshRenderer currentRenderer = null;
    private Color originalColor;
    private Vector3 targetPosition;

    void Update()
    {
        Ray ray = Camera.main.ScreenPointToRay(Input.mousePosition);

        // ZUSTAND A: Wir tragen gerade den "Geist" des Blocks
        if (currentBlock != null)
        {
            // Der Strahl tastet den Untergrund ab
            if (Physics.Raycast(ray, out RaycastHit hit, Mathf.Infinity))
            {
                targetPosition = hit.point + Vector3.up * liftHeight;
                currentBlock.transform.position = Vector3.Lerp(currentBlock.transform.position, targetPosition, Time.deltaTime * smoothSpeed);
            }

            // Rotieren mit R
            if (Input.GetKeyDown(KeyCode.R))
            {
                currentBlock.transform.Rotate(0f, 45f, 0f);
            }

            // Absetzen mit Linksklick
            if (Input.GetMouseButtonDown(0))
            {
                // 1. Transparenz wieder rückgängig machen (Original-Farbe wiederherstellen)
                if (currentRenderer != null)
                {
                    currentRenderer.material.color = originalColor;
                }

                // 2. Collider wieder einschalten, damit er physikalisch landet
                Collider blockCollider = currentBlock.GetComponentInChildren<Collider>();
                if (blockCollider != null) blockCollider.enabled = true;

                // 3. Physik wieder aktivieren
                if (currentRb != null)
                {
                    currentRb.isKinematic = false;
                }

                currentBlock = null;
            }
        }
        // ZUSTAND B: Hand ist leer, wir suchen nach einem Block zum Aufheben
        else
        {
            if (Input.GetMouseButtonDown(0))
            {
                if (Physics.Raycast(ray, out RaycastHit hit, Mathf.Infinity))
                {
                    if (hit.collider.CompareTag("Block"))
                    {
                        GameObject clickedObject = hit.collider.gameObject;

                        if (clickedObject.transform.parent != null && clickedObject.transform.parent.CompareTag("Block"))
                        {
                            currentBlock = clickedObject.transform.parent.gameObject;
                        }
                        else
                        {
                            currentBlock = clickedObject;
                        }

                        currentRb = currentBlock.GetComponent<Rigidbody>();
                        currentRenderer = currentBlock.GetComponentInChildren<MeshRenderer>();

                        if (currentRb != null)
                        {
                            currentRb.isKinematic = true;
                        }

                        // JETZT MACHEN WIR DEN BLOCK ZUM GEIST:

                        // 1. Collider komplett ausschalten (Er kann nichts mehr wegschubsen!)
                        Collider blockCollider = currentBlock.GetComponentInChildren<Collider>();
                        if (blockCollider != null) blockCollider.enabled = false;

                        // 2. Material auf transparent schalten
                        if (currentRenderer != null)
                        {
                            // Wir merken uns die echte Farbe für später
                            originalColor = currentRenderer.material.color;

                            // Wir setzen den Alpha-Wert runter
                            Color ghostColor = originalColor;
                            ghostColor.a = ghostTransparency;

                            // Wichtig für Unity-Standard-Shader: Rendering Mode auf Transparent zwingen
                            currentRenderer.material.SetInt("_SrcBlend", (int)UnityEngine.Rendering.BlendMode.SrcAlpha);
                            currentRenderer.material.SetInt("_DstBlend", (int)UnityEngine.Rendering.BlendMode.OneMinusSrcAlpha);
                            currentRenderer.material.SetInt("_ZWrite", 0);
                            currentRenderer.material.DisableKeyword("_ALPHATEST_ON");
                            currentRenderer.material.EnableKeyword("_ALPHABLEND_ON");
                            currentRenderer.material.DisableKeyword("_ALPHAPREMULTIPLY_ON");
                            currentRenderer.material.renderQueue = 3000;

                            currentRenderer.material.color = ghostColor;
                        }
                    }
                }
            }
        }
    }
}