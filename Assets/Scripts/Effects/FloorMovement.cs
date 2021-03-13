using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class FloorMovement : MonoBehaviour
{

    private const float MAX_HEIGHT = 0.2f;
    private const float MIN_HEIGHT = -0.2f;
    private const float SPEED_UP = 0.05f;
    private const float SPEED_DOWN = -0.05f;

    private float speed = 0.0f;

    // Start is called before the first frame update
    void Start()
    {
        speed = SPEED_UP;
    }

    // Update is called once per frame
    void Update()
    {
        transform.position = new Vector3(0.0f, transform.position.y + speed * Time.deltaTime, 0.0f);
        if (transform.position.y >= MAX_HEIGHT)
        {
            speed = SPEED_DOWN;
        }
        else if (transform.position.y <= MIN_HEIGHT)
        {
            speed = SPEED_UP;
        }
    }

}
