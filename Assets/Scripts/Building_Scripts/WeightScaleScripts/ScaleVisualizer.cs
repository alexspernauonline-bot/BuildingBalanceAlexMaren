using UnityEngine;
using UnityEngine.UI;
using TMPro;

public class ScaleVisualizer : MonoBehaviour
{
    public Transform leftScaleTransform;
    public Transform rightScaleTransform;

    public GameManager gameManager;

    public MeshRenderer[] pillarRenderers;
    public Material greenBlinkMaterial;
    private Material[] originalMaterials;

    // UI BAROMETER 
    [Header("UI Barometer")]
    public RectTransform barometerNeedle; // Das RectTransform der Zeiger-Nadel
    public float maxNeedleAngle = 60f;    // Maximaler Ausschlagswinkel (z.B. -60° bis +60°)
    private Quaternion startNeedleRotation; //Die Grund-Drehung der Nadel

    // 3D-Animationen der Indicator Säulen
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
        if (barometerNeedle != null)
        {
            startNeedleRotation = barometerNeedle.localRotation;
        }
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

        // 1. Differenz auslesen
        float currentRawDifference = gameManager.weightLeft - gameManager.weightRight;

        // 2. WICHTIG: normalizedDiff HIER deklarieren (damit es überall in FixedUpdate verfügbar ist!)
        float normalizedDiff = Mathf.Clamp(currentRawDifference / maxWeightDifference, -1f, 1f);

        // 3. 3D-Säulen Animation berechnen
        if (Mathf.Abs(currentRawDifference - lastStableDifference) > weightChangeThreshold)
        {
            // Neue stabile Differenz merken
            lastStableDifference = currentRawDifference;
            // Neue Ziel-Höhen berechnen
            targetYLeft = startYLeft - (normalizedDiff * maxMovementY);
            targetYRight = startYRight + (normalizedDiff * maxMovementY);
        }

        // Vektoren bauen & 3D-Modelle bewegen
        Vector3 targetPosLeft = new Vector3(leftScaleTransform.localPosition.x, targetYLeft, leftScaleTransform.localPosition.z);
        Vector3 targetPosRight = new Vector3(rightScaleTransform.localPosition.x, targetYRight, rightScaleTransform.localPosition.z);

        leftScaleTransform.localPosition = Vector3.Lerp(leftScaleTransform.localPosition, targetPosLeft, Time.deltaTime * moveSpeed);
        rightScaleTransform.localPosition = Vector3.Lerp(rightScaleTransform.localPosition, targetPosRight, Time.deltaTime * moveSpeed);


        // 4. UI-BAROMETER ZEIGER DREHEN
        if (barometerNeedle != null)
        {
            float tiltAngle = normalizedDiff * maxNeedleAngle;

            // Quaternionen multipliziert man, um Rotationen zu addieren!
            // Wir nehmen die gemerkte Start-Drehung und rechnen unseren Ausschlag obendrauf.
            Quaternion targetRotation = startNeedleRotation * Quaternion.Euler(0, 0, tiltAngle);

            // Sanfte Drehung der Nadel
            barometerNeedle.localRotation = Quaternion.Lerp(barometerNeedle.localRotation, targetRotation, Time.deltaTime * moveSpeed);
        }
    
    }
}