using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class FirstPersonCameraControl : MonoBehaviour
{
    public float mouseSensitivity = 300f;

    float xRotation = 0f;
    
    [SerializeField] Transform player;
    
    float mouseX, mouseY;
    // Start is called before the first frame update
    void Start()
    {
        
    }
    
    void Update()
    {
        CameraControl();
    }

    void CameraControl()
    {
        mouseX = Input.GetAxis("Mouse X") * mouseSensitivity * Time.deltaTime;
        mouseY = Input.GetAxis("Mouse Y") * mouseSensitivity * Time.deltaTime;
        xRotation -= mouseY;
        xRotation = Mathf.Clamp(xRotation, -65, 65);
        //mouseY = Mathf.Clamp(mouseY, -65, 65);


        transform.localRotation = Quaternion.Euler(xRotation, 0f, 0f);
        player.Rotate(Vector3.up * mouseX);
        //player.rotation = Quaternion.Euler(mouseY, mouseX, 0);
    }
}
