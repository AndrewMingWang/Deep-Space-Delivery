using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BasicRotationAnimation : MonoBehaviour
{
    public float RotationSpeed;
    public int RotateOnFrame;
    public int Frame = 0;

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
