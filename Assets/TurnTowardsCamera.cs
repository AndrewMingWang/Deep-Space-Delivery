using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class TurnTowardsCamera : MonoBehaviour
{
    private void FixedUpdate()
    {
        Vector3 target = new Vector3(Camera.main.transform.position.x, Camera.main.transform.position.y, Camera.main.transform.position.z);
        target.y = target.y + 0;
        transform.LookAt(target);
    }
}
