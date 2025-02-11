using UnityEngine;
using System.Collections;
using System.Collections.Generic;

//https://www.youtube.com/watch?v=f473C43s8nE&t 11/02/25
public class Movement : MonoBehaviour
{
    //adds title on unity
    [Header("Movement")] 
    //the players speed
    public float moveSpeed;
    //player orientation
    public Transform orientation;

    float horizontalInput;
    float verticalInput;
    
    Vector3 moveDirection;
    Rigidbody rb;
    
    void Start()
    {
        rb = GetComponent<Rigidbody>();
        rb.freezeRotation = true;
    }
    
    // Update is called once per frame
    void Update()
    {
        MyInput();
    }

    private void FixedUpdate()
    {
        MovePlayer();
    }
    
    private void MyInput()
    {
        horizontalInput = Input.GetAxisRaw("Horizontal");
        verticalInput = Input.GetAxisRaw("Vertical");
    }

    private void MovePlayer()
    {
        //calculates movement direction
        moveDirection = orientation.forward * verticalInput + orientation.right * horizontalInput;
        
        rb.AddForce(moveDirection * moveSpeed * moveSpeed * 10f, ForceMode.Force);
    }
}
