using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class FloorMovement : MonoBehaviour
{

    public float MaxHeight = 0.2f;
    public float MinHeight = -0.2f;
    public float SpeedUp = 0.05f;
    public float SpeedDown = -0.05f;
    public int frame = 0;
    public int UpdateOnFrame;

    private float speed = 0.0f;

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
            transform.position = new Vector3(0.0f, transform.position.y + UpdateOnFrame * speed * Time.deltaTime, 0.0f);
            if (transform.position.y >= MaxHeight)
            {
                speed = SpeedDown;
            }
            else if (transform.position.y <= MinHeight)
            {
                speed = SpeedUp;
            }
        }
        frame += 1;
    }

}
