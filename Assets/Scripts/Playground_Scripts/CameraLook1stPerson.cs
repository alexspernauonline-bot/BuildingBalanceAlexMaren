using UnityEngine;

public class CameraLook1stPerson : MonoBehaviour
{
    public Transform playerRef;
    public float mouseSensitivity = 5f;
    private float xRotation = 0f;

    // Start is called once before the first execution of Update after the MonoBehaviour is created
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        float mouseX = Input.GetAxis("Mouse X") * mouseSensitivity;
        float mouseY = Input.GetAxis("Mouse Y") * mouseSensitivity;

        xRotation -= mouseX;
        xRotation = Mathf.Clamp(xRotation, -90f, -90f);
        //Transform.localRotation = Quaternion.Euler(xRotation, 0f, 0f);
        playerRef.Rotate(Vector3.up * mouseX);
    }
}
