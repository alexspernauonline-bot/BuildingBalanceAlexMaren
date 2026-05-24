using UnityEngine;

public class PhysicalSnap : MonoBehaviour
{
    private bool hasSnapped = false;
    private Rigidbody rb;

    void Start()
    {
        rb = GetComponent<Rigidbody>();
    }

    private void OnCollisionEnter(Collision collision)
    {
        // 1. Did we hit a block? And have we not snapped yet?
        if (hasSnapped == false && collision.gameObject.CompareTag("Block"))
        {
            // 2. THE SAFETY CHECK: Did we land ON TOP of the block?
            // If the player accidentally bumped the SIDE of a tower, we don't want to teleport 
            // inside of it. We only snap if our block is physically higher than the one we hit.
            if (transform.position.y > collision.transform.position.y)
            {
                // 3. THE SNAP (Alignment only!)
                // We perfectly match the X and Z coordinates of the bottom block, 
                // but we DO NOT touch the Y (height) coordinate.
                Vector3 bottomPos = collision.transform.position;
                transform.position = new Vector3(bottomPos.x, transform.position.y, bottomPos.z);

                // 4. KILL THE WIGGLE
                // We instantly stop the block from bouncing sideways or spinning, 
                // so it drops straight down flush onto the block below it.
                rb.linearVelocity = new Vector3(0f, rb.linearVelocity.y, 0f);
                rb.angularVelocity = Vector3.zero;

                // Notice what we DIDN'T do: We didn't use isKinematic. We didn't use FixedJoints.
                // Physics is still fully alive!

                hasSnapped = true;
            }
        }
    }
}