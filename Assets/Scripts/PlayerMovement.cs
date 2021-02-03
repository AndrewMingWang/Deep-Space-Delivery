using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerMovement : MonoBehaviour
{

    public float speed = 2.0f;
    public Vector3 direction = Vector3.forward;

    // Start is called before the first frame update
    void Start()
    {
        
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

    public void LosePlayer()
    {
        GameObject.FindGameObjectWithTag("goal").GetComponent<GoalTrigger>().playersFailed += 1;
        gameObject.SetActive(false);
    }
}
