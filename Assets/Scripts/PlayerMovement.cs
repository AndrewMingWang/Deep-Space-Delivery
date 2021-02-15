using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerMovement : MonoBehaviour
{

    public float speed = 2.0f;
    public Vector3 direction;
    public Animator Animator;
    public bool WasGroundedLastFrame = false;
    public bool IsGroundedThisFrame = true;

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
        Debug.DrawLine(GetComponent<Collider>().bounds.center, GetComponent<Collider>().bounds.center - Vector3.up * (_distToGround + 0.05f), Color.red, 1.0f);
        return Physics.Raycast(GetComponent<Collider>().bounds.center, -Vector3.up, _distToGround + 0.05f);
    }

    // Update is called once per frame
    void Update()
    {
        transform.position += speed * Time.deltaTime * direction;
        if (transform.position.y < -10.0f)
        {
            LosePlayer();
        }

        Debug.Log(IsGrounded());
        IsGroundedThisFrame = IsGrounded();
        
        if (WasGroundedLastFrame && !IsGroundedThisFrame)
        {
            Debug.Log("airborne");
            Animator.SetBool("isAirborne", true);
            Animator.ResetTrigger("jump");
        } else if (!WasGroundedLastFrame && IsGroundedThisFrame && GetComponent<Rigidbody>().velocity.y < 0)
        {
            Debug.Log("landed");
            Animator.SetBool("isAirborne", false);
        }

        WasGroundedLastFrame = IsGroundedThisFrame;
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
