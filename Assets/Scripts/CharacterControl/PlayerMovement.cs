using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerMovement : MonoBehaviour
{
    public Rigidbody controller;

    public float speed = 120f;

    // Update is called once per frame
    void Update()
    {
        float x = Input.GetAxis("Horizontal");
        float z = Input.GetAxis("Vertical");

        //Use transform directions for local movement
        Vector3 move = transform.right * x + transform.forward * z;

        //Time.deltaTime to make it framerate independent
        controller.AddForce(move * speed * Time.deltaTime);
        //controller.Move(move * speed * Time.deltaTime);
    }
}
