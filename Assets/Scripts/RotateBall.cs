using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class RotateBall : MonoBehaviour
{
    public float degrees;
    public Transform ball;

    void FixedUpdate()
    {
        transform.Rotate(new Vector3(0, 0, degrees));
        if(ball != null) ball.Rotate(new Vector3(0, 0, -degrees));
    }
}
