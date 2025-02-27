using System;
using UnityEngine;
using System.Collections;
using System.Collections.Generic;
using Player;

public class PlayerController : MonoBehaviour
{
    //adds title on unity
    [Header("Movement")]
    //the players speed
    public float playerSpeed;
    public float playerDrag;
    public float playerDash;
    public Transform cameraTransform;
    
    private bool _canDash;
    private const float DashCooldown = 2f;

    private bool _canSpit;
    private const float SpitCooldown = 1f;

    public Transform playerOrientation;
    //inputs
    private float _horizontalInput;
    private float _verticalInput;
    
    private Vector3 _moveDirection;
    private Rigidbody _rb;
    
    [Header("Attack")]
    public GameObject spitProjectile;
    public float projectileSpeed;
    private const int MoistureTakeAway = 10;
    //private int moistLevel;
    
    //animation
    private Animator _animator;
    
    //public static event Action<int> OnPlayerSpit;
    
    void Start()
    {
        _rb = GetComponent<Rigidbody>();
        _animator = GetComponentInChildren<Animator>();
        _animator.SetFloat("speed", 0);
        _canDash = true;
        _canSpit = true;
        //moistLevel = 0;
    }

    /*void onEnable()
    {
        PlayerAttributes.OnMoistLevelChanged += HandleMoistChange;
    }

    void onDisable()
    {
        PlayerAttributes.OnMoistLevelChanged -= HandleMoistChange;
    }*/
    
    // Update is called once per frame
    void Update()
    {
        MyInput();
        MovePlayer();
        
        //for dashing
        if (Input.GetKeyDown(KeyCode.Q) && _canDash)
        {
            _rb.AddForce(transform.forward * playerDash, ForceMode.Impulse);
            
            StartCoroutine(DashCooldownCoroutine());
        }
        
        //for attacking
        if (Input.GetKeyDown(KeyCode.E) && _canSpit /*&& moistLevel >= MoistureTakeAway*/)
        {
            ShootSpit();
            StartCoroutine(SpitCooldownCoroutine());
            //OnPlayerSpit?.Invoke(MoistureTakeAway);
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
        Animate();
        //applies the force to the player
        _rb.AddForce(_moveDirection * playerSpeed, ForceMode.Force);
    }

    private void ShootSpit()
    {
        //gets player v3 and then adds the forward of the player x amount and then x amount up 
        Vector3 spawnPosition = transform.position + (transform.forward * 2f) ;
        var spitClone = Instantiate(spitProjectile, spawnPosition, Quaternion.identity);
        spitClone.GetComponent<Rigidbody>().AddForce((transform.forward + Vector3.up * 0.2f) * projectileSpeed, ForceMode.Impulse);
    }

    private void Animate()
    {
        //_animator.SetFloat("speed", 0.5f);
        if (_canDash)
        {
            if (_horizontalInput == 0 && _verticalInput == 0)
            {
                _animator.SetFloat("speed", 0);
            }
            else
            {
                _animator.SetFloat("speed", 0.5f);
            }
        }
        else
        {
            _animator.SetFloat("speed", 1);
        }
    }
    
    // Enumerators are functions that run on timers
    
    private IEnumerator DashCooldownCoroutine()
    {
        _canDash = false;
        yield return new WaitForSeconds(DashCooldown);
        _canDash = true;
    }
    
    private IEnumerator SpitCooldownCoroutine()
    {
        _canSpit = false;
        yield return new WaitForSeconds(SpitCooldown);
        _canSpit = true;
    }

    /*private void HandleMoistChange(int currentMoistLevel)
    {
        moistLevel = currentMoistLevel;
    }*/
    
}
