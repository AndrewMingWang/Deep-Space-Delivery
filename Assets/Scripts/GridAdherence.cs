using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GridAdherence : MonoBehaviour
{
    // inspired by https://www.youtube.com/watch?v=eUFwxK9Z9aw

    private GameObject target;
    private RaycastHit hit;

    private Vector3 _truePos;
    public float gridSize = 1;

    // we use late update because we are overwriting a position and we want this to be the final version
    void LateUpdate()
    {
        if(Physics.Raycast(transform.position, Vector3.down, out hit, 10)){
            target = hit.transform.gameObject;
            _truePos.x = target.transform.position.x;
            _truePos.y = transform.position.y;
            _truePos.z = target.transform.position.z;
            transform.position = _truePos;
        }
    }
}
