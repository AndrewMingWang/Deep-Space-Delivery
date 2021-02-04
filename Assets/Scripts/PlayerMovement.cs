using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerMovement : MonoBehaviour
{

    public float speed = 2.0f;
    public Vector3 direction;

    // Start is called before the first frame update
    void Start()
    {
        direction = transform.forward;
    }

    // Update is called once per frame
    void Update()
    {
        transform.position += speed * Time.deltaTime * direction;
        if (transform.position.y < -10.0f)
        {
            LosePlayer();
        }
    }

    // this will determine the next iterations direction, done in this way to allow fixedupdate to change direction field
    void LateUpdate(){
        direction = transform.forward;
    }

    public void LosePlayer()
    {
        GameObject.FindGameObjectWithTag("goal").GetComponent<GoalTrigger>().PlayersFailed += 1;
        gameObject.SetActive(false);
    }
}
