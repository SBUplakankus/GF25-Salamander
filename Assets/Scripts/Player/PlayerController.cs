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
    
    // Jump cooldown, doesn't need to be a const
    private bool _canJump;
    private const float JumpCooldown = 2f;
    
    private bool _canDash;
    private const float DashCooldown = 2f;

    public Transform playerOrientation;
    //inputs
    private float _horizontalInput;
    private float _verticalInput;
    
    private Vector3 _moveDirection;
    private Rigidbody _rb;
    
    [Header("Attack")]
    public GameObject spitProjectile;
    public float projectileSpeed;
    
    void Start()
    {
        _rb = GetComponent<Rigidbody>();
        _canJump = true;
        _canDash = true;
    }
    
    // Update is called once per frame
    void Update()
    {
        MyInput();
        MovePlayer();
        
        // Only jump when space is pressed and the bool is true
        if (Input.GetKeyDown(KeyCode.Space) && _canJump)
        {
            _rb.AddForce(Vector3.up * playerJumpForce, ForceMode.Impulse);
            
            // To call Enumerators you need to wrap the function inside of a StartCoroutine() call
            StartCoroutine(JumpCooldownCoroutine());
        }

        //for dashing
        if (Input.GetKeyDown(KeyCode.Q) && _canDash)
        {
            _rb.AddForce(transform.forward * playerDash, ForceMode.Impulse);
            
            StartCoroutine(DashCooldownCoroutine());
        }
        
        //for attacking
        if (Input.GetKeyDown(KeyCode.E))
        {
            //gets player v3 and then adds the forward of the player x amount and then x amount up 
            Vector3 spawnPosition = transform.position + (transform.forward * 0.65f) + (Vector3.up * 0.7f);
            var spitClone = Instantiate(spitProjectile, spawnPosition, Quaternion.identity);
            spitClone.GetComponent<Rigidbody>().AddForce(transform.forward * projectileSpeed, ForceMode.Impulse);
            //Destroy(spitClone, 10f); //data loss cant do :(
            //need to takeaway moisture
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
    
    // Enumerators are functions that run on timers
    private IEnumerator JumpCooldownCoroutine()
    {
        // Sets can jump to false then waits for seconds based on the cooldown time
        _canJump = false;
        yield return new WaitForSeconds(JumpCooldown);
        _canJump = true;
    }
    
    private IEnumerator DashCooldownCoroutine()
    {
        _canDash = false;
        yield return new WaitForSeconds(DashCooldown);
        _canDash = true;
    }
    
}
