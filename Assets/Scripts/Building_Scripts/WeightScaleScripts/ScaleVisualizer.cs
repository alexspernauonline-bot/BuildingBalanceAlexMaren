using UnityEngine;

public class ScaleVisualizer : MonoBehaviour
{
    
    public Transform leftScaleTransform;
    public Transform rightScaleTransform;
   

    public GameManager gameManager;

   //Animationen
    public float maxMovementY = 1.0f;
    public float maxWeightDifference = 10f;
    public float moveSpeed = 5.0f;

    public float weightChangeThreshold = 0.5f;

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
    }

    void FixedUpdate()
    {
        // Sicherheits-Check
        if (leftScaleTransform == null || rightScaleTransform == null || gameManager == null) return;

        // 1. Wir lesen die Gewichte einfach aus dem GameManager ab
        // (Voraussetzung: weightLeft und weightRight sind in deinem GameManager 'public')
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