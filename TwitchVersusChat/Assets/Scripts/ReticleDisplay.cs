using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ReticleDisplay : MonoBehaviour
{
    public Transform ReticleRaw;
    public float MatchSpeed = 5f;

    void Update()
    {
        transform.position = ReticleRaw.position;
        transform.rotation = Quaternion.Lerp(transform.rotation, ReticleRaw.rotation, Time.deltaTime * MatchSpeed);
    }
}
