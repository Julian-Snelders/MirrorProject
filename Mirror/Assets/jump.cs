using Mirror;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class jump : NetworkBehaviour
{
    public CharacterController controller;
    public float jumpSpeed = 2.0f;
    public float gravity = -9.81f;
    public float jumpHeight = 3.0f;

    public Transform groundCheck;
    public float groundDistance = 0.4f;
    public LayerMask groundMask;

    Vector3 velocity;
    bool isGrounded;


   public override void OnStartAuthority()
    {
        enabled = true;
    }



    [ClientCallback]
    void Update()
    {
        Jump();
    }

    [Client]
    void Jump()
    {
        //Jump
        isGrounded = Physics.CheckSphere(groundCheck.position, groundDistance, groundMask); //creating sphere on empty object groundcheck to check ground

        if(isGrounded && velocity.y < 0)
        {
            velocity.y = -2f;
        }

        if(Input.GetKey(KeyCode.Space) && isGrounded)
        {
            velocity.y = Mathf.Sqrt(jumpHeight * -2f * gravity);
        }
        velocity.y += gravity * Time.deltaTime;

        controller.Move(velocity * Time.deltaTime);
    }
}
