using UnityEngine;
using System.Collections;
using System.Collections.Generic;

public class Movement : MonoBehaviour
{
    //adds title on unity
    [Header("Movement")]
    //the players speed
    public float playerSpeed;
    public float playerJumpForce;
    public float playerDrag;
    public float playerDash;
    public Transform cameraTransform; 

    public Transform playerOrientation;
    //inputs
    private float _horizontalInput;
    private float _verticalInput;
    
    private Vector3 _moveDirection;
    private Rigidbody _rb;
    
    void Start()
    {
        _rb = GetComponent<Rigidbody>();
    }
    
    // Update is called once per frame
    void Update()
    {
        MyInput();
        MovePlayer();
        
        //for jumping
        if (Input.GetKeyDown(KeyCode.Space))
        {
            _rb.AddForce(Vector3.up * playerJumpForce, ForceMode.Impulse);
        }

        //for dashing
        if (Input.GetKeyDown(KeyCode.F))
        {
            _rb.AddForce(transform.forward * playerDash, ForceMode.Impulse);
        }
    }
    
    void FixedUpdate()
    {
        Vector3 velocity = _rb.linearVelocity;

        // Apply drag only to X and Z
        velocity.x *= playerDrag; 
        velocity.z *= playerDrag;

        _rb.linearVelocity = velocity;
        
        //rotating player based of camera view
        transform.rotation = Quaternion.Euler(0, cameraTransform.eulerAngles.y, 0);
    }
    
    private void MyInput()
    {
        _horizontalInput = Input.GetAxisRaw("Horizontal");
        _verticalInput = Input.GetAxisRaw("Vertical");
    }

    private void MovePlayer()
    {
        //calculates movement direction
        _moveDirection = playerOrientation.forward * _verticalInput + playerOrientation.right * _horizontalInput;

        //applies the force to the player
        _rb.AddForce(_moveDirection * playerSpeed, ForceMode.Force);
    }
}
