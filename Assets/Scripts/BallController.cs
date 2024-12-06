using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;

public class BallController : MonoBehaviour
{
    

    void OnTriggerEnter(Collider other)
    {
        //SoundManager.Instance.Play(Sounds.Ball);
    }

    

    void ResetBall()
    {
        transform.position = Vector3.zero;
        GetComponent<Rigidbody>().velocity = Vector3.zero;
    }
}
