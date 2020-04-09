using System.Collections;
using System.Collections.Generic;
using UnityEngine;

//Mouse look script to attach to the camera, child of player body. The player body can be dragged into the editor for left right looking
public class MouseLook : MonoBehaviour
{
    public float sensitivity = 100f;

    public Transform playerBody;

    float xRotation = 0f;

    // Start is called before the first frame update
    void Start()
    {
        //To prevent the mouse from clicking out of the screen
        Cursor.lockState = CursorLockMode.Locked;
    }

    // Update is called once per frame
    void Update()
    {
        //Time.delta time is used to prevent variance with different fps
        float mouseX = Input.GetAxis("Mouse X") * sensitivity * Time.deltaTime;
        float mouseY = Input.GetAxis("Mouse Y") * sensitivity * Time.deltaTime;

        xRotation -= mouseY;
        //Prevents player from looking behind them
        xRotation = Mathf.Clamp(xRotation, -90, 90);

        transform.localRotation = Quaternion.Euler(xRotation, 0, 0);
        playerBody.Rotate(Vector3.up * mouseX);
    }
}
