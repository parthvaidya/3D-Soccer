using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ComputerController : MonoBehaviour
{
    public Transform ball;
    public Transform goalPost;
    public float moveSpeed = 3f;
    public float shootForce = 8f;
    public float detectionRange = 1.5f; // Range to detect if AI is too close to the ball

    private CharacterController controller;

    private void Awake()
    {
        controller = GetComponent<CharacterController>();
    }

    private void FixedUpdate()
    {
        if (ball != null)
        {
            // Calculate direction to move towards the ball
            Vector3 directionToBall = (ball.position - transform.position).normalized;

            // Check if the AI is too close to the ball
            if (Vector3.Distance(transform.position, ball.position) > detectionRange)
            {
                // Move towards the ball if not too close
                Vector3 moveDirection = directionToBall * moveSpeed * Time.fixedDeltaTime;
                controller.Move(moveDirection);
            }
            else
            {
                // If close to the ball, shoot the ball towards the goal
                ShootBallTowardsGoal();
            }
        }
    }

    private void ShootBallTowardsGoal()
    {
        Rigidbody ballRb = ball.GetComponent<Rigidbody>();
        if (ballRb != null)
        {
            // Calculate shoot direction towards the goal post
            Vector3 shootDirection = (goalPost.position - ball.position).normalized;
            SoundManager.Instance.Play(Sounds.Ball);
            ballRb.AddForce(shootDirection * shootForce, ForceMode.Impulse);
        }
    }
    
}
