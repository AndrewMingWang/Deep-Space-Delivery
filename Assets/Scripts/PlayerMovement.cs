using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerMovement : MonoBehaviour
{

    public float speed = 2.0f;
    public Vector3 direction;
    public Animator Animator;

    private float _distToGround;

    // Start is called before the first frame update
    void Start()
    {
        direction = transform.forward;
        Animator = GetComponent<Animator>();
        _distToGround = GetComponent<Collider>().bounds.extents.y;
    }

    public bool IsGrounded()
    {
        Debug.DrawLine(transform.position, transform.position - Vector3.up * (_distToGround + 0.01f), Color.red, 1.0f);
        return Physics.Raycast(transform.position, -Vector3.up, _distToGround + 0.01f);
    }

    // Update is called once per frame
    void Update()
    {
        transform.position += speed * Time.deltaTime * direction;
        if (transform.position.y < -10.0f)
        {
            LosePlayer();
        }

        if (Animator.GetBool("isAirborne") && GetComponent<Rigidbody>().velocity.y < 0)
        {
            if (IsGrounded())
            {
                Animator.SetBool("isAirborne", false);
            }
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
