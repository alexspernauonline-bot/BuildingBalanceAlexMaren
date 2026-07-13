using UnityEngine;

public class RotationResetArea : MonoBehaviour
{
    private void OnCollisionEnter(Collision collision)
    {
        // Reagiert nur auf deine Spieler-Blöcke
        if (collision.gameObject.CompareTag("Block"))
        {
            Rigidbody rb = collision.rigidbody;
            if (rb != null)
            {
                // in welche Richtung der Block gerade schaut (Y-Achse)
                float currentYRotation = collision.transform.rotation.eulerAngles.y;

                // X und Z auf exakt 0, damit er perfekt aufrecht steht
                collision.transform.rotation = Quaternion.Euler(0f, currentYRotation, 0f);

               
                rb.linearVelocity = Vector3.zero;
                rb.angularVelocity = Vector3.zero;
            }
        }
    }
}