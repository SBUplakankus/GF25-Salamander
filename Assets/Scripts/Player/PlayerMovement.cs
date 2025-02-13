using UnityEngine;
using System.Collections;
using System.Collections.Generic;

//https://www.youtube.com/watch?v=f473C43s8nE&t 11/02/25
public class Movement : MonoBehaviour
{
    //adds title on unity
    [Header("Movement")]
    //the players speed
    public float playerSpeed;
    public float playerJumpForce;
    public float playerDrag;

    public Transform playerOrientation;
    //inputs
    private float horizontalInput;
    private float verticalInput;
    
    Vector3 moveDirection;
    Rigidbody rb;
    
    void Start()
    {
        rb = GetComponent<Rigidbody>();
    }
    
    // Update is called once per frame
    void Update()
    {
        MyInput();
        MovePlayer();
        
        if (Input.GetKeyDown(KeyCode.Space))
        {
            rb.AddForce(Vector3.up * playerJumpForce, ForceMode.Impulse);
        }
    }
    
    void FixedUpdate()
    {
        Vector3 velocity = rb.linearVelocity;

        // Apply drag only to X and Z
        velocity.x *= playerDrag; 
        velocity.z *= playerDrag;

        rb.linearVelocity = velocity;
    }
    
    private void MyInput()
    {
        horizontalInput = Input.GetAxisRaw("Horizontal");
        verticalInput = Input.GetAxisRaw("Vertical");
    }

    private void MovePlayer()
    {
        //calculates movement direction
        moveDirection = playerOrientation.forward * verticalInput + playerOrientation.right * horizontalInput;

        //applies the force to the player
        rb.AddForce(moveDirection * playerSpeed, ForceMode.Force);
    }
}
