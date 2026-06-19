using UnityEngine;

public class CameraContoller : MonoBehaviour
{
    public float speedRotation;
    // Start is called once before the first execution of Update after the MonoBehaviour is created
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        float horizontalInput = Input.GetAxis("Horizontal");
        transform.Rotate(Vector3.up * Time.deltaTime * speedRotation * horizontalInput * -1);
        float verticalInput = Input.GetAxis("Vertical");
        transform.Rotate(Vector3.right * Time.deltaTime * speedRotation * verticalInput * -1);
    }
}
