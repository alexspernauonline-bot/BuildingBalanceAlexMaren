using UnityEngine;

public class ScaleVisualizer : MonoBehaviour
{
    public Transform leftScaleTransform;
    public Transform rightScaleTransform;

    public GameManager gameManager;

    public MeshRenderer[] pillarRenderers;
    public Material greenBlinkMaterial;
    private Material[] originalMaterials;


    //Animationen
    public float maxMovementY = 1.0f;
    public float maxWeightDifference = 10f;
    public float moveSpeed = 5.0f;

    public float weightChangeThreshold = 0.4f;

    private float startYLeft;
    private float startYRight;

    private float targetYLeft;
    private float targetYRight;
    private float lastStableDifference = 0f;

    void Start()
    {
        // Startpositionen der 3D-Modelle merken
        if (leftScaleTransform != null)
        {
            startYLeft = leftScaleTransform.localPosition.y;
            targetYLeft = startYLeft;
        }
        if (rightScaleTransform != null)
        {
            startYRight = rightScaleTransform.localPosition.y;
            targetYRight = startYRight;
        }
        //für das Blinken der Säule
        if (pillarRenderers != null && pillarRenderers.Length > 0)
        {
            originalMaterials = new Material[pillarRenderers.Length];
            for (int i = 0; i < pillarRenderers.Length; i++)
            {
                if (pillarRenderers[i] != null)
                {
                    originalMaterials[i] = pillarRenderers[i].material;
                }
            }
        }
    }

    public void UpdateBlink(bool isBlinking, float currentTime)
    {
        if (pillarRenderers == null || greenBlinkMaterial == null) return;

        if (isBlinking)
        {
            bool showGreen = Mathf.Repeat(currentTime, 1f) < 0.5f;
            for (int i = 0; i < pillarRenderers.Length; i++)
            {
                if (pillarRenderers[i] != null)
                {
                    pillarRenderers[i].material = showGreen ? greenBlinkMaterial : originalMaterials[i];
                }
            }
        }
        else
        {
            for (int i = 0; i < pillarRenderers.Length; i++)
            {
                if (pillarRenderers[i] != null && originalMaterials != null && i < originalMaterials.Length)
                {
                    pillarRenderers[i].material = originalMaterials[i];
                }
            }
        }
    }

    void FixedUpdate()
    {
        // Sicherheits-Check
        if (leftScaleTransform == null || rightScaleTransform == null || gameManager == null) return;

        // 1. Wir lesen die Gewichte einfach aus dem GameManager ab
        float currentRawDifference = gameManager.weightLeft - gameManager.weightRight;

        // 2. Den Wert normalisieren
        if (Mathf.Abs(currentRawDifference - lastStableDifference) > weightChangeThreshold)
        {
            // Neue stabile Differenz merken
            lastStableDifference = currentRawDifference;

            // Neue Ziel-Höhen berechnen
            float normalizedDiff = Mathf.Clamp(lastStableDifference / maxWeightDifference, -1f, 1f);
            targetYLeft = startYLeft - (normalizedDiff * maxMovementY);
            targetYRight = startYRight + (normalizedDiff * maxMovementY);
        }

        // Vektoren bauen
        Vector3 targetPosLeft = new Vector3(leftScaleTransform.localPosition.x, targetYLeft, leftScaleTransform.localPosition.z);
        Vector3 targetPosRight = new Vector3(rightScaleTransform.localPosition.x, targetYRight, rightScaleTransform.localPosition.z);

        // 5. Sanft animieren
        leftScaleTransform.localPosition = Vector3.Lerp(leftScaleTransform.localPosition, targetPosLeft, Time.deltaTime * moveSpeed);
        rightScaleTransform.localPosition = Vector3.Lerp(rightScaleTransform.localPosition, targetPosRight, Time.deltaTime * moveSpeed);
    }
}