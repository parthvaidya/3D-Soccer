using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GoalKeeperPlayer : MonoBehaviour
{
    public Transform ball; // Reference to the ball
    public float moveSpeed = 5f;
    public float maxPatrolSpeed = 7f;
    public float patrolRange = 5f;
    public Transform goalCenter; // The goal center for reference
    public float predictionFactor = 0.5f; // How far ahead to predict the ball's movement

    public Transform passTarget; // Reference to where the ball should be passed (Player)

    private Vector3 startPosition;

    private void Start()
    {
        startPosition = transform.position;
    }

    private void Update()
    {
        if (ball == null || goalCenter == null) return;

        float ballDistance = Vector3.Distance(ball.position, transform.position);

        if (ballDistance < 10f) // If the ball gets close
        {
            PredictAndBlock();
        }
        else
        {
            PatrolGoalArea();
        }
    }

    private void PredictAndBlock()
    {
        Rigidbody ballRb = ball.GetComponent<Rigidbody>();
        Vector3 predictedPosition = ball.position;

        // Predict the ball's future position
        if (ballRb != null)
        {
            predictedPosition += ballRb.velocity * predictionFactor;
        }

        Vector3 directionToBall = (predictedPosition - transform.position).normalized;
        Vector3 targetPosition = transform.position + directionToBall * moveSpeed * Time.deltaTime;

        // Preserve the original Y position
        targetPosition.y = startPosition.y;

        // Clamp movement within patrol range
        targetPosition.x = Mathf.Clamp(targetPosition.x, startPosition.x - patrolRange, startPosition.x + patrolRange);
        targetPosition.z = Mathf.Clamp(targetPosition.z, startPosition.z - patrolRange, startPosition.z + patrolRange);

        transform.position = targetPosition;

        // Intercept and pass the ball back to the player
        InterceptBall(predictedPosition);
    }

    private void PatrolGoalArea()
    {
        float patrolSpeed = Mathf.Lerp(moveSpeed, maxPatrolSpeed, 0.5f); // Dynamic patrol speed
        Vector3 patrolTarget = new Vector3(
            startPosition.x + Mathf.PingPong(Time.time * patrolSpeed, patrolRange * 2) - patrolRange,
            startPosition.y,
            startPosition.z
        );

        transform.position = patrolTarget;
    }

    private void InterceptBall(Vector3 predictedPosition)
    {
        if (ball != null)
        {
            Rigidbody ballRb = ball.GetComponent<Rigidbody>();

            // If the goalkeeper reaches the ball
            if (Vector3.Distance(ball.position, transform.position) < 1f)
            {
                // Apply force to pass the ball back to the player
                if (ballRb != null && passTarget != null)
                {
                    Vector3 passDirection = (passTarget.position - ball.position).normalized;
                    ballRb.AddForce(passDirection * moveSpeed, ForceMode.Impulse);
                }
            }
        }
    }
}
