using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class TurnTowardsCamera : MonoBehaviour
{
    int frame = 0;
    int moveOnFrame = 10;

    private void FixedUpdate()
    {
        if (frame == moveOnFrame)
        {
            frame = 0;
            Vector3 target = new Vector3(Camera.main.transform.position.x, Camera.main.transform.position.y, Camera.main.transform.position.z);
            target.y = target.y + 0;
            transform.LookAt(target);
        } else
        {
            frame += 1;
        }
    }
}
