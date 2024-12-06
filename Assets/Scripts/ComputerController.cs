using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ComputerController : MonoBehaviour
{
    //public Transform ball;
    //public Transform goalPost;
    //public float moveSpeed = 3f;
    //public float shootForce = 8f;
    //public float detectionRange = 1.5f; // Range to detect if AI is too close to the ball

    //private CharacterController controller;

    //private void Awake()
    //{
    //    controller = GetComponent<CharacterController>();
    //}

    //private void FixedUpdate()
    //{
    //    if (ball != null)
    //    {
    //        // Calculate direction to move towards the ball
    //        Vector3 directionToBall = (ball.position - transform.position).normalized;

    //        // Check if the AI is too close to the ball
    //        if (Vector3.Distance(transform.position, ball.position) > detectionRange)
    //        {
    //            // Move towards the ball if not too close
    //            Vector3 moveDirection = directionToBall * moveSpeed * Time.fixedDeltaTime;
    //            controller.Move(moveDirection);
    //        }
    //        else
    //        {
    //            // If close to the ball, shoot the ball towards the goal
    //            ShootBallTowardsGoal();
    //        }
    //    }
    //}

    //private void ShootBallTowardsGoal()
    //{
    //    Rigidbody ballRb = ball.GetComponent<Rigidbody>();
    //    if (ballRb != null)
    //    {
    //        // Calculate shoot direction towards the goal post
    //        Vector3 shootDirection = (goalPost.position - ball.position).normalized;
    //        SoundManager.Instance.Play(Sounds.Ball);
    //        ballRb.AddForce(shootDirection * shootForce, ForceMode.Impulse);
    //    }
    //}

    //private void GuideBallToGoal()
    //{
    //    // Calculate direction towards the goal post
    //    Vector3 directionToGoal = (goalPost.position - ball.position).normalized;

    //    // Move the AI in the same direction to "push" the ball
    //    Vector3 moveDirection = directionToGoal * moveSpeed * Time.fixedDeltaTime;
    //    controller.Move(moveDirection);

    //    // Apply a small force to the ball to simulate the "push"
    //    Rigidbody ballRb = ball.GetComponent<Rigidbody>();
    //    if (ballRb != null)
    //    {
    //        ballRb.AddForce(directionToGoal * moveSpeed * 0.5f, ForceMode.Force);
    //    }
    //}


    public Transform ball;
    public Transform goalPost;
    public float moveSpeed = 3f;
    public float shootForce = 8f;
    public float detectionRange = 1.5f; // Range to detect if AI is close to the ball
    public float ballReleaseCooldown = 1f; // Time in seconds to wait before chasing the ball again

    private CharacterController controller;
    private bool hasShotBall = false; // Tracks if the ball has been shot
    private float ballReleaseTimer = 0f;

    private void Awake()
    {
        controller = GetComponent<CharacterController>();
    }

    private void FixedUpdate()
    {
        if (ball != null)
        {
            // Handle cooldown after shooting the ball
            if (hasShotBall)
            {
                ballReleaseTimer += Time.fixedDeltaTime;

                if (ballReleaseTimer >= ballReleaseCooldown)
                {
                    hasShotBall = false; // Reset flag after cooldown
                    ballReleaseTimer = 0f;
                }
                return; // Skip further actions while in cooldown
            }

            // Calculate direction to move towards the ball
            Vector3 directionToBall = (ball.position - transform.position).normalized;

            // Check if the AI is too close to the ball
            if (Vector3.Distance(transform.position, ball.position) > detectionRange)
            {
                // Move towards the ball if not too close
                Vector3 moveDirection = directionToBall * moveSpeed * Time.fixedDeltaTime;
                controller.Move(moveDirection);

                // Rotate the AI to face the ball
                RotateTowards(ball.position);
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

            // Set flag to prevent immediate chasing
            hasShotBall = true;
        }
    }

    private void RotateTowards(Vector3 targetPosition)
    {
        Vector3 direction = (targetPosition - transform.position).normalized;
        direction.y = 0; // Ignore vertical rotation
        Quaternion targetRotation = Quaternion.LookRotation(direction);
        transform.rotation = Quaternion.Slerp(transform.rotation, targetRotation, Time.fixedDeltaTime * 5f); // Smooth rotation
    }

}
