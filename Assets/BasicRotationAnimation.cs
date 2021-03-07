﻿using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BasicRotationAnimation : MonoBehaviour
{
    public float RotationSpeed;

    // Update is called once per frame
    void Update()
    {
        transform.RotateAround(Vector3.up, RotationSpeed * Time.deltaTime);
    }
}
