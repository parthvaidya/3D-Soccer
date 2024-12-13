using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;

public class BallController : MonoBehaviour
{
    public GameObject sparkEffectPrefab; // Drag your spark prefab here in the Inspector
    private GameObject currentSparkEffect;

    void OnTriggerEnter(Collider other)
    {
        //SoundManager.Instance.Play(Sounds.Ball);
        if (sparkEffectPrefab != null)
        {
            if (currentSparkEffect == null) // Ensure there's no duplicate effect
            {
                currentSparkEffect = Instantiate(sparkEffectPrefab, transform.position, Quaternion.identity);
                currentSparkEffect.transform.SetParent(transform); // Make the effect follow the ball
            }
        }
    }

    

    void ResetBall()
    {
        transform.position = Vector3.zero;
        GetComponent<Rigidbody>().velocity = Vector3.zero;
        if (currentSparkEffect != null)
        {
            Destroy(currentSparkEffect);
        }
    }
}
