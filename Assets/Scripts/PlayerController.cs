using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerController : MonoBehaviour
{
    private CharacterController controller;

    public float speed = 6;
    public float jumpForce = 8;
    public float gravity = -9.81f;

    private Vector3 velocity;

    private void Start()
    {
        controller = GetComponent<CharacterController>();
    }

    private void Update()
    {
        if (controller.isGrounded)
        {
            float x = Input.GetAxis("Horizontal");
            float z = Input.GetAxis("Vertical");
            velocity = transform.TransformDirection(new Vector3(x, 0f, z)).normalized;
            velocity *= speed;

            if (Input.GetKeyDown(KeyCode.Space))
                velocity.y += jumpForce;
        }

        velocity.y += gravity * Time.deltaTime;
        controller.Move(velocity * Time.deltaTime);
    }
}
