using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.InputSystem;

public class PlayerController : MonoBehaviour
{
    private CharacterController controller;
    private Vector3 playerVelocity;
    private bool IsGrounded;
    public float gravity = -9.8f;
    public float moveSpeed = 5f;

    public float shootForce = 10f;
    public float pushBackForce = 1f;

    public Transform cameraTransform; // Assign the main camera's transform in the inspector
    public Transform shootPoint; // Point from where the player shoots the ball

    private Vector2 lookInput;
    private Vector2 moveInput;
    public float lookSensitivity = 2f;

    private Animator animator;
    private void Start()
    {
        controller = GetComponent<CharacterController>();
        animator = GetComponent<Animator>();
        Cursor.lockState = CursorLockMode.Locked;
    }

    private void Update()
    {
        IsGrounded = controller.isGrounded;
        transform.Rotate(0, lookInput.x * lookSensitivity, 0);

        // Camera rotation (vertical)
        Vector3 cameraRotation = cameraTransform.localEulerAngles;
        cameraRotation.x -= lookInput.y * lookSensitivity;
        cameraRotation.x = Mathf.Clamp(cameraRotation.x, -90f, 90f); // Limit camera pitch
        cameraTransform.localEulerAngles = cameraRotation;
    }

    public void TriggerJumpAnimation()
    {
        if (animator != null)
        {
            animator.SetTrigger("Jump"); // Trigger jump animation
        }
    }

    public void processMove(Vector2 input)
    {
        Vector3 moveDirection = Vector3.zero;

        moveDirection.x = input.x; // Horizontal movement
        moveDirection.z = input.y; // Forward/backward movement (Z axis in 3D)

        controller.Move(moveDirection * moveSpeed * Time.deltaTime);
        playerVelocity.y += gravity * Time.deltaTime;
        if (IsGrounded && playerVelocity.y < 0)
        {
            playerVelocity.y = -2f;
        }

        controller.Move(playerVelocity * Time.deltaTime);
    }

    public void ProcessLook(Vector2 input)
    {
        lookInput = input; // Save the look input
    }

    public void Shoot()
    {
        // Find the ball by tag
        GameObject ball = GameObject.FindGameObjectWithTag("Ball");
        if (ball != null)
        {
            Rigidbody ballRb = ball.GetComponent<Rigidbody>();
            if (ballRb != null)
            {
                // Calculate shoot direction
                Vector3 shootDirection = (ball.transform.position - shootPoint.position).normalized;

                // Apply force to the ball
                ballRb.AddForce(shootDirection * shootForce, ForceMode.Impulse);
                SoundManager.Instance.Play(Sounds.Ball);
            }
        }
    }

    private void OnCollisionEnter(Collision collision)
    {
        if (collision.gameObject.CompareTag("Ball"))
        {
            // Apply a small force to push the player back
            Rigidbody ballRb = collision.gameObject.GetComponent<Rigidbody>();
            if (ballRb != null)
            {
                Vector3 pushDirection = (transform.position - collision.transform.position).normalized;
                ballRb.AddForce(pushDirection * pushBackForce, ForceMode.Impulse);
            }
        }
    }


}
