using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GoalKeeper : MonoBehaviour
{
    public Transform ball; // Assign the ball in the Inspector
    public float moveSpeed = 5f; // Speed of the goalkeeper
    public float patrolRange = 5f; // Range within which the goalkeeper moves
    public Transform goalCenter; // Center of the goal to patrol around

    private Vector3 startPosition;

    private void Start()
    {
        // Save the starting position to patrol around it
        startPosition = transform.position;
    }

    private void Update()
    {
        if (ball == null || goalCenter == null) return;

        float ballDistance = Vector3.Distance(ball.position, transform.position);

        // If the ball is close, move towards it to block
        if (ballDistance < 8f) // Adjust this range as needed
        {
            Vector3 directionToBall = (ball.position - transform.position).normalized;
            Vector3 targetPosition = transform.position + directionToBall * moveSpeed * Time.deltaTime;

            // Restrict movement within patrol range
            targetPosition.x = Mathf.Clamp(targetPosition.x, startPosition.x - patrolRange, startPosition.x + patrolRange);
            targetPosition.z = Mathf.Clamp(targetPosition.z, startPosition.z - patrolRange, startPosition.z + patrolRange);

            transform.position = targetPosition;
        }
        else
        {
            // If the ball is far, patrol around the goal
            PatrolGoalArea();
        }
    }

    private void PatrolGoalArea()
    {
        float patrolSpeed = moveSpeed * 0.5f; // Slower patrol speed
        Vector3 patrolTarget = new Vector3(
            startPosition.x + Mathf.PingPong(Time.time * patrolSpeed, patrolRange * 2) - patrolRange,
            startPosition.y,
            startPosition.z
        );

        transform.position = patrolTarget;
    }
}
