using UnityEngine;

public class PlayerController : MonoBehaviour
{
    public ThirdPersonMovement playerCam;

    void OnCollisionEnter(Collision collider)
    {
        if (collider.gameObject.CompareTag("Boden"))
        {
            playerCam.PlayerIsGrounded();
        }
    }
}



