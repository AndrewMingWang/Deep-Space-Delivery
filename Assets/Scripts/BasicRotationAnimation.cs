using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BasicRotationAnimation : MonoBehaviour
{
    public bool RandomRotation = false;
    public float RotationSpeed;
    public int RotateOnFrame;
    public int Frame = 0;

    private void Start()
    {
        if (RandomRotation)
        {
            RotationSpeed = Random.Range(0.5f, 1.5f) * RotationSpeed;
        }
    }

    // Update is called once per frame
    void Update()
    {
        if (Frame == RotateOnFrame)
        {
            Frame = 0;
            transform.Rotate(Vector3.up, RotateOnFrame * RotationSpeed * Time.deltaTime);
        }
        Frame += 1;
    }
}
