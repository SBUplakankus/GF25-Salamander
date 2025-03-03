// ============================================================================================
// CLASS: PlayerController
// ============================================================================================
// Description:
//   Controls the player movement, dashing and spitting
// ============================================================================================

using System;
using UnityEngine;
using System.Collections;
using System.Collections.Generic;
using Player;
using Systems;
using UnityEngine.Serialization;
using Random = UnityEngine.Random;

public class PlayerController : MonoBehaviour
{
    //adds title on unity
    [Header("Movement")]
    //the players speed
    public float playerSpeed;
    public float playerDrag;
    public float playerDash;
    public Transform cameraTransform;
    private bool _gameOver;
    
    private bool _canDash;
    private const int DashCooldown = 2;

    private bool _canSpit;
    private const int SpitCooldown = 1;

    public Transform playerOrientation;

    private const float RotationSmooth = 10;
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
    
    [FormerlySerializedAs("_spitSound")]
    [Header("Sounds")]
    [SerializeField] private AudioClip spitSound;
    [SerializeField] private AudioClip dashSound;
    [SerializeField] private AudioClip walkSound;
    private AudioSource _audioSource;
    
    private const float StepInterval = 0.4f; // Time between steps
    private float _nextStepTime = 0f; // Timer to track when to play the next step

    public static event Action<int> OnPlayerSpit;
    public static event Action<int> OnPlayerDash; 
    
    void Start()
    {
        _gameOver = false;
        _audioSource = GetComponent<AudioSource>();
        _rb = GetComponent<Rigidbody>();
        _animator = GetComponentInChildren<Animator>();
        _animator.SetFloat("speed", 0);
        _canDash = true;
        _canSpit = true;
        //moistLevel = 0;
    }

    void OnEnable()
    {
        GameManager.OnTimerExpiration += HandleGameOver;
    }

    void OnDisable()
    {
        GameManager.OnTimerExpiration -= HandleGameOver;
    }
    
    // Update is called once per frame
    void Update()
    {
        if (_gameOver) return;
        MyInput();
        MovePlayer();
        
        //for dashing
        if (Input.GetKeyDown(KeyCode.Q) && _canDash)
        {
            PlaySfx(dashSound, 0.8f);
            _rb.AddForce(transform.forward * playerDash, ForceMode.Impulse);
            _animator.SetTrigger("Dash");
            StartCoroutine(DashCooldownCoroutine());
        }
        
        //for attacking
        if (Input.GetKeyDown(KeyCode.E) && _canSpit /*&& moistLevel >= MoistureTakeAway*/)
        {
            PlaySfx(spitSound, 0.3f);
            Invoke("ShootSpit", 0.2f);
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
        Quaternion targetRotate = Quaternion.Euler(0, cameraTransform.eulerAngles.y, 0);
        transform.rotation = Quaternion.Slerp(transform.rotation, targetRotate, RotationSmooth * Time.deltaTime);
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
        if (_horizontalInput == 0 && _verticalInput == 0)
        {
            _animator.SetFloat("speed", 0);
        }
        else
        {
            _animator.SetFloat("speed", 0.5f);
            if (!(Time.time >= _nextStepTime)) return;
            PlaySfx(walkSound, 0.08f);
            _nextStepTime = Time.time + StepInterval;
        }
    }
    
    // Enumerators are functions that run on timers
    
    private IEnumerator DashCooldownCoroutine()
    {
        _canDash = false;
        OnPlayerDash?.Invoke(DashCooldown);
        yield return new WaitForSeconds(DashCooldown);
        _canDash = true;
    }
    
    private IEnumerator SpitCooldownCoroutine()
    {
        _canSpit = false;
        OnPlayerSpit?.Invoke(SpitCooldown);
        yield return new WaitForSeconds(SpitCooldown);
        _canSpit = true;
    }

    /*private void HandleMoistChange(int currentMoistLevel)
    {
        moistLevel = currentMoistLevel;
    }*/
    
    private void PlaySfx(AudioClip clip, float volume)
    {
        _audioSource.volume = volume;
        _audioSource.pitch = Random.Range(0.7f, 1f);
        _audioSource.PlayOneShot(clip);
    }

    private void HandleGameOver()
    {
        _gameOver = true;
    }
    
}
