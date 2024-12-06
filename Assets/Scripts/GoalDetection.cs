using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GoalDetection : MonoBehaviour
{
    [SerializeField] private ScoreController scoreController;

    private void OnTriggerEnter(Collider other)
    {

        Debug.Log("Collision with: " + other.gameObject.name);
        if (other.gameObject.CompareTag("Ball"))
        {
            scoreController.IncreaseScore(gameObject.tag, 1);
            SoundManager.Instance.Play(Sounds.Ball);
            Debug.Log("Ball detected in goal: " + gameObject.tag);
        }
        else
        {
            Debug.Log("Collision detected, but not with the ball. Object: " + other.name);
        }

    }
}
