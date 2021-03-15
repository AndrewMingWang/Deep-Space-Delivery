using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class FloorMovement : MonoBehaviour
{

    private float MaxHeight = 0.1f;
    private float MinHeight = -0.1f;
    private float SpeedUp = 0.05f;
    private float SpeedDown = -0.05f;
    private int frame = 0;
    private int UpdateOnFrame = 10;

    private float speed;

    // Start is called before the first frame update
    void Start()
    {
        speed = SpeedUp;
    }

    // Update is called once per frame
    void Update()
    {
        if (frame == UpdateOnFrame)
        {
            frame = 0;
            if (transform.position.y >= MaxHeight)
            {
                speed = SpeedDown;
            }
            else if (transform.position.y <= MinHeight)
            {
                speed = SpeedUp;
            }
            transform.position = new Vector3(0.0f, transform.position.y + UpdateOnFrame * speed * Time.deltaTime, 0.0f);
        }
        frame += 1;
    }

}
