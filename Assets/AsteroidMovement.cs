using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class AsteroidMovement : MonoBehaviour
{

    private float xSpeed;
    private float zSpeed;

    // Start is called before the first frame update
    void Start()
    {
        xSpeed = Random.Range(0.5f, 1.0f);
        zSpeed = -1.0f * Random.Range(1.0f, 2.0f);
    }

    // Update is called once per frame
    void Update()
    {
        transform.position += new Vector3(xSpeed, 0.0f, zSpeed) * Time.deltaTime;
    }

}
