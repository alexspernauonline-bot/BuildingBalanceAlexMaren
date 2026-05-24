using UnityEngine;

public class ScaleSide : MonoBehaviour
{
    // This stores the total weight currently on this side.

    public float currentWeight = 0f;

    // runs automatically when another object enters the Trigger zone.
    private void OnTriggerEnter(Collider other)
    {
        // We check if the object that entered the zone has a Rigidbody (physics) component attached.
        Rigidbody rb = other.GetComponent<Rigidbody>();

        // If 'rb' is NOT null, it means we found a Rigidbody.
        if (rb != null)
        {
            // We take the mass of that object and add it to our total.
            currentWeight += rb.mass;
        }
    }

    // This runs automatically when an object falls out or is taken out of the Trigger zone.
    private void OnTriggerExit(Collider other)
    {
        Rigidbody rb = other.GetComponent<Rigidbody>();

        if (rb != null)
        {
            // We subtract the mass because the object is no longer on the scale.
            currentWeight -= rb.mass;

            // This is a safety measure. Physics engines can sometimes be slightly inaccurate.
            // Mathf.Max ensures our scale's weight never accidentally bugs out and drops below 0.
            currentWeight = Mathf.Max(0f, currentWeight);
        }
    }
}
