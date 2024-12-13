using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GoalKeeper : MonoBehaviour
{
    

    public Transform ball;
    public float moveSpeed = 5f;
    public float maxPatrolSpeed = 7f;
    public float patrolRange = 5f;
    public Transform goalCenter;
    public float predictionFactor = 0.5f; // How far ahead to predict the ball's movement

    private Vector3 startPosition;

    private void Start()
    {
        startPosition = transform.position;
    }

    private void Update()
    {
        if (ball == null || goalCenter == null) return;

        float ballDistance = Vector3.Distance(ball.position, transform.position);

        if (ballDistance < 10f) // Adjust for when the ball gets close
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
}
