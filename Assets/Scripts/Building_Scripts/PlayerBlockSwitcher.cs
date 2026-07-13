using UnityEngine;

public class PlayerBlockSwitcher : MonoBehaviour
{
    public BlockWeightCategory currentCategory = BlockWeightCategory.Light;

    // OnMouseDown ist eine eingebaute Unity-Funktion. 
    // Sie wird automatisch ausgelöst, wenn du mit der Maus auf den Collider des Objekts klickst.
    private void OnMouseOver()
    {
        if (Input.GetMouseButtonDown(1) || Input.GetKeyDown(KeyCode.F))
        {
            SwitchToNextCategory();
        }
    }

    public void SwitchToNextCategory()
    {
        // 1. Die Kategorie einen Schritt weiter schalten
        if (currentCategory == BlockWeightCategory.Light)
        {
            currentCategory = BlockWeightCategory.Medium;
        }
        else if (currentCategory == BlockWeightCategory.Medium)
        {
            currentCategory = BlockWeightCategory.Heavy;
        }
        else if (currentCategory == BlockWeightCategory.Heavy)
        {
            currentCategory = BlockWeightCategory.Light; // Wieder zurück zum Anfang
        }

        // 2. Den GameManager anweisen, das physikalische Gewicht und die Farbe sofort zu ändern!
        if (GameManager.Instance != null)
        {
            GameManager.Instance.ConfigurePlayerBlock(gameObject, currentCategory);
        }
    }
}
