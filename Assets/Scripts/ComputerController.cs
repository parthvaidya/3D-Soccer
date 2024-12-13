using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ComputerController : MonoBehaviour
{

    public Transform ball;
    public Transform goalPost;
    public Transform goalkeeper; // Assign the goalkeeper to avoid shooting directly at them
    public float moveSpeed = 3f;
    public float shootForce = 8f;
    public float detectionRange = 1.5f;
    public float ballReleaseCooldown = 1f;

    private CharacterController controller;
    private bool hasShotBall = false;
    private float ballReleaseTimer = 0f;

    private void Awake()
    {
        controller = GetComponent<CharacterController>();
    }

    private void FixedUpdate()
    {
        if (ball != null)
        {
            if (hasShotBall)
            {
                ballReleaseTimer += Time.fixedDeltaTime;
                if (ballReleaseTimer >= ballReleaseCooldown)
                {
                    hasShotBall = false;
                    ballReleaseTimer = 0f;
                }
                return;
            }

            Vector3 directionToBall = (ball.position - transform.position).normalized;

            if (Vector3.Distance(transform.position, ball.position) > detectionRange)
            {
                Vector3 moveDirection = directionToBall * moveSpeed * Time.fixedDeltaTime;
                controller.Move(moveDirection);

                RotateTowards(ball.position);
            }
            else
            {
                ShootBallTowardsWeakSpot();
            }
        }
    }

    private void ShootBallTowardsWeakSpot()
    {
        Rigidbody ballRb = ball.GetComponent<Rigidbody>();
        if (ballRb != null && goalkeeper != null)
        {
            // Calculate weak spot (shoot away from the goalkeeper's position)
            Vector3 directionToGoal = (goalPost.position - ball.position).normalized;
            Vector3 weakSpot = goalPost.position + (goalPost.position - goalkeeper.position).normalized * 2f;

            Vector3 shootDirection = (weakSpot - ball.position).normalized;
            ballRb.AddForce(shootDirection * shootForce, ForceMode.Impulse);

            hasShotBall = true;
        }
    }

    private void RotateTowards(Vector3 targetPosition)
    {
        Vector3 direction = (targetPosition - transform.position).normalized;
        direction.y = 0;
        Quaternion targetRotation = Quaternion.LookRotation(direction);
        transform.rotation = Quaternion.Slerp(transform.rotation, targetRotation, Time.fixedDeltaTime * 5f);
    }
}
