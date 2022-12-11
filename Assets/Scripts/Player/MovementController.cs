using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MovementController : MonoBehaviour
{
    public Controlls ctrls;
    public CharacterController controller;
    public PlayerInventory inventory;
    public bool canMove;
    [Space]
    public Transform groundCheck;
    public float checkDistance;
    public LayerMask groundMask;
    [Space]
    public float currentSpeed;
    public float gravity = -20;
    public float jumpHeight;
    [Space]
    public float walkSpeed;
    public float runSpeed;
    public float crouchSpeed;
    [Space]
    public AudioSource stepAudioSrc;
    public AudioClip[] groundSteps;
    public AudioClip[] normalSteps;
    public AudioClip landingSound;
    public float stepFrequency;
    float stepTimer; 

    public Vector3 moveDir;
    Vector3 velocity;
    bool isGrounded;
    bool isRunning;
    public bool isWalking;
    bool jumped;
    float jumpTimer;

    private void Awake()
    {
        ctrls = new Controlls();
    }
    private void OnEnable()
    {
        ctrls.Enable();
    }
    private void OnDisable()
    {
        ctrls.Disable();
    }
    private void Update()
    {
        if (canMove)
        {
            Vector2 move = ctrls.Player.Movement.ReadValue<Vector2>();
            moveDir = transform.right * move.x + transform.forward * move.y;
            if (ctrls.Player.Jump.triggered && isGrounded && !inventory.isShopUIOpen)
            {
                velocity.y = Mathf.Sqrt(jumpHeight * -2 * gravity);
                jumped = true;
            }

            if (ctrls.Player.Run.ReadValue<float>() > .1 && isGrounded)
            {
                isRunning = true;
            }
            else
                isRunning = false;
            if (!inventory.isShopUIOpen && (controller.velocity.x > 1 || controller.velocity.z > 1 || controller.velocity.z < -1 || controller.velocity.z < -1))
            {
                isWalking = true;
                stepTimer += Time.deltaTime;
                if (stepTimer >= stepFrequency && isGrounded)
                {
                    PlayStepSound();
                    stepTimer = 0;
                }
            }
            else
            {
                isWalking = false;
                stepTimer = 0;
            }
        }
    }
    public void PlayStepSound()
    {
        Collider[] hitColliders = Physics.OverlapSphere(groundCheck.position, checkDistance);
        if (hitColliders[0].gameObject.layer == 8)
        {
            stepAudioSrc.clip = groundSteps[Random.Range(0, groundSteps.Length)];
            stepAudioSrc.Play();
        }
        else
        {
            stepAudioSrc.clip = normalSteps[Random.Range(0, normalSteps.Length)];
            stepAudioSrc.Play();
        }    
    }
    private void FixedUpdate()
    {
        if (canMove)
        {
            if (isRunning)
            {
                currentSpeed = Mathf.MoveTowards(currentSpeed, runSpeed, 2 * Time.fixedDeltaTime);
            }
            else
            {
                currentSpeed = Mathf.MoveTowards(currentSpeed, walkSpeed, 2 * Time.fixedDeltaTime);
            }

            isGrounded = Physics.CheckSphere(groundCheck.position, checkDistance, groundMask);
            if (isGrounded && velocity.y < 0)
                velocity.y = -2;

            if (jumped)
            {
                stepTimer = 0;
                jumpTimer += Time.deltaTime;
                if (jumpTimer >= .2f && isGrounded)
                {
                    stepAudioSrc.clip = landingSound;
                    stepAudioSrc.Play();
                    jumped = false;
                    jumpTimer = 0;
                }
            }

            if (!inventory.isShopUIOpen)
            {
                velocity.x = moveDir.x * currentSpeed;
                velocity.z = moveDir.z * currentSpeed;
                velocity.y += gravity * Time.fixedDeltaTime;
                controller.Move(velocity * Time.fixedDeltaTime);
            }
        }
    }
}
